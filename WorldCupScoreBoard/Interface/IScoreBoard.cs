using SportRadar.WorldCupScoreBoard.Models;


namespace SportRadar.WorldCupScoreBoard.Interface
{
    public interface IScoreBoard
    {
        public ResponseContainer StartGame(string homeTeam, string awayTeam);
        public ResponseContainer FinishGame(int idGame);
        public ResponseContainer UpdateGame(int idGame, int homeTeamScore, int awayTeamScore);
        public List<Game> SummaryGamesList();

    }
}
