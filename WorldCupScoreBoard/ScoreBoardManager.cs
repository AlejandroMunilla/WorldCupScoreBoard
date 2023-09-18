using SportRadar.WorldCupScoreBoard.Enums;
using SportRadar.WorldCupScoreBoard.Interface;
using SportRadar.WorldCupScoreBoard.Models;

namespace SportRadar.WorldCupScoreBoard
{
    public class ScoreBoardManager : IScoreBoard
    {
        private int internalGameId = 0;
        private List<Game> gamesList = new List<Game>();

        public ResponseContainer StartGame(string homeTeam, string awayTeam)
        {
            var response = new ResponseContainer();

            if (String.IsNullOrEmpty(homeTeam) || String.IsNullOrEmpty(awayTeam))
            {
                response.ResponseType = ResponseTypes.Error;
                response.Message = "ScoreBoardManager.StartGame Error: HomeTeam or AwayTeam strings are either null or empty. Game not started";
                return response;
            }

            try
            {
                var game = new Game();
                game.IdGame = GetNewInternalId();
                game.HomeTeamName = homeTeam;
                game.AwayTeamName = awayTeam;
                game.LastUpdate = DateTime.Now;
                gamesList.Add(game);
                response.ResponseType = ResponseTypes.Success;
                response.Message = "ScoreBoardManager.StartGameWithResponse StartGame: " + homeTeam+"/" + awayTeam;
            }
            catch (Exception ex)
            {
                response.ResponseType = ResponseTypes.Error;
                response.Message = "ScoreBoardManager.StartGameWithResponse Error: " + ex.Message;
            }

            return response;
        }

        private int GetNewInternalId ()
        {
            return internalGameId++;
        }

        public ResponseContainer UpdateGame (int idGame, int homeTeamScore, int awayTeamScore)
        {
            ResponseContainer response = new ResponseContainer();

            
            try
            {
                var game = gamesList.SingleOrDefault(x => x.IdGame == idGame);

                if (game != null)
                {
                    game.HomeScore = homeTeamScore;
                    game.AwayScore = awayTeamScore;
                    response.ResponseType = ResponseTypes.Success;
                    response.Message = "ScoreBoardManager.StartGameWithResponse FinishGame: " + game.HomeTeamName + "/" + game.AwayTeamName;
                }
                else
                {
                    response.ResponseType = ResponseTypes.Error;
                    response.Message = "ScoreBoardManager.StartGameWithResponse FinishGame: I could not find a game with that internal Id";
                }
            }
            catch (Exception ex)
            {
                response.ResponseType = ResponseTypes.Error;
                response.Message = "ScoreBoardManager.StartGameWithResponse Error: " + ex.Message;
            }
            return response;
        }


        public ResponseContainer FinishGame (int idGame)
        {
            ResponseContainer response = new ResponseContainer();
            try
            {
                var game = gamesList.SingleOrDefault(x => x.IdGame == idGame);
                if (game != null)
                {
                    gamesList.Remove(game);
                    response.ResponseType = ResponseTypes.Success;
                    response.Message = "ScoreBoardManager.StartGameWithResponse FinishGame: " + game.HomeTeamName + "/" + game.AwayTeamName;
                }
                else
                {
                    response.ResponseType = ResponseTypes.Error;
                    response.Message = "ScoreBoardManager.StartGameWithResponse FinishGame: I could not find a game with that internal Id";
                }
            }
            catch (Exception ex)
            {
                response.ResponseType = ResponseTypes.Error;
                response.Message = "ScoreBoardManager.StartGameWithResponse Error: " + ex.Message;
            }
            return response;
        }   


        

        public List<Game> SummaryGamesList ()
        {
            return gamesList.OrderByDescending(x => x.TotalScore).OrderByDescending(x => x.LastUpdate).ToList();
        }
    }
}
