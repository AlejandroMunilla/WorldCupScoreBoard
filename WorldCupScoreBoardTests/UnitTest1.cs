using SportRadar.WorldCupScoreBoard;
using SportRadar.WorldCupScoreBoard.Enums;
using SportRadar.WorldCupScoreBoard.Models;
using System.Diagnostics;
//Alejandro Munilla. Sept 18, 2023
//This tests could be done using json or xml of games with their country names and scores,
//and make them more comples. This is just something quick I did to show I like testing creating 
//unit tests for my apps. 

namespace SportRadar.WorldCupScoreBoardTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void StartGame()
        {
            //_________________________________________Arrange________________________________//
            var scoreBoardManager = new ScoreBoardManager();

            //_________________________________________Act____________________________________//

            var myResponse = scoreBoardManager.StartGame("Spain", "Malta");
            var myResponse2 = scoreBoardManager.StartGame("Germany", "France");
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
            var scoreBoardManager = new ScoreBoardManager();

            string firstName = "Serbia";
            string secondName = "Uruguay";
            var myResponse3 = scoreBoardManager.StartGame(firstName, secondName);

            var myResponse2 = scoreBoardManager.StartGame("Germany", "France");
            var myResponse = scoreBoardManager.StartGame("Spain", "Malta");
            var listGames = scoreBoardManager.SummaryGamesList();
            Debug.WriteLine("List of Games: " + listGames.Count);
            foreach (var game in listGames)
            {
                Debug.WriteLine("GameId:" + game.IdGame + "/Homename: " + game.HomeTeamName + "/AwayName" + game.AwayTeamName);

            }
            //_________________________________________Act____________________________________//

            scoreBoardManager.FinishGame(1);
            listGames = scoreBoardManager.SummaryGamesList();

            foreach (var game in listGames)
            {
                Debug.WriteLine("GameId:" + game.IdGame + "/Homename: " + game.HomeTeamName + "/AwayName" + game.AwayTeamName);

            }
            //_________________________________________Assert_________________________________//

            Assert.IsTrue(myResponse != null &&
                myResponse.ResponseType == ResponseTypes.Success &&
                listGames.Count == 2 &&
                listGames[1].HomeTeamName == firstName &&
                listGames[1].AwayTeamName == secondName
                );
        }

        [TestMethod]
        public void CheckNullOrEmptyNames ()
        {
            //_________________________________________Arrange________________________________//
            var scoreBoardManager = new ScoreBoardManager();


            List<ResponseContainer>  responseContainers = new List<ResponseContainer>();
            responseContainers.Add( scoreBoardManager.StartGame(string.Empty, string.Empty));
            responseContainers.Add(scoreBoardManager.StartGame("Spain", string.Empty));
            responseContainers.Add(scoreBoardManager.StartGame(string.Empty, "Malta"));

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
    }
}