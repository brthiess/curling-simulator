using System;
using System.Collections.Generic;

namespace CurlingSimulator
{
	class Program
	{
		static void Main(string[] args)
		{
			Tournament tournament = new WorldsTournament();
			tournament.AddTeam(new Team("Team  #2", null, 2));
			tournament.AddTeam(new Team("Team  #4", null, 4));
			tournament.AddTeam(new Team("Team  #7", null, 6));
			tournament.AddTeam(new Team("Team #11", null, 11));
			tournament.AddTeam(new Team("Team #14", null, 14));
			tournament.AddTeam(new Team("Test #26", null, 26));
			tournament.AddTeam(new Team("Test #28", null, 28));
			tournament.AddTeam(new Team("Test #34", null, 34));
			tournament.AddTeam(new Team("Test #36", null, 36));
			tournament.AddTeam(new Team("Test #65", null, 65));
			tournament.AddTeam(new Team("Test #84", null, 84));
			tournament.AddTeam(new Team("Test #134", null, 134));
			tournament.AddTeam(new Team("Test #152", null, 152));

			List<TournamentResult> results = new List<TournamentResult>();
			for (var i =0; i < 100000; i++)
			{
				var tournamentResult = tournament.Run();
				results.Add(tournamentResult);
			}

			TabulateResults(results);
		}

		private static void TabulateResults(List<TournamentResult> results)
		{
			StatsManager statsManager = SetUpTeamResults(results);
			statsManager.PrintPercentages();
		}

		private static StatsManager SetUpTeamResults(List<TournamentResult> results)
		{
			var statsManager = new StatsManager();
			foreach (var result in results)
			{
				foreach (var team in result.Teams)
				{
					statsManager.AddPlacingForTeam(team.Name, team.FinalRanking);
				}
			}
			return statsManager;
		}
	}
}
