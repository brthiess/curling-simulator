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

		public double LsdTotal {get;set;} 

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
				Rating = GetRatingFromRanking(tourRanking.Value, RegressionType.Logarithmic);
			}
			else
			{
				throw new Exception("Record and Ranking are both null in call to Team constructor");
			}
			if (Rating < 0 || Rating > 1)
			{
				throw new Exception("Rating is not within a valid range (0,1).  Rating is: " + Rating);
			}

			this.Name = name;
			this.RoundRobinRecord = new Record();
			this.LsdTotal = 0;
		}

		public static double GetRatingFromRanking(int ranking, RegressionType regressionType)
		{
			double rating = 0;
			if (regressionType == RegressionType.Logarithmic)
			{
				rating = (94.6740233 + -14.68314639 * Math.Log(ranking + Math.E)) / 100;
			}
			else if (regressionType == RegressionType.Exponential)
			{
				rating = 65.54*Math.Pow(Math.E, -0.0093221 * ranking) / 100;
			}
			else if (regressionType == RegressionType.ArcTan) 
			{
				rating = (-25 * Math.Atan(0.1 * ranking - 1.5) + 55) / 100;
			}
			else if (regressionType == RegressionType.Inverse)
			{
				rating = (-1833.15 / (-1.05558 * ranking - 23.703) + 12.144) / 100;
			}
			else if (regressionType == RegressionType.Linear)
			{
				rating = (-0.3134148880054439 * ranking +  64.4390619317387) / 100;
			}
			else if (regressionType == RegressionType.None)
			{
				return 0.5;
			}
			//
			//double rating = (-9450.702849 * Math.Atan(5.4421135 * ranking + 129.2) + 14857) / 100;
			//double rating = (-23 * Math.Atan(0.07 * ranking - 1.5) + 55) / 100;
			//double rating = (-25 * Math.Atan(0.1 * ranking - 1.5) + 55) / 100;
			
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
			double probabilityHomeBeatsAway = GetProbabilityOfHomeBeatingAway(homeTeam, awayTeam, false); 
			var random = new Random();
			DrawToTheButton(homeTeam);
			DrawToTheButton(awayTeam);
			
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

        private static void DrawToTheButton(Team team)
        {
			Random r = new Random();
			double lsdLength = 500/(Math.Sqrt(team.Rating) * r.Next(1,100)) - 5.7;
			if (lsdLength < 0 )
			{
				lsdLength = 0;
			}
			else if (lsdLength > 144)
			{
				lsdLength = 144;
			}
			team.LsdTotal += lsdLength;
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

	public enum RegressionType {
		Logarithmic,
		Exponential,
		ArcTan,
		Inverse,
		Linear,
		None
	}
}
