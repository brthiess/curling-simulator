using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CurlingSimulator
{
	public abstract class Tournament
	{
		public List<Team> teams;

		public Tournament()
		{
			teams = new List<Team>();
		}

		public void AddTeam(Team team)
		{
			teams.Add(team);
		}

	

		public abstract TournamentResult Run();

		protected void ResetTeams()
		{
			for (var i = 0; i < teams.Count; i++)
			{
				teams[i] = new Team(teams[i].Name, teams[i].TourRecord, teams[i].TourRanking);
			}
		}
	}
}
