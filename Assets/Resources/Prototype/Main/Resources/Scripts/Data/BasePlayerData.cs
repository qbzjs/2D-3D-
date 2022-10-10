using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSH_Lib
{
	public class BasePlayerData
	{
		/*--- Inner Class ---*/
		public enum RoleType
		{
			Null,
			Doll,
			Exorcist
		}

		public enum ExorcistType
        {
			Photographer,
			Bishop,
			Dokkaebi,
			Priest,
			Hunter,
			Count
        }
		public enum DollType
        {
			Monkey,
			Penguin,
			Rabbit,
			Wolf,
			Tortoise,
			Count
		}

		public class ExorcistScoreData
		{
			public ulong ClenseScore;
			public ulong JusticeScore;
			public ulong HolyWarScore;
			public ulong JudgeScore;
			public ulong[] ExorcistScores = new ulong[(int)ExorcistType.Count];
        }
		public class DollScoreData
		{
			public ulong CoOpScore;
			public ulong SurviveScore;
			public ulong MissionScore;
			public ulong TauntScore;
			public ulong[] DollScores = new ulong[(int)DollType.Count];
		}

		/*--- Public Fields ---*/

		public RoleType TypeRole;
		public ExorcistType TypeExorcist;
		public DollType TypeDoll;

		public ulong Score;
		public ExorcistScoreData ExorcistScore;
		public DollScoreData DollScore;

		public float MoveSpeed;
		public float InteractionSpeed;
		public float ProjectileSpeed;

		public float DollHP;
		public float DevilHP;

		public float attackPower;

		

		/*--- Score ---*/



		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/


		/*--- Public Methods ---*/


		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
	}
}