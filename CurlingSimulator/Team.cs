using System;
using System.Collections.Generic;
using System.Text;

namespace CurlingSimulator
{
	public class Team
	{
		public string Name { get; set; }
		private double Rating { get; set; }

		public Record RoundRobinRecord {get;set;}

		public Record TourRecord { get; private set; }

		public int? TourRanking { get; private set; }

		public int FinalRanking { get; set; }

		public Team(string name, Record tourRecord = null, int? tourRanking = null)
		{
			TourRecord = tourRecord;
			TourRanking = tourRanking;
			if (tourRecord != null)
			{
				Rating = GetRatingFromRecord(tourRecord);
			}
			else if (tourRanking.HasValue)
			{
				Rating = GetRatingFromRanking(tourRanking.Value);
			}
			else
			{
				throw new Exception("Record and Ranking are both null in call to Team constructor");
			}
			if (Rating < 0 || Rating > 1)
			{
				throw new Exception("Rating is not within a valid range (0,1)");
			}

			this.Name = name;
			this.RoundRobinRecord = new Record();
		}

		public static double GetRatingFromRanking(int ranking)
		{
			double rating = (95.6740233 + -14.68314639 * Math.Log(ranking)) / 100;
			if (rating < 0.01)
			{
				rating = 0.01;
			}
			return rating;
		}

		private double GetRatingFromRecord(Record record)
		{
			return (double) record.Wins / (record.Wins + record.Losses);
		}

		public static (Team winner, Team loser) PlayGame(Team homeTeam, Team awayTeam, bool giveHammerAdvantageToTeamWithBetterRecord = false, bool isRoundRobinGame=true)
		{
			double probabilityHomeBeatsAway = GetProbabilityOfHomeBeatingAway(homeTeam, awayTeam, giveHammerAdvantageToTeamWithBetterRecord); 
			var random = new Random();
			if (random.NextDouble() < probabilityHomeBeatsAway)
			{
				if (isRoundRobinGame)
				{
					homeTeam.RoundRobinRecord.AddWin();
					awayTeam.RoundRobinRecord.AddLoss();
				}
				return (homeTeam, awayTeam);
			}
			else
			{
				if (isRoundRobinGame)
				{
					homeTeam.RoundRobinRecord.AddLoss();
					awayTeam.RoundRobinRecord.AddWin();
				}
				return (awayTeam, homeTeam);
			}
			
		}

		public static double GetProbabilityOfHomeBeatingAway(Team homeTeam, Team awayTeam, bool giveHammerAdvantageToTeamWithBetterRecord = false)
		{
			if (homeTeam.Rating == 0)
			{
				if (awayTeam.Rating > 0)
				{
					return 0;
				}
				else
				{
					return 0.5;
				}
			}
			if (homeTeam.Rating == 1)
			{
				if (awayTeam.Rating < 1)
				{
					return 1;
				}
				else
				{
					return 0.5;
				}
			}
			var initialProbability =  ((1 - awayTeam.Rating) * homeTeam.Rating) / ((1 - awayTeam.Rating) * (homeTeam.Rating) + (1 - homeTeam.Rating) * (awayTeam.Rating));
			return initialProbability + (giveHammerAdvantageToTeamWithBetterRecord && homeTeam.RoundRobinRecord.Wins > awayTeam.RoundRobinRecord.Wins ? 0.05 : (giveHammerAdvantageToTeamWithBetterRecord && awayTeam.RoundRobinRecord.Wins > homeTeam.RoundRobinRecord.Wins ? -0.05 : 0)); //TODO make this a little better instead of just adding 0.05
		}
	}
}
