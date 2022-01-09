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
            Add("New England", new Team() { Name = "New England", Abrev = "NE", ImageNum = "44", BackgroundColor = "01224B", ForegroundColor = "FFFFFF"});
            Add("Buffalo", new Team() { Name = "Buffalo", Abrev = "BUF", ImageNum = "61", BackgroundColor = "143D73", ForegroundColor = "FFFFFF" });
            Add("Miami", new Team() { Name = "Miami", Abrev = "MIA", ImageNum = "64", ForegroundColor = "016D6D", BackgroundColor = "DA6B38" });
            Add("NY Jets", new Team() { Name = "NY Jets", Abrev = "NYJ", ImageNum = "51" , BackgroundColor = "055032", ForegroundColor = "FFFFFF" });
            Add("Baltimore", new Team() { Name = "Baltimore", Abrev = "BAL", ImageNum = "10" , BackgroundColor = "000000", ForegroundColor = "FFFFFF"});
            Add("Cincinnati", new Team() { Name = "Cincinnati", Abrev = "CIN", ImageNum = "11", BackgroundColor = "F36A26", ForegroundColor = "000000" });
            Add("Pittsburgh", new Team() { Name = "Pittsburgh", Abrev = "PIT", ImageNum = "12", BackgroundColor = "000000", ForegroundColor = "FFFFFF" });
            Add("Cleveland", new Team() { Name = "Cleveland", Abrev = "CLE", ImageNum = "13" , BackgroundColor = "663514", ForegroundColor = "FFFFFF" });
            Add("Tennessee", new Team() { Name = "Tennessee", Abrev = "TEN", ImageNum = "14", BackgroundColor = "32A4DA", ForegroundColor = "FFFFFF" });
            Add("Indianapolis", new Team() { Name = "Indianapolis", Abrev = "IND", ImageNum = "50", BackgroundColor = "00368C", ForegroundColor = "FFFFFF" });
            Add("Houston", new Team() { Name = "Houston", Abrev = "HOU", ImageNum = "04", BackgroundColor = "00133E", ForegroundColor = "FFFFFF" });
            Add("Jacksonville", new Team() { Name = "Jacksonville", Abrev = "JAC", ImageNum = "53" , BackgroundColor = "018399", ForegroundColor = "000000" });
            Add("Kansas City", new Team() { Name = "Kansas City", Abrev = "KC", ImageNum = "30", BackgroundColor = "C30011", ForegroundColor = "FFFFFF" });
            Add("LA Chargers", new Team() { Name = "LA Chargers", Abrev = "LAC", ImageNum = "03", BackgroundColor = "02275C", ForegroundColor = "FFFFFF" });
            Add("Las Vegas", new Team() { Name = "Las Vegas", Abrev = "LV", ImageNum = "31" ,BackgroundColor = "BEBEBE", ForegroundColor = "FFFFFF" });
            Add("Denver", new Team() { Name = "Denver", Abrev = "DEN", ImageNum = "52" , BackgroundColor = "F46A22", ForegroundColor = "FFFFFF" });
            Add("Dallas", new Team() { Name = "Dallas", Abrev = "DAL", ImageNum = "23", BackgroundColor = "BEBEBE", ForegroundColor = "FFFFFF" });
            Add("Washington", new Team() { Name = "Washington", Abrev = "WAS", ImageNum = "21" , BackgroundColor = "7E1315", ForegroundColor = "FFFFFF" });
            Add("Philadelphia", new Team() { Name = "Philadelphia", Abrev = "PHI", ImageNum = "22", BackgroundColor = "004049", ForegroundColor = "FFFFFF"});
            Add("NY Giants", new Team() { Name = "NY Giants", Abrev = "NYG", ImageNum = "20" , BackgroundColor = "003B7D", ForegroundColor = "FFFFFF" });
            Add("Green Bay", new Team() { Name = "Green Bay", Abrev = "GB", ImageNum = "24" , BackgroundColor = "367141", ForegroundColor="FFFFFF"});
            Add("Minnesota", new Team() { Name = "Minnesota", Abrev = "MIN", ImageNum = "33" , BackgroundColor = "1A065B", ForegroundColor = "FFFFFF" });
            Add("Chicago", new Team() { Name = "Chicago", Abrev = "CHI", ImageNum = "32", BackgroundColor = "151635", ForegroundColor = "FFFFFF" });
            Add("Detroit", new Team() { Name = "Detroit", Abrev = "DET", ImageNum = "62", BackgroundColor = "026AB1", ForegroundColor = "FFFFFF" });
            Add("Tampa Bay", new Team() { Name = "Tampa Bay", Abrev = "TB", ImageNum = "43", BackgroundColor = "706051", ForegroundColor = "FFFFFF" });
            Add("Atlanta", new Team() { Name = "Atlanta", Abrev = "ATL", ImageNum = "34", BackgroundColor = "B50E20", ForegroundColor = "FFFFFF" });
            Add("Carolina", new Team() { Name = "Carolina", Abrev = "CAR", ImageNum = "41", BackgroundColor = "D3D3D5", ForegroundColor = "000000" });
            Add("New Orleans", new Team() { Name = "New Orleans", Abrev = "NO", ImageNum = "40", BackgroundColor = "C1A746", ForegroundColor = "000000" });
            Add("Arizona", new Team() { Name = "Arizona", Abrev = "ARZ", ImageNum = "00", BackgroundColor = "AE0639", ForegroundColor = "FFFFFF" });
            Add("LA Rams", new Team() { Name = "LA Rams", Abrev = "LAR", ImageNum = "42", BackgroundColor = "002859", ForegroundColor = "" });
            Add("San Francisco", new Team() { Name = "San Francisco", Abrev = "SF", ImageNum = "02", BackgroundColor= "BF9A4C", ForegroundColor = "FFFFFF" });
            Add("Seattle", new Team() { Name = "Seattle", Abrev = "SEA", ImageNum = "01", BackgroundColor = "2B597E", ForegroundColor = "FFFFFF" });
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

        private string backgroundColor;
        public string BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        private string foregroundColor;
        public string ForegroundColor
        {
            get { return foregroundColor; }
            set { foregroundColor = value; }
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}

