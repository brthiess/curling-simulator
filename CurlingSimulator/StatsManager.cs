using System;
using System.Collections.Generic;
using System.Text;

namespace CurlingSimulator
{
	public class StatsManager
	{
		Dictionary<string, RankingsStat> rankingStats;


		public StatsManager()
		{
			rankingStats = new Dictionary<string, RankingsStat>();
		}

		public void AddPlacingForTeam(string teamName, int placing)
		{
			if (!rankingStats.ContainsKey(teamName))
			{
				rankingStats[teamName] = new RankingsStat();
			}
			rankingStats[teamName].AddPlacing(placing);
		}

		public void PrintPercentages()
		{
			Console.Write("\nTeam Name");
			for (var i = 1; i <= rankingStats.Count; i++)
			{
				Console.Write("\t" + i);
			}
			
			foreach(KeyValuePair<string, RankingsStat> teamRanking in rankingStats)
			{
				Console.Write("\n" + teamRanking.Key);
				for(var i = 1; i <= rankingStats.Count; i++)
				{
					Console.Write("\t" + string.Format("{0:0.0}", teamRanking.Value.GetPercentageForPlacing(i)) +"%");
				}
			}
		}
	}
}
