using Microsoft.AspNetCore.SignalR.Client;
using Scoreboard.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Scoreboard.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Compositor _compositor = Window.Current.Compositor;
        private SpringVector3NaturalMotionAnimation _springAnimation;
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            PubSub.Hub.Default.Subscribe<ScoreMessage>(OnTeamScored);
            UpdateSpringAnimation();



        }
        public async void OnTeamScored(ScoreMessage msg)
        {
            await Task.Delay(10);
            if (msg.IsHome)
            {
                HomeScore.Height = Home.ActualHeight;
                HomeScore.Width = Home.ActualWidth;
                HomeScore.VerticalAlignment = VerticalAlignment.Center;

                await ScoreAnimation(HomeImage, HomeTextBlock, HomeViewbox, msg, Away, AwayImage, AwayTextblock);
            } else
            {
                AwayScore.Height = Away.ActualHeight;
                AwayScore.Width = Away.ActualWidth;
                AwayScore.VerticalAlignment = VerticalAlignment.Center;

                await ScoreAnimation(AwayImage, AwayTextblock, AwayViewbox, msg, Home, HomeImage, HomeTextBlock);

            }
        }

        private async Task ScoreAnimation(Image scoringImage, TextBlock scoringTextblock, Viewbox scoringViewbox, ScoreMessage msg, Grid nonScoringTeam, Image nonScoreImage, TextBlock nonscoringTextBlock)
        {

            await MajorScore(scoringImage, scoringTextblock, msg, nonScoringTeam, nonScoreImage);

            await BouncingScore(scoringViewbox);
        }

        private async Task BouncingScore(Viewbox scoringViewbox)
        {
            //Show Number Bouncing
            Storyboard boucingStoryboard = new Storyboard();
            BouncingScore(scoringViewbox, boucingStoryboard);
            boucingStoryboard.Begin();
            scoringViewbox.StartAnimation(_springAnimation);
            await Task.Delay(3000);
            boucingStoryboard.Stop();
        }

        private async Task MajorScore(Image scoringImage, TextBlock scoringTextblock, ScoreMessage msg, Grid nonScoringTeam, Image nonScoreImage)
        {
            //Hide non-scoring team 
            if (msg.Points == 3 || msg.Points == 6)
            {
                nonScoringTeam.Visibility = Visibility.Collapsed;
                nonScoreImage.Visibility = Visibility.Collapsed;
                //Slide Scoring Team up and to left (smaller
                Team team = ((Team)scoringImage.Tag);
                byte r = 36;
                byte g = 66;
                byte b = 195;
                ScoreRectangleGetColor(team, ref r, ref g, ref b);
                ScoreRectangle.Fill = new SolidColorBrush(Color.FromArgb(255, r, g, b));

                ScoreTextGetColor(team, ref r, ref g, ref b);
                scoringTextblock.Foreground = new SolidColorBrush(Color.FromArgb(255, r, g, b));

                ScoreRectangleGetText(msg);
                ScoreRectangle.Visibility = Visibility.Visible;
                ScoreText.Visibility = Visibility.Visible;

                //Slide Scoring Team up and to left (smaller
                if (msg.IsHome)
                {
                    Storyboard Storyboard1 = scoringImage.Resources["Storyboard1"] as Storyboard;
                    var ttv = scoringImage.TransformToVisual(Window.Current.Content);
                    Point screenCoords = ttv.TransformPoint(new Point(0, 0));
                    Storyboard1.Children[0].SetValue(DoubleAnimation.FromProperty, Translation1.X);
                    Storyboard1.Children[0].SetValue(DoubleAnimation.ToProperty, -(screenCoords.X));
                    Storyboard1.Begin();
                    //Wait
                    await Task.Delay(4000);
                    Storyboard1.Stop();
                }
                else
                {
                    AwayViewboxScoreAnimation.Visibility = Visibility.Visible;
                    scoringTextblock.Visibility = Visibility.Collapsed;
                    //Wait
                    await Task.Delay(4000);
                    scoringTextblock.Visibility = Visibility.Visible;
                }
            }
            //Reset
            ScoreText.Visibility = Visibility.Collapsed;
            ScoreRectangle.Visibility = Visibility.Collapsed;
            AwayViewboxScoreAnimation.Visibility = Visibility.Collapsed;
            nonScoringTeam.Visibility = Visibility.Visible;
            nonScoreImage.Visibility = Visibility.Visible;
        }

        private void ScoreRectangleGetText(ScoreMessage msg)
        {
            if (msg.Points == 6)
            {
                ScoreText.Text = "TOUCHDOWN";
            }
            else
            {
                ScoreText.Text = "FIELD GOAL";
            }
        }

        private static void ScoreRectangleGetColor(Team team, ref byte r, ref byte g, ref byte b)
        {
            if (!string.IsNullOrEmpty(team.BackgroundColor))
            {
                var rgb = StringToByteArray(team.BackgroundColor);
                r = rgb[0];
                g = rgb[1];
                b = rgb[2];
            }
        }
        private static void ScoreTextGetColor(Team team, ref byte r, ref byte g, ref byte b)
        {
            r = 255;
            g = 255;
            b = 255;

            if (!string.IsNullOrEmpty(team.ForegroundColor))
            {
                var rgb = StringToByteArray(team.ForegroundColor);
                r = rgb[0];
                g = rgb[1];
                b = rgb[2];
            }
        }

        private static void BouncingScore(Viewbox scoringViewbox, Storyboard boucingStoryboard)
        {
            boucingStoryboard.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            DoubleAnimation opacityAnimation = new DoubleAnimation()
            {
                From = 1.0,
                To = 0.0,
                BeginTime = TimeSpan.FromMilliseconds(0),
                Duration = new Duration(TimeSpan.FromMilliseconds(200.0)),
                EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseInOut, Power = 7 },
            };

            Storyboard.SetTarget(opacityAnimation, scoringViewbox);
            Storyboard.SetTargetProperty(opacityAnimation, "Opacity");
            boucingStoryboard.Children.Add(opacityAnimation);
            boucingStoryboard.RepeatBehavior = RepeatBehavior.Forever;
            boucingStoryboard.AutoReverse = true;
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        private void UpdateSpringAnimation()
        {
            if (_springAnimation == null)
            {
                _springAnimation = _compositor.CreateSpringVector3Animation();
                _springAnimation.Target = "Scale";
            }
            _springAnimation.InitialValue = new Vector3(0.1f, 0.1f, 0.1f);
            _springAnimation.FinalValue = new Vector3(1f);
            _springAnimation.DampingRatio = 0.2f;
            _springAnimation.Period = TimeSpan.FromMilliseconds(200);
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //await ((MainViewModel)DataContext).Initialize("Endpoint=https://scoreboard.service.signalr.net;AccessKey=1wnnrH4YcO+CxyPqAEdkQ5wz0q2HetjIADpB8yYdRR8=;Version=1.0;", "https://scoreboard.service.signalr.net/hubs");
            HttpServer server = new HttpServer(1265);
            server.ViewModel = (MainViewModel)DataContext;

            try
            {
                MRBackgroundImage.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("https://gopackgo.s3.us-east-2.amazonaws.com/MR.jpg", UriKind.Absolute));
            }
            catch (Exception ex )
            { 
                System.Diagnostics.Debug.Print(ex.Message); 
            }
        }

    }
}
