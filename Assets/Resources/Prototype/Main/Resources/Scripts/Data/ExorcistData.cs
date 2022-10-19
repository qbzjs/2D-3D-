using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DEM;

namespace KSH_Lib.Data
{
	public class ExorcistData : RoleData
	{
		/*--- Constructor ---*/
		public ExorcistData( float moveSpeed, float interactionSpeed, float projectileSpeed, string roleName, float attackPower, float attackSpeed)
        :
			base(moveSpeed, interactionSpeed, projectileSpeed, RoleType.Exorcist, roleName)
		{
			AttackPower = attackPower;
			AttackSpeed = attackSpeed;
        }

		/*--- Public Fields ---*/
		public float AttackPower = 90;
		public float AttackSpeed = 1.0f;

        /*--- Public Methods ---*/
    }

	
}