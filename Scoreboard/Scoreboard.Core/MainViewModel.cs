using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Scoreboard.Core
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private HubConnection hubConnection;
        public ObservableCollection<string> messages = new ObservableCollection<string>();
        private string _awayName;
        public string awayName
        {
            get { return _awayName; }
            set { _awayName = value; OnPropertyChanged(); }
        }
        private string _homeName;

        public string homeName
        {
            get { return _homeName; }
            set { _homeName = value; OnPropertyChanged(); }
        }
        private Team _home;

        public Team home
        {
            get { return _home; }
            set { _home = value; OnPropertyChanged(); }
        }
        private Team _away;

        public Team away
        {
            get { return _away; }
            set { _away = value; OnPropertyChanged(); }
        }
        private Teams _teams = new Teams();

        public Teams teams
        {
            get { return _teams; }
            set { _teams = value; OnPropertyChanged(); }
        }
        private bool _hideButtons;

        public bool hideButtons
        {
            get { return _hideButtons; }
            set { _hideButtons = value; OnPropertyChanged(); }
        }

        public ICommand AwayChangedCommand { get; set; }
        public ICommand HomeChangedCommand { get; set; }
        public ICommand AwayScoreCommand { get; set; }
        public ICommand HomeScoreCommand { get; set; }
        public ICommand AwayResetCommand { get; set; }
        public ICommand HomeResetCommand { get; set; }
        public MainViewModel()
        {
            AwayChangedCommand = new DelegateCommand<Team>(OnAwayChanged);
            HomeChangedCommand = new DelegateCommand<Team>(OnHomeChanged);
            AwayScoreCommand = new DelegateCommand<object>(async i => await AwayScore(int.Parse(i.ToString())), (i) => true);
            HomeScoreCommand = new DelegateCommand<object>(async i => await HomeScore(int.Parse(i.ToString())), (i) => true);
            AwayResetCommand = new DelegateCommand(() => { if (away!= null) away.Score = 0; });
            HomeResetCommand = new DelegateCommand(() => { if (home != null) home.Score = 0; });
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public async Task Initialize(string accessToken, string serverUrl)
        {
            try
            {
                hubConnection = new HubConnectionBuilder()
                        .WithUrl(serverUrl, options =>
                        {
                            options.AccessTokenProvider = () => Task.FromResult(accessToken);
                        })
                    .Build();
                hubConnection.On<string, string>("ScoreUpdate", ScoreUpdate);
                hubConnection.On<string>("HomeUpdate", HomeUpdate);
                hubConnection.On<string>("AwayUpdate", AwayUpdate);

                await hubConnection.StartAsync();
            }
            catch { }
        }
        private void ScoreUpdate(string home , string away)
        {
            if (this.home == null || this.away == null) return;
            int homeScore, awayScore;
            int.TryParse(home, out homeScore);
            int.TryParse(away, out awayScore);
            this.home.Score = homeScore;
            this.away.Score = awayScore;
            var encodedMsg = $"Home {home}: Away {away}";
            messages.Add(encodedMsg);
        }
        private void HomeUpdate(string homeName)
        {
            this.homeName = homeName;
            if (!string.IsNullOrEmpty(homeName))
            {
                home = teams[homeName];
            }
        }
        private void AwayUpdate(string awayName)
        {
            this.awayName = awayName;
            if (!string.IsNullOrEmpty(awayName))
            {
                away = teams[awayName];
            }
        }

        public async void OnAwayChanged(Team team)
        {
            await AwayChanged(team.Name);
        }
        public async Task AwayChanged(string awayName)
        {
            this.awayName = awayName;
            if (!string.IsNullOrEmpty(awayName))
            {
                away = teams[awayName];
                if (hubConnection != null)
                {
                    await hubConnection.SendAsync("AwayUpdate", awayName);
                }
            }
        }
        public async void OnHomeChanged(Team team)
        {
            await HomeChanged(((Team)team).Name);
        }
        public async Task HomeChanged(string homeName)
        {
            this.homeName = homeName;
            if (!string.IsNullOrEmpty(homeName))
            {
                home = teams[homeName];
                if (hubConnection != null)
                {
                    await hubConnection.SendAsync("HomeUpdate", homeName);
                }
            }
        }
        public async Task Send()
        {
            if (hubConnection != null)
            {
                await hubConnection.SendAsync("ScoreUpdate", home?.Score.ToString(), away?.Score.ToString());
            }
        }
        public async Task AwayScore(int points)
        {
            if (away == null) return;
            away.Score += points;
            await Send();
        }
        public async Task HomeScore(int points)
        {
            if (home == null) return;
            home.Score += points;
            await Send();
        }
        public bool IsConnected =>
            hubConnection?.State == HubConnectionState.Connected;

        public void ToggleButtons()
        {
            hideButtons = !hideButtons;
        }
        public async ValueTask DisposeAsync()
        {
            if (hubConnection != null)
            {
                await hubConnection.DisposeAsync();
            }
        }
    }
}
