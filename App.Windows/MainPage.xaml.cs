using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region variables
        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer spielertimer = new DispatcherTimer();

        Stopwatch watch = new Stopwatch();
        Stopwatch jumptime = new Stopwatch();

        CompositeTransform kreistransform;
        CompositeTransform spielertransform;
        Game game;

        double distance = 300;
        bool spielermoving = false;
        double max;
        int count;
        string inhalt;
      

        SolidColorBrush gelb = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 0));
        SolidColorBrush blau = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 255));
        SolidColorBrush grün = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 255, 0));
        SolidColorBrush rot = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0));


        #endregion

        public MainPage()
        {
            this.InitializeComponent();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, object e)
        {
            if (!game.Dead)
            {
                game.Update(watch.ElapsedMilliseconds);
                SpielerGravitation();
                SetSpielerFarbe();
                watch.Restart();



                kreistransform.TranslateY = game.Kreis.Y;
                kreistransform.Rotation = game.Rotation;
                ScoreBox.Text = game.Score + " Punkte";

            }
            else
            {
                timer.Stop();
                spielertimer.Stop();
                Dead.Visibility = Visibility.Visible;
                Kreis.Visibility = Visibility.Collapsed;
                Spieler.Visibility = Visibility.Collapsed;
                Datenspeichern(game.Score);
            }


        }

        public async void Datenspeichern(int score)
        {
            DatenHohlen();
            int current = int.Parse(inhalt);
          
            if (current < score)
            {
                StorageFolder Ordner = ApplicationData.Current.LocalFolder;
                StorageFile scorefile = await Ordner.CreateFileAsync("Highscore.txt", CreationCollisionOption.OpenIfExists);
                await FileIO.WriteTextAsync(scorefile, score.ToString());
            }                  
        }

        public async void DatenHohlen()
        {
            StorageFolder Ordner = ApplicationData.Current.LocalFolder;
            StorageFile score = await Ordner.GetFileAsync("Highscore.txt");
            inhalt = await FileIO.ReadTextAsync(score);
            Highscore.Text = "Highscore : " + inhalt;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            game = new Game(
                Window.Current.Bounds.Width,
                Window.Current.Bounds.Height);

            Kreis.Width = Game.RADIUS * 2;
            Kreis.Height = Game.RADIUS * 2;
            Canvas.SetLeft(Kreis, -Game.RADIUS);
            Canvas.SetTop(Kreis, -Game.RADIUS);

            Spieler.Width = Game.RADIUSSPIELER * 2;
            Spieler.Height = Game.RADIUSSPIELER * 2;
            Canvas.SetLeft(Spieler, -Game.RADIUSSPIELER);
            Canvas.SetTop(Spieler, -Game.RADIUSSPIELER);

            kreistransform = Kreis.RenderTransform as CompositeTransform;
            kreistransform.TranslateY = game.Kreis.Y;
            kreistransform.TranslateX = game.Kreis.X;

            spielertransform = Spieler.RenderTransform as CompositeTransform;
            spielertransform.TranslateX = game.Spieler.X;
            spielertransform.TranslateY = game.Spieler.Y;


            watch.Restart();
            timer.Start();

            DatenHohlen();   
        }

        private void Jump(object sender, RoutedEventArgs e)
        {
            jumptime.Restart();
            spielertimer.Start();
            spielertimer.Tick += Spielertimer_Tick;
            max = game.Spieler.Y - distance;


        }

        private void Spielertimer_Tick(object sender, object e)
        {
            if (game.Spieler.Y > 25)
            {


                if (game.Spieler.Y >= max)
                {
                    spielermoving = true;
                    game.Jumping(jumptime.ElapsedMilliseconds);
                    jumptime.Restart();
                    spielertransform.TranslateY = game.Spieler.Y;

                }
                else
                {
                    spielertimer.Stop();
                    spielermoving = false;
                }
            }
            else
            {
                spielertimer.Stop();
                spielermoving = false;
            }

        }

        public void SpielerGravitation()
        {

            if (!spielermoving)
            {
                double grenze = game.height / 3 * 2;
                if (grenze >= game.Spieler.Y)
                {
                    game.Falling(watch.ElapsedMilliseconds);
                    spielertransform.TranslateY = game.Spieler.Y;
                }

            }
        }
        
        public void SetSpielerFarbe()
        {
            count++;
            if (count > 900)
            {
                count = 0;
                game.Spielerfarbe++;
                if (game.Spielerfarbe > 4)
                {
                    game.Spielerfarbe = 1;
                }

                switch (game.Spielerfarbe)
                {
                    case 1:
                        Spieler.Fill = blau;
                        break;
                    case 2:
                        Spieler.Fill = rot;
                        break;
                    case 3:
                        Spieler.Fill = gelb;
                        break;
                    case 4:
                        Spieler.Fill = grün
                        ; break;

                    default:
                        break;
                }
            }

        }

        private void Hauptmenü_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage1));
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            spielertimer.Stop();
            timer.Stop();
            watch.Stop();
            jumptime.Stop();

            Spieler.Opacity = 0.7;
            Kreis.Opacity = 0.7;
            Resume.Visibility = Visibility.Visible;
            Pause.Visibility = Visibility.Collapsed;
        }

        private void Resume_Click(object sender, RoutedEventArgs e)
        {
            spielertimer.Start();
            timer.Start();
            watch.Restart();
            jumptime.Restart();
            Spieler.Opacity = 1;
            Kreis.Opacity = 1;
            Resume.Visibility = Visibility.Collapsed;
            Pause.Visibility = Visibility.Visible;

        }
    }

}
