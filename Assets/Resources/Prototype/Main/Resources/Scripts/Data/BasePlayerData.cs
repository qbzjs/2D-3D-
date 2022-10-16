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

		public abstract class ScoreData
        {
			public ulong[] RoleScores;

			public abstract void AddData(in ScoreData data);
        }
		public class ExorcistScoreData : ScoreData
		{
			ExorcistScoreData()
            {
				RoleScores = new ulong[(int)ExorcistType.Count];
			}

			public ulong ClenseScore;
			public ulong JusticeScore;
			public ulong HolyWarScore;
			public ulong JudgeScore;

            public override void AddData( in ScoreData data )
            {
				if(!(data is ExorcistScoreData))
                {
					return;
                }
				ExorcistScoreData exData = data as ExorcistScoreData;

				ClenseScore += exData.ClenseScore;
				JusticeScore += exData.JusticeScore;
				HolyWarScore += exData.HolyWarScore;
				JudgeScore += exData.JudgeScore;
				
				for( int i = 0; i < (int)ExorcistType.Count; ++i )
                {
					RoleScores[i] += exData.RoleScores[i];
                }
            }
        }
		public class DollScoreData : ScoreData
		{
			DollScoreData()
			{
				RoleScores = new ulong[(int)DollType.Count];
			}

			public ulong CoOpScore;
			public ulong SurviveScore;
			public ulong MissionScore;
			public ulong TauntScore;
			public override void AddData( in ScoreData data )
			{
				if ( !(data is DollScoreData) )
				{
					return;
				}
				DollScoreData dollData = data as DollScoreData;

				CoOpScore += dollData.CoOpScore;
				SurviveScore += dollData.SurviveScore;
				MissionScore += dollData.MissionScore;
				TauntScore += dollData.TauntScore;

				for ( int i = 0; i < (int)DollType.Count; ++i )
				{
					RoleScores[i] += dollData.RoleScores[i];
				}
			}
		}


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