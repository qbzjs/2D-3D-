using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSH_Lib
{
    public class TypingPattern : MonoBehaviour
    {
        CastingSystem typingCasting;

        [SerializeField] KeyCode[] keyCodes;
        GameObject typingUIObj;
        enum PatternType { Z,X,C,V}

        PatternType[] patterns;

        [field: SerializeField] public float RateOfGauge {get; private set;}

        GameObject buttonUI_Z;
        GameObject buttonUI_X;
        GameObject buttonUI_C;
        GameObject buttonUI_V;

        Coroutine coroutine;

        void StartPattern(float coolTime, float limitTime)
        {
            

            typingCasting.ForceSetRatioTo( 1.0f );
            //typingCasting.StartCasting(CastingSystem.Cast.CreateByTime(-limitTime, 0.0f, 0.1f),
            //    CastingSystem.CastFuncSet(SyncData,))
        }

        void SyncData(float value)
        {
            RateOfGauge = value;
        }

        void ForceStop()
        {
            
        }


        void CreatePattern(int count)
        {
            patterns = new PatternType[count];
            for(int i = 0; i < count; ++i )
            {
                patterns[i] = GetRandomType();
            }
        }
        PatternType GetRandomType()
        {
            return (PatternType)Random.Range( 0, (int)PatternType.V );
        }

    }
}
