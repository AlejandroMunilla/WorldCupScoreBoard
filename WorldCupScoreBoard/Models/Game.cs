namespace SportRadar.WorldCupScoreBoard.Models
{
    public class Game
    {
        private int idGame;
        private DateTime lastUpdate;
        private string homeTeamName = string.Empty;
        private string awayTeamName = string.Empty;
        private int awayTeamScore = 0;
        private int homeTeamScore = 0;


        public int IdGame
        {
            get { return idGame; }
            set { idGame = value; }
        }

        public DateTime LastUpdate
        { 
            get { return lastUpdate; }
            set { lastUpdate = value; }
        }

        public string HomeTeamName
        {
            get { return homeTeamName; }
            set { homeTeamName = value; }
        }

        public string AwayTeamName
        {
            get { return awayTeamName; }
            set { awayTeamName = value; }
        }

        public int AwayScore
        {
            get { return awayTeamScore; }
            set { awayTeamScore = value; }
        }

        public int HomeScore
        {
            get { return homeTeamScore; }
            set { homeTeamScore = value; }
        }

        public int TotalScore
        {
            get { return (homeTeamScore + awayTeamScore);  }
        }
    }
}
