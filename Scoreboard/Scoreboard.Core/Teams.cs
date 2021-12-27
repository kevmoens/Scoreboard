using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Scoreboard.Core
{
    public class Teams : Dictionary<string, Team>
    {
        public Teams()
        {
            Add("New England", new Team() { Name = "New England", Abrev = "NE", ImageNum = "44" });
            Add("Buffalo", new Team() { Name = "Buffalo", Abrev = "BUF", ImageNum = "61" });
            Add("Miami", new Team() { Name = "Miami", Abrev = "MIA", ImageNum = "64" });
            Add("NY Jets", new Team() { Name = "NY Jets", Abrev = "NYJ", ImageNum = "51" });
            Add("Baltimore", new Team() { Name = "Baltimore", Abrev = "BAL", ImageNum = "10" });
            Add("Cincinnati", new Team() { Name = "Cincinnati", Abrev = "CIN", ImageNum = "11" });
            Add("Pittsburgh", new Team() { Name = "Pittsburgh", Abrev = "PIT", ImageNum = "12" });
            Add("Cleveland", new Team() { Name = "Cleveland", Abrev = "CLE", ImageNum = "13" });
            Add("Tennessee", new Team() { Name = "Tennessee", Abrev = "TEN", ImageNum = "14" });
            Add("Indianapolis", new Team() { Name = "Indianapolis", Abrev = "IND", ImageNum = "50" });
            Add("Houston", new Team() { Name = "Houston", Abrev = "HOU", ImageNum = "04" });
            Add("Jacksonville", new Team() { Name = "Jacksonville", Abrev = "JAC", ImageNum = "53" });
            Add("Kansas City", new Team() { Name = "Kansas City", Abrev = "KC", ImageNum = "30" });
            Add("LA Chargers", new Team() { Name = "LA Chargers", Abrev = "LAC", ImageNum = "03" });
            Add("Las Vegas", new Team() { Name = "Las Vegas", Abrev = "LV", ImageNum = "31" });
            Add("Denver", new Team() { Name = "Denver", Abrev = "DEN", ImageNum = "52" });
            Add("Dallas", new Team() { Name = "Dallas", Abrev = "DAL", ImageNum = "23" });
            Add("Washington", new Team() { Name = "Washington", Abrev = "WAS", ImageNum = "21" });
            Add("Philadelphia", new Team() { Name = "Philadelphia", Abrev = "PHI", ImageNum = "22" });
            Add("NY Giants", new Team() { Name = "NY Giants", Abrev = "NYG", ImageNum = "20" });
            Add("Green Bay", new Team() { Name = "Green Bay", Abrev = "GB", ImageNum = "24" });
            Add("Minnesota", new Team() { Name = "Minnesota", Abrev = "MIN", ImageNum = "33" });
            Add("Chicago", new Team() { Name = "Chicago", Abrev = "CHI", ImageNum = "32" });
            Add("Detroit", new Team() { Name = "Detroit", Abrev = "DET", ImageNum = "62" });
            Add("Tampa Bay", new Team() { Name = "Tampa Bay", Abrev = "TB", ImageNum = "43" });
            Add("Atlanta", new Team() { Name = "Atlanta", Abrev = "ATL", ImageNum = "34" });
            Add("Carolina", new Team() { Name = "Carolina", Abrev = "CAR", ImageNum = "41" });
            Add("New Orleans", new Team() { Name = "New Orleans", Abrev = "NO", ImageNum = "40" });
            Add("Arizona", new Team() { Name = "Arizona", Abrev = "ARZ", ImageNum = "00" });
            Add("LA Rams", new Team() { Name = "LA Rams", Abrev = "LAR", ImageNum = "42" });
            Add("San Francisco", new Team() { Name = "San Francisco", Abrev = "SF", ImageNum = "02" });
            Add("Seattle", new Team() { Name = "Seattle", Abrev = "SEA", ImageNum = "01" });
        }

    }
    public class Team : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        private string _Abrev;

        public string Abrev
        {
            get { return _Abrev; }
            set { _Abrev = value; OnPropertyChanged(); }
        }
        private string _ImageNum;

        public string ImageNum
        {
            get { return _ImageNum; }
            set { _ImageNum = value; OnPropertyChanged(); }
        }
        private int _Score;

        public int Score
        {
            get { return _Score; }
            set { _Score = value; OnPropertyChanged(); }
        }


        public override string ToString()
        {
            return $"{Name}";
        }
    }
}

