using System;
using System.Collections.Generic;

namespace CurlingSimulator
{
	class Program
	{
		static void Main(string[] args)
		{
			Tournament tournament = new WorldsTournament();
			tournament.AddTeam(new Team("Hasselborg", null, 1));
			tournament.AddTeam(new Team("Tirinzoni", null, 4));
			tournament.AddTeam(new Team("Fujisawa", null, 5));
			tournament.AddTeam(new Team("Muirhead", null, 8));
			tournament.AddTeam(new Team("Kovaleva", null, 13));
			tournament.AddTeam(new Team("Roth\t", null, 14));
			tournament.AddTeam(new Team("Kubeskova", null, 75));
			tournament.AddTeam(new Team("Halse\t", null, 168));
			tournament.AddTeam(new Team("Jentsch\t", null, 25));
			tournament.AddTeam(new Team("Gim\t", null, 15));
			tournament.AddTeam(new Team("Sullanmaa", null, 152));
			tournament.AddTeam(new Team("Han\t", null, 36));
			tournament.AddTeam(new Team("Carey\t", null, 10));

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
