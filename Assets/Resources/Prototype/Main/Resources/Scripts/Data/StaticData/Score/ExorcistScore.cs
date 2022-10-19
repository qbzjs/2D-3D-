using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace KSH_Lib.Data
{
    [Serializable]
    public struct PurifyScore
    {
        public int Total { get { return AttackScore + InterruptScore + CaptureScore + RewardScore; } }
        public int AttackScore;
        public int InterruptScore;
        public int CaptureScore;
        public int RewardScore;
    }

    [Serializable]
    public struct JusticeScore
    {
        public int Total { get { return RitualScore + InjureScore + AccomplishScore + SealExitAltarScore; } }
        public int RitualScore;
        public int InjureScore;
        public int AccomplishScore;
        public int SealExitAltarScore;
    }
    
    [Serializable]
    public struct HolywarScore
    {
        public int Total { get { return FindScore + ChaseScore + HuntScore; } }
        public int FindScore;
        public int ChaseScore;
        public int HuntScore;
    }

    [Serializable]
    public struct JudgeScore
    {
        public int Total
        {
            get
            {
                return ExecuteScore + FirstJudgeScore + BeginPurifyScore + PurifiedEvilScore + LateExecuteScore + LatePurifyScore + TimeOverScore;
            }
        }
        public int ExecuteScore;
        public int FirstJudgeScore;
        public int BeginPurifyScore;
        public int PurifiedEvilScore;
        public int LateExecuteScore;
        public int LatePurifyScore;
        public int TimeOverScore;
    }


	public class ExorcistScore : RoleScore
	{
        /*--- Public Fields ---*/
        public PurifyScore Purify;
        public JusticeScore Justice;
        public HolywarScore Holywar;
        public JudgeScore Judge;


        /*--- Public Methods ---*/
        public override int GetRoleScore()
        {
            return Purify.Total + Justice.Total + Holywar.Total + Judge.Total;
        }
    }
}