using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CurlingSimulator
{
	public class TournamentResult
	{

		public List<Team> Teams;

		public TournamentResult(Tournament tournament)
		{
			Teams = new List<Team>();
			SetTeams(tournament.teams);
		}

		private void SetTeams(List<Team> teams)
		{
			foreach(var team in teams)
			{
				var newTeam = new Team(team.Name, team.TourRecord, team.TourRanking);
				newTeam.RoundRobinRecord = team.RoundRobinRecord;
				newTeam.FinalRanking = team.FinalRanking;
				this.Teams.Add(newTeam);
			}
		}

		public string GetFinalRoundRobinPlacings()
		{
			string standingsString = "Name\t\tW\tL";
			var teamsSorted = Teams.OrderByDescending(o => o.RoundRobinRecord.Wins).ToList();
			foreach (var team in teamsSorted)
			{
				standingsString += "\n" + team.Name + "\t" + team.RoundRobinRecord.Wins + "\t" + team.RoundRobinRecord.Losses;
			}
			return standingsString;
		}

		public string GetPlayoffResults()
		{
			string playoffResults = "\nName\t\tPlacing";
			var teamsSorted = Teams.OrderBy(o => o.FinalRanking).ToList();
			foreach(var team in teamsSorted)
			{
				playoffResults += "\n" + team.Name + "\t" + team.FinalRanking;
			}
			return playoffResults;
		}
	}
}
