using System;
using System.Collections.Generic;
using System.Text;

namespace CurlingSimulator
{
	public class RankingsStat
	{
		private Dictionary<int, int> placingFrequencies;
		
		private int totalNumberOfPlacings;

		public RankingsStat()
		{
			placingFrequencies = new Dictionary<int, int>();
			totalNumberOfPlacings = 0;
		}

		public void AddPlacing(int placingNumber)
		{
			if (!placingFrequencies.ContainsKey(placingNumber))
			{
				placingFrequencies.Add(placingNumber, 0);
			}
			placingFrequencies[placingNumber] += 1;
			totalNumberOfPlacings++;
		}

		public double GetPercentageForPlacing(int placingNumber)
		{
			if (!placingFrequencies.ContainsKey(placingNumber))
			{
				return 0;
			}
			return (double) 100 * placingFrequencies[placingNumber] / totalNumberOfPlacings;
		}
	}
}
