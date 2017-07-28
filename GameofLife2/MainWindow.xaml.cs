using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GameofLife2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(0.2);
            timer.Tick += Timer_Tick;

        }

        const int maxnumberofcells = 200;

        static int numberofcellshor = maxnumberofcells;
        static int numberofcellsvert = maxnumberofcells;

        public Rectangle[,] celltracking = new Rectangle[numberofcellsvert, numberofcellshor];
        Brush alive = Brushes.White;
        Brush dead = Brushes.Black;
        Random randomizer = new Random(123);

        DispatcherTimer timer = new DispatcherTimer();

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            
            if (int.TryParse(TextBoxdimvert.Text, out numberofcellsvert))
            {
                if (numberofcellsvert > maxnumberofcells || numberofcellsvert < 1)
                {
                    numberofcellsvert = maxnumberofcells;
                    TextBoxdimvert.Text = maxnumberofcells.ToString();
                }
            }
            if (int.TryParse(TextBoxdimhor.Text, out numberofcellshor))
            {
                if (numberofcellshor > maxnumberofcells || numberofcellshor < 1)
                {
                    numberofcellshor = maxnumberofcells;
                    TextBoxdimhor.Text = maxnumberofcells.ToString();
                }
            }
            double spacer = 3.0 / numberofcellshor;
            drawarea.Children.Clear();
            for (int i = 0; i < numberofcellsvert; i++)
            {
                for (int j = 0; j < numberofcellshor; j++)
                {
                    Rectangle r = new Rectangle();
                    r.Width = drawarea.ActualWidth / numberofcellshor-spacer;
                    r.Height = drawarea.ActualHeight / numberofcellsvert-spacer;
                    int deadoralive = randomizer.Next(0,100);
                    int alivepercentage;
                    if (int.TryParse(TextBoxrandom.Text, out alivepercentage))
                    {
                        if (alivepercentage < 0)
                        {
                            alivepercentage = 0;
                            TextBoxrandom.Text = "0";
                        }
                        else if (alivepercentage > 100)
                        {
                            alivepercentage = 100;
                            TextBoxrandom.Text = "100";
                        }
                    }
                    r.Fill = deadoralive>=Int32.Parse(TextBoxrandom.Text) ? dead : alive;
                    drawarea.Children.Add(r);
                    Canvas.SetLeft(r, j * drawarea.ActualWidth / numberofcellshor);
                    Canvas.SetTop(r, i * drawarea.ActualHeight / numberofcellsvert);
                    r.MouseDown += R_MouseDown;

                    celltracking[i, j] = r;

                }
            }
        }

        private void R_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ((Rectangle)sender).Fill = (((Rectangle)sender).Fill == alive) ? dead : alive;

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int maxneighbors;
            int minneighbors;
            int createnew;
            int.TryParse(TextBoxminnr.Text, out minneighbors);
            int.TryParse(TextBoxmaxnr.Text, out maxneighbors);
            int.TryParse(TextBoxalivenr.Text, out createnew);
           
            int[,] neighbors = new int[numberofcellsvert,numberofcellshor];
            for (int i = 0; i < (numberofcellsvert ); i++)
            {
                for (int j = 0; j < (numberofcellshor ); j++)
                {
                    int iup = i + 1;
                    int idown = i - 1;
                    int jup = j + 1;
                    int jdown = j - 1;
                    if (iup == numberofcellsvert)
                    {
                        iup = 0;
                    }
                    if (idown <0)
                    {
                        idown =numberofcellsvert - 1;
                    }
                    if (jup == numberofcellshor)
                    {
                        jup = 0;
                    }
                    if (jdown <0)
                    {
                        jdown = numberofcellshor - 1;
                    }
                    int n = 0;
                    if (celltracking[idown, jdown].Fill == alive)
                    {
                        n++;
                    }
                    if (celltracking[idown, j].Fill == alive)
                    {
                        n++;
                    }
                    if (celltracking[idown, jup].Fill == alive)
                    {
                        n++;
                    }
                    if (celltracking[i, jdown].Fill == alive)
                    {
                        n++;
                    }
                    if (celltracking[i, jup].Fill == alive)
                    {
                        n++;
                    }
                    if (celltracking[iup,jup].Fill == alive)
                    {
                        n++;
                    }
                    if (celltracking[iup, j].Fill == alive)
                    {
                        n++;
                    }
                    if (celltracking[iup, jdown].Fill == alive)
                    {
                        n++;
                    }
                   neighbors[i, j] = n;
                }
            }
            for (int i = 0; i < numberofcellsvert ; i++)
            {
                for (int j = 0; j < numberofcellshor  ; j++)
                {
                    if (neighbors[i,j] > maxneighbors || neighbors[i,j] < minneighbors)
                    {
                        celltracking[i, j].Fill = dead;
                    }
                    else if (neighbors[i,j] == createnew)
                    {
                        celltracking[i, j].Fill = alive;
                    }
                }
            }
        }

        bool running = false;

        private void ButtonTimer_Click(object sender, RoutedEventArgs e)
        {
            double timerintervall;
            if (Double.TryParse(TextBoxtimeintervall.Text, out timerintervall))
            {
                if (timerintervall < 0)
                {
                    timerintervall = 0.1;
                    TextBoxtimeintervall.Text = "0,1";
                }
            }
            timer.Interval = TimeSpan.FromSeconds(timerintervall);
            if (running)
            {
                timer.Stop();
                running = false;
                ButtonStartStop.Content = "Start Timer";
            }
            else
            {
                timer.Start();
                running = true;
                ButtonStartStop.Content = "Stop Timer";
            }
        }

        private void ButtonKill_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < numberofcellsvert; i++)
            {
                for (int j = 0; j < numberofcellshor; j++)
                {
                    celltracking[i, j].Fill = dead;
                }
            }
        }
    }
}
