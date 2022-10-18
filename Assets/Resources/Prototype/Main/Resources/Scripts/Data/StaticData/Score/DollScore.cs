using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace KSH_Lib
{
	[Serializable]
	public struct CoopScore
	{
		public int Total
		{
			get
			{
				return	CureScore		+	SucessPatternScore	+	RegroupScore	+
						HelpScore		+	InterruptSocre		+	ProtectScore	+
						RescueBoxScore	+	 RescueTrapScore	+	RescueSafelyScore;
			}
		}
		public int CureScore;
		public int SucessPatternScore;
		public int RegroupScore;
		public int HelpScore;
		public int InterruptSocre;
		public int ProtectScore;
		public int RescueBoxScore;
		public int RescueTrapScore;
		public int RescueSafelyScore;
    }

	[Serializable]
	public struct SurviveScore
	{
		public int Total { get { return EscapeScore + CureSelfScore; } }
		public int EscapeScore;
		public int CureSelfScore;
    }

	[Serializable]
	public struct MissionScore
    {
		public int Total
		{
			get
			{
				return RitualAltarScore + SuccessAltarPatternScore + GroupRitualScore +
						BeadScore + EvidenceScore + OpenExitAltarScore +
						ExitByExitAltarScore;
			}
		}
		public int RitualAltarScore;
		public int SuccessAltarPatternScore;
		public int GroupRitualScore;
		public int BeadScore;
		public int EvidenceScore;
		public int OpenExitAltarScore;
		public int ExitByExitAltarScore;
    }

	[Serializable]
	public struct TauntScore
	{
		public int Total { get { return StrongHeartScore + RunScore + EscapeFromChasesScore + CamouflageScore; } }

		public int StrongHeartScore;
		public int RunScore;
		public int EscapeFromChasesScore;
		public int CamouflageScore;
		public int SucessCamouflageScore;
	}
		

	public class DollScore : RoleScore
	{

		/*--- Public Fields ---*/
		public CoopScore CoOp;
		public SurviveScore Survive;
		public MissionScore Mission;
		public TauntScore Taunt;


		/*--- Public Methods ---*/
		public override int GetRoleScore()
		{
			return CoOp.Total + Survive.Total + Mission.Total + Taunt.Total;
		}
	}
}