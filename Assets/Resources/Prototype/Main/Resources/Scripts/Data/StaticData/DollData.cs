using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DEM;

namespace KSH_Lib
{
	public abstract class DollData : RoleData
	{
		/*--- Constructor ---*/
		public DollData() { }
		public DollData( float moveSpeed, float interactionSpeed, float projectileSpeed, RoleType type, string roleName, int dollHP, int devilHP )
		:
			base(moveSpeed, interactionSpeed, projectileSpeed, RoleType.Doll, roleName)
		{
			DollHP = dollHP;
			DevilHP = devilHP;
		}



		/*--- Public Fields ---*/
		public int DollHP = 200;
		public int DevilHP = 200;



		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/


		/*--- Public Methods ---*/


		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
	}
}