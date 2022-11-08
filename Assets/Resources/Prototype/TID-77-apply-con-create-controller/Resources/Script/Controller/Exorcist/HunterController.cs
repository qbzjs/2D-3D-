using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
	public class HunterController: ExorcistController
	{
		public GameObject TrapPrefab;
		
		public int TrapCount { get; protected set; }
		
	}
}