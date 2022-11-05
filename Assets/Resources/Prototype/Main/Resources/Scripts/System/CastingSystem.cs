using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace KSH_Lib
{
    public class CastingSystem : MonoBehaviour
    {
        public struct Cast
        {
            Cast( float deltaRatio, float destRatio, float? coolTime )
            {
                this.deltaRatio = deltaRatio;
                this.destRatio = destRatio;
                this.coolTime = coolTime;
            }
            public float deltaRatio { get; private set; }
            public float destRatio { get; private set; }
            public float? coolTime { get; private set; }
            public static Cast CreateByRatio( float deltaRatio, float destRatio = 1.0f, float? coolTime = null )
            {
                return new Cast( deltaRatio, destRatio, coolTime );
            }
            public static Cast CreateByTime( float castTime, float destRatio = 1.0f, float? coolTime = null )
            {
                return new Cast( 1 / castTime, destRatio, coolTime );
            }
        }
        public struct CastFuncSet
        {
            public CastFuncSet( Action<float> SyncDataWith = null, Func<bool> RunningCondition = null, Action PauseAction = null, Action FinishAction = null )
            {
                this.SyncDataWith = SyncDataWith;
                this.RunningCondition = RunningCondition;
                this.PauseAction = PauseAction;
                this.FinishAction = FinishAction;
            }
            public Action<float> SyncDataWith { get; private set; }
            public Func<bool> RunningCondition { get; private set; }
            public Action PauseAction { get; private set; }
            public Action FinishAction { get; private set; }
        }

        /*--- Public Fields ---*/
        [field:SerializeField]public bool IsFinshCasting { get; private set; }
        //public bool CanCasting { get; private set; }
        [field: SerializeField] public bool IsCoroutineRunning { get; private set; }
        public bool WasReset { get { return !IsFinshCasting && !IsCoroutineRunning; } }
        [field: SerializeField] public float CurCoolTime { get; private set; }
        public float SliderVal { get { return slider.value; } }
    

        /*--- Private Fields ---*/
        [SerializeField]
        GameObject castingSliderObj;
        Slider slider;
        Coroutine curCoroutine;


        /*--- MonoBehaviour Callbacks ---*/
        private void Start()
        {
            slider = castingSliderObj.GetComponent<Slider>();
            if(slider == null)
            {
                Debug.LogError( "CastingSystem.Start: No Slider Found" );
            }
            castingSliderObj.SetActive( false );

        }

        private void OnGUI()
        {
            GUI.Box( new Rect( 300, 0, 150, 30 ), $"WasReset={WasReset}" );
        }


        /*--- Public Methods ---*/
        public void StartCasting( Cast cast, CastFuncSet funcSet )
        {
            if ( IsCoroutineRunning )
            {
                Debug.LogWarning( "CastingSystem.StartCasting: castingSlider is already activated!" );
                return;
            }

            if(funcSet.RunningCondition != null)
            {
                if(!funcSet.RunningCondition())
                {
                    Debug.Log( "CastingSystem.StartCasting: runnig condition is false" );
                    return;
                }
            }

            curCoroutine = StartCoroutine( Casting( cast, funcSet ) );
        }

        public void ForceSetRatioTo(float ratio)
        {
            slider.value = ratio;
        }
        public void ForceStopCasting()
        {
            StopCoroutine( curCoroutine );
            ResetCasting();
        }
        public void ResetCasting()
        {
            slider.value = 0.0f;
            CurCoolTime = 0.0f;
            IsCoroutineRunning = false;
            IsFinshCasting = false;
        }
        void FinishCasting( Cast cast, CastFuncSet castFunc, bool isFinish )
        {
            castingSliderObj.SetActive( false );
            IsFinshCasting = isFinish;

            if ( isFinish )
            {
                if ( castFunc.FinishAction != null )
                {
                    castFunc.FinishAction();
                }
                slider.value = 0.0f;
            }
            else
            {
                if ( castFunc.PauseAction != null )
                {
                    castFunc.PauseAction();
                }
            }

            CurCoolTime = 0.0f;
            if ( cast.coolTime != null )
            {
                StartCoroutine( CoolDown( cast.coolTime ) );
            }
            else
            {
                IsCoroutineRunning = false;
            }
        }


        /*--- Private Methods ---*/
        IEnumerator Casting( Cast cast, CastFuncSet castFunc )
        {
            castingSliderObj.SetActive( true );
            IsCoroutineRunning = true;

            while ( true )
            {
                if( slider.value >= cast.destRatio )
                {
                    FinishCasting( cast, castFunc, true );
                    break;
                }

                // Change Value and Sync
                slider.value += cast.deltaRatio * Time.deltaTime;
                if ( castFunc.SyncDataWith != null )
                {
                    castFunc.SyncDataWith( slider.value );
                }

                // Check Runnig Condition
                if ( castFunc.RunningCondition != null )
                {
                    if(!castFunc.RunningCondition())
                    {
                        FinishCasting( cast, castFunc, false );
                        break;
                    }
                }
                yield return null;
            }
            yield return null;
        }

        IEnumerator CoolDown(float? coolTime)
        {
            while ( CurCoolTime < coolTime )
            {
                CurCoolTime += Time.deltaTime;
                yield return null;
            }
            ResetCasting();
            yield return null;
        }
    }
}