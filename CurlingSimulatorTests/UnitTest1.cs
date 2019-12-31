using Microsoft.VisualStudio.TestTools.UnitTesting;
using CurlingSimulator;
using System;



namespace CurlingSimulatorTests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		[DataRow(9,1,6,4, .86)]
		[DataRow(9,1,0,4, 1)]
		[DataRow(9,0,6,4, 1)]
		[DataRow(9,0,0,4, 1)]
		[DataRow(0,9,1,4, 0)]
		[DataRow(0,9,0,4, 0.5)]
		[DataRow(9,0,5,0, 0.5)]
		[DataRow(9,1,1,9, 0.99)]
		public void GetProbabilityOfHomeBeatingAway(int homeWins, int homeLosses, int awayWins, int awayLosses, double probability)
		{
			Team t1 = new Team("Test 1", new Record(homeWins, homeLosses));
			Team t2 = new Team("Test 2", new Record(awayWins, awayLosses));
			Assert.AreEqual(probability, Math.Round(Team.GetProbabilityOfHomeBeatingAway(t1, t2), 2));
		}

		[TestMethod]
		public void WorldsTournamentRoundRobinTest()
		{
			Tournament tournament = new WorldsTournament();
			
			tournament.AddTeam(new Team("Team 91", new Record(9,1)));
			tournament.AddTeam(new Team("Team 92", new Record(9,2)));
			tournament.AddTeam(new Team("Team 33", new Record(3,3)));
			tournament.AddTeam(new Team("Team 88", new Record(8,8)));
			tournament.AddTeam(new Team("Team 82", new Record(8,2)));
			tournament.AddTeam(new Team("Team 36", new Record(3,6)));
			tournament.AddTeam(new Team("Test 19", new Record(1, 9)));
			var tournamentResult = tournament.Run();
			Assert.AreEqual(true, !string.IsNullOrEmpty(tournamentResult.GetFinalRoundRobinPlacings()));
		}

		[TestMethod]
		public void LargeNumberOfGamesTest()
		{
			Team t1 = new Team("Test 1", new Record(4, 6));
			Team t2 = new Team("Test 2", new Record(6, 4));
			Team.GetProbabilityOfHomeBeatingAway(t1, t2);
			t1.RoundRobinRecord.AddWin();
			Assert.AreEqual(.31, Math.Round(Team.GetProbabilityOfHomeBeatingAway(t1, t2), 2));
			Assert.AreEqual(.36, Math.Round(Team.GetProbabilityOfHomeBeatingAway(t1, t2, true), 2));
			t2.RoundRobinRecord.AddWin();
			Assert.AreEqual(.31, Math.Round(Team.GetProbabilityOfHomeBeatingAway(t1, t2, true), 2));
			t2.RoundRobinRecord.AddWin();
			Assert.AreEqual(.26, Math.Round(Team.GetProbabilityOfHomeBeatingAway(t1, t2, true), 2));
		}

		[TestMethod]
		public void GetRatingFromRankingTest()
		{
			var ranking1 = Team.GetRatingFromRanking(1);
			var ranking2 = Team.GetRatingFromRanking(2);
			var ranking50 = Team.GetRatingFromRanking(50);
			var ranking200 = Team.GetRatingFromRanking(200);
			var ranking1000 = Team.GetRatingFromRanking(1000);

			Assert.IsTrue(ranking1 < 1 && ranking1 > 0);
			Assert.IsTrue(ranking2 < 1 && ranking2 > 0);
			Assert.IsTrue(ranking50 < 1 && ranking50 > 0);
			Assert.IsTrue(ranking200 < 1 && ranking200 > 0);
			Assert.IsTrue(ranking1000 < 1 && ranking1000 > 0);

			Assert.IsTrue(ranking1 > ranking2);
			Assert.IsTrue(ranking2 > ranking50);
			Assert.IsTrue(ranking50 > ranking200);
			Assert.IsTrue(ranking200 > ranking1000);			
		}
	}
}
