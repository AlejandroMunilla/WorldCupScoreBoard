using SportRadar.WorldCupScoreBoard.Enums;
using SportRadar.WorldCupScoreBoard.Interface;
using SportRadar.WorldCupScoreBoard.Models;
using System.Diagnostics;

namespace SportRadar.WorldCupScoreBoard
{
    public class ScoreBoardManager : IScoreBoard
    {
        private int _internalGameId = 1;            //Similar to Sql db unique identifiers, to ensure IdGame of each game is unique. 
        private List<Game> _gamesList = new List<Game>();

        //Start a Game and add it to the _gameList;
        //TODO: Should we do some validation on strings? E.g. Min characters, allowed characters, etc. 
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
                _gamesList.Add(game);
                response.ResponseType = ResponseTypes.Success;
                response.Message = "ScoreBoardManager.StartGame StartGame: " + homeTeam+"/" + awayTeam;
            }
            catch (Exception ex)
            {
                response.ResponseType = ResponseTypes.Error;
                response.Message = "ScoreBoardManager.StartGame Error: " + ex.Message;
            }

            return response;
        }


        //Update scores of a game in the GameList through its internal IdGame 
        //TODO: Should we do some validation? E.g. max score 1000? score not lower than before?  
        public ResponseContainer UpdateGame (int idGame, int homeTeamScore, int awayTeamScore)
        {
            ResponseContainer response = new ResponseContainer();

            
            try
            {
                var game = _gamesList.FirstOrDefault(x => x.IdGame == idGame);

                if (game != null)
                {
                    game.HomeScore = homeTeamScore;
                    game.AwayScore = awayTeamScore;
                    game.LastUpdate = DateTime.Now;
                    response.ResponseType = ResponseTypes.Success;
                    response.Message = "ScoreBoardManager.Update Game: " + game.HomeTeamName + "/" + game.AwayTeamName;
                }
                else
                {
                    response.ResponseType = ResponseTypes.Error;
                    response.Message = "ScoreBoardManager.Update Update Game: I could not find a game with that internal Id";
                }
            }
            catch (Exception ex)
            {
                response.ResponseType = ResponseTypes.Error;
                response.Message = "ScoreBoardManager.UPdateGame Error: " + ex.Message;
            }
            return response;
        }

        //Remove a game from the _gamesList (if it exist)
        public ResponseContainer FinishGame (int idGame)
        {
            ResponseContainer response = new ResponseContainer();
            try
            {
                var game = _gamesList.SingleOrDefault(x => x.IdGame == idGame);
                if (game != null)
                {
                    _gamesList.Remove(game);
                    response.ResponseType = ResponseTypes.Success;
                    response.Message = "ScoreBoardManager.FinishGame: " + game.HomeTeamName + "/" + game.AwayTeamName;
                }
                else
                {
                    response.ResponseType = ResponseTypes.Error;
                    response.Message = "ScoreBoardManager.FinishGame: A game with internal Id: " + idGame + " could not be found";
                }
            }
            catch (Exception ex)
            {
                response.ResponseType = ResponseTypes.Error;
                response.Message = "ScoreBoardManager.Error: " + ex.Message;
            }
            return response;
        }           

        public List<Game> SummaryGamesList ()
        {
            var list = _gamesList.OrderByDescending(x => x.TotalScore).ThenByDescending(x =>x.LastUpdate). ToList();

            return list;
        }

        private int GetNewInternalId()
        {
            return _internalGameId++;
        }

    }
}
