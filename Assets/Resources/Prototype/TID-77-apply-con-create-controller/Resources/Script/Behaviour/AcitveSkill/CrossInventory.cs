using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public struct CrossInventory
	{
		public List<Cross> crosses;
		public CrossInventory(int crossCount)
		{
			crosses = new List<Cross>();

			for (int i = 0; i < crossCount; ++i)
			{
				crosses.Add(new Cross());
			}
		}
		

	}
}