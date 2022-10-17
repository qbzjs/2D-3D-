using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSH_Lib
{
	public class BasePlayerData
	{
		/*--- Inner Class ---*/
		


		/*--- Public Fields ---*/
		public RoleType TypeRole;
		public ExorcistType TypeExorcist;
		public DollType TypeDoll;

		public ExorcistScoreData ExorcistScore;
		public DollScoreData DollScore;

		public float MoveSpeed;
		public float InteractionSpeed;
		public float ProjectileSpeed;

		public float DollHP;
		public float DevilHP;

		public float attackPower;
	}
}