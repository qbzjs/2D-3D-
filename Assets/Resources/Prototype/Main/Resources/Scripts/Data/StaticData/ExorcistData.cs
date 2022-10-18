using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DEM;

namespace KSH_Lib
{
	public abstract class ExorcistData : RoleData
	{
		/*--- Constructor ---*/
		public ExorcistData() { }
		public ExorcistData( float moveSpeed, float interactionSpeed, float projectileSpeed, RoleType type, string roleName, float attackPower, float attackSpeed)
        :
			base(moveSpeed, interactionSpeed, projectileSpeed, RoleType.Exorcist, roleName)
		{
			AttackPower = attackPower;
			AttackSpeed = attackSpeed;
        }

		/*--- Public Fields ---*/
		public float AttackPower = 90;
		public float AttackSpeed = 1.0f;

		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/



		/*--- Public Methods ---*/


		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
	}
}