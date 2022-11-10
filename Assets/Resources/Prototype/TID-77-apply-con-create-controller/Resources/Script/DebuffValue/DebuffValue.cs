using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
    public class DebuffValue : MonoBehaviour
    {
        public struct BishopValue
        {
            public int Stack;
            public BishopValue(int stack =0)
            {
                Stack = stack;
            }
        }

        public struct HunterValue
        {
            public bool IsCrowDebuff;
            public float CrowGauge;
            public HunterValue(bool isCrowDebuff=false, float crowGauge = 0.0f)
            {
                IsCrowDebuff = isCrowDebuff;
                CrowGauge = crowGauge;
            }
        }

    }

}

