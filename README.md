# WorldCupScoreBoard

BACKGROUND
Training Test exercise to create a basic WordCupScoreBoard as library /dll

USAGE

Project WorldCupScoreBoard, create a new object ScoreBoardManager. It has the following methods;

- ResponseContainer StartGame(string homeTeam, string awayTeam)
- ResponseContainer UpdateGame (int idGame, int homeTeamScore, int awayTeamScore)
- ResponseContainer FinishGame (int idGame)
- List<Game> SummaryGamesList ()

All methods exccept SummaryGamesList returns a ResponseContainer with basic info to provide with feedback;
- ResponseTypes (enum); Default, Debug, Success, Error. 
- Message


Project WorldCupScoreBoardTests implement Unit Tests to the above project WorldCupScoreBoard. 

