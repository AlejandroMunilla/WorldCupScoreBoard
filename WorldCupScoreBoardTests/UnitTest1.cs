using SportRadar.WorldCupScoreBoard;
using SportRadar.WorldCupScoreBoard.Enums;
using SportRadar.WorldCupScoreBoard.Models;
using System.Diagnostics;
//Alejandro Munilla. Sept 18, 2023
//This tests could be done automatically using json or xml of games with their country names and scores,
//and make them more automatic. This is just something quick I did to show I like testing creating 
//unit tests for my apps. 
//I could also do more extreme situations; E.g. non allowed characters.  

namespace SportRadar.WorldCupScoreBoardTests
{
    [TestClass]
    public class UnitTest1
    {
        private const int _maxAllowedScore = 30;
        private const int _maxCharsNameAllowed = 10;

        [TestMethod]
        public void StartGame()
        {
            //_________________________________________Arrange________________________________//
            var scoreBoardManager = new ScoreBoardManager();

            //_________________________________________Act____________________________________//

            var myResponse = scoreBoardManager.StartGame(GenerateRandomString(_maxCharsNameAllowed), GenerateRandomString(_maxCharsNameAllowed));
            var myResponse2 = scoreBoardManager.StartGame(GenerateRandomString(_maxCharsNameAllowed), GenerateRandomString(_maxCharsNameAllowed));
            var listGames = scoreBoardManager.SummaryGamesList();
            Debug.WriteLine("Myresponse: " + myResponse.Message + "/" + myResponse.ResponseType);
            //_________________________________________Assert_________________________________//

            Assert.IsTrue(myResponse != null && 
                myResponse.ResponseType == ResponseTypes.Success &&
                myResponse2.ResponseType == ResponseTypes.Success &&
                listGames.Count == 2 &&
                listGames[0].HomeScore == 0 &&
                listGames[0].AwayScore == 0
                );
        }

        [TestMethod]
        public void FinishGame()
        {
            //_________________________________________Arrange________________________________//
            var scoreBoardManager = CreateTestScoreBoard(5);

            var initialListCount = scoreBoardManager.SummaryGamesList().Count;

            //Finish a random Game -not truly random I know

            Random rnd = new Random();
            var gameId = rnd.Next(0, initialListCount-1);
            var myResponse = scoreBoardManager.FinishGame(gameId);

            var deletedGame = scoreBoardManager.SummaryGamesList().FirstOrDefault( x=> x.IdGame == gameId);
            //_________________________________________Assert_________________________________//

            Assert.IsTrue(myResponse != null &&
                myResponse.ResponseType == ResponseTypes.Success &&
                scoreBoardManager.SummaryGamesList().Count == initialListCount - 1 &&
                deletedGame == null ) ;
        }

        [TestMethod]
        public void FinishNonExistingGame ()
        {
            var scoreBoardManager = CreateTestScoreBoard(5);
            var myResponse = scoreBoardManager.FinishGame(9999);

            Debug.WriteLine("Unit Test FinishNonExistingGame. Response: " + myResponse.ResponseType + "/" + myResponse.Message);
        
            Assert.IsTrue (myResponse.ResponseType != ResponseTypes.Success);

        }

        [TestMethod]
        public void CheckNullOrEmptyNames ()
        {
            //_________________________________________Arrange________________________________//
            var scoreBoardManager = new ScoreBoardManager();
            List<ResponseContainer>  responseContainers = new List<ResponseContainer>();
            responseContainers.Add( scoreBoardManager.StartGame(string.Empty, string.Empty));
            responseContainers.Add(scoreBoardManager.StartGame(GenerateRandomString(_maxCharsNameAllowed), string.Empty));
            responseContainers.Add(scoreBoardManager.StartGame(string.Empty, GenerateRandomString(_maxCharsNameAllowed)));

            var listGames = scoreBoardManager.SummaryGamesList();
            Debug.WriteLine("List of Games: " + listGames.Count);
            foreach (var game in listGames)
            {
                Debug.WriteLine("GameId:" + game.IdGame + "/Homename: " + game.HomeTeamName + "/AwayName" + game.AwayTeamName);

            }
            //_________________________________________Act____________________________________//
            bool allAreErrors = true;
            foreach (var response in responseContainers)
            {
                if (response.ResponseType != ResponseTypes.Error || listGames.Count > 0) allAreErrors = false;
            }
            Debug.WriteLine("Listgame count: " + listGames.Count +"/AllAreErrors" + allAreErrors);
            //_________________________________________Assert_________________________________//
            Assert.IsTrue( allAreErrors);
        }

        [TestMethod]
        public void UpdateGame()
        {
            //_________________________________________Arrange________________________________//
            var scoreBoardManager = CreateTestScoreBoard(5);
            var listGames = scoreBoardManager.SummaryGamesList();
            Debug.WriteLine ("Update Game. ListGames Count: " + listGames.Count);
            //_________________________________________Act____________________________________//
            bool noErrors = true;
            for (int i = 0; i < listGames.Count; i++)
            {
                int homeScore = GenerateRandomScore();
                int awayScore = GenerateRandomScore();
                var response = scoreBoardManager.UpdateGame(listGames[i].IdGame, homeScore, awayScore);

                if (response.ResponseType != ResponseTypes.Success ||
                    listGames[i].HomeScore != homeScore ||
                    listGames[i].AwayScore != awayScore)
                {
                    noErrors = false;
                }
            }

            var list = scoreBoardManager.SummaryGamesList();

            foreach (var game in list)
            {
                Debug.WriteLine("Game: " + game.TotalScore + "/" + game.LastUpdate + "/" + game.IdGame + "/" + game.HomeTeamName + ": " + game.HomeScore + "/" + game.AwayTeamName + ": " + game.AwayScore);
            }

            //_________________________________________Assert_________________________________//
            Assert.IsTrue (noErrors);

        }


        #region Helpers
        private ScoreBoardManager CreateTestScoreBoard(int numberOfGames)
        {
            ScoreBoardManager scoreBoardManager = new ScoreBoardManager();

            for (int i=0; i < numberOfGames; i++)
            {
                scoreBoardManager.StartGame(GenerateRandomString(_maxCharsNameAllowed), GenerateRandomString(_maxCharsNameAllowed));
            }
            //For debugging purposes
            //scoreBoardManager.StartGame("Germany", "France");
            //scoreBoardManager.StartGame("Spain", "Malta");
            //scoreBoardManager.StartGame("Argentina", "Brazil");
            return scoreBoardManager;
        }

        private string GenerateRandomString (int numberOfCharacters)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var stringChars = new char[8];

            var random = new Random();

            for (int j = 0; j < stringChars.Length; j++)
            {
                stringChars[j] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }

        //I know it is not truly random, just for the purpuse of testing and use it to feed Unit tests
        private int GenerateRandomScore ()
        {

            Random rnd = new Random();
            return rnd.Next(0, _maxAllowedScore);            
        }
        #endregion
    }
}