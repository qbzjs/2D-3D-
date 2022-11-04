using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace KSH_Lib
{
    public class CastingSystem : MonoBehaviour
    {
        public class Cast
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
        public bool IsFinshCasting { get; private set; }
        //public bool CanCasting { get; private set; }
        //public bool IsReset { get { return !IsFinshCasting && !CanCasting; } }
        public bool IsCoroutineRunning { get; private set; }
        public float CurCoolTime { get; private set; }
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


        /*--- Public Methods ---*/
        //public void StartAutoCasting( Cast cast, CastFuncSet funcSet)
        //{
        //    if( !CanCasting )
        //    {
        //        Debug.LogWarning( "CastingSystem.StartAutoCasting: castingSlider is already activated!" );
        //        return;
        //    }
        //    curCoroutine = StartCoroutine( AutoCasting( cast, funcSet ) );
        //}
        //public void StartManualCasting( Cast cast, Func<bool> IsInputNow, Action<float> SyncDataWith = null, Action PauseAction = null, Action FinishAction = null )
        //{
        //    if ( !CanCasting || !IsInputNow() )
        //    {
        //        return;
        //    }
        //    else if ( !IsCoroutineRunning )
        //    {
        //        Debug.Log( $"Start Manual Casting Now  (delta = {cast.deltaRatio}" );
        //        curCoroutine = StartCoroutine( ManualCasting( cast, IsInputNow, SyncDataWith, PauseAction, FinishAction ) );
        //    }
        //}

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
        }
        public void ResetCasting()
        {
            //if ( !IsFinshCasting )
            //{
            //    Debug.LogWarning( "CastingSystem.ResetCasting: Called Reset when not finishing Casting" );
            //    return;
            //}

            //IsFinshCasting = false;
            //IsCoroutineRunning = false;
            //slider.value = 0.0f;
            //IsCoolDown = false;
            //CurCoolTime = 0.0f;

            slider.value = 0.0f;
            CurCoolTime = 0.0f;
            IsCoroutineRunning = false;

        }


        /*--- Private Methods ---*/
        void StartCasting(Cast cast)
        {
            //castingSliderObj.SetActive( true );
            //IsCoroutineRunning = true;
            //IsCoolDown = true;

        }
        void EndCasting( Cast cast, CastFuncSet castFunc )
        {
            //slider.value = cast.destRatio;
            //IsFinshCasting = true;
            //castingSliderObj.SetActive( false );

            //if(EndAction != null)
            //{
            //    EndAction();
            //}

            //if ( cast.coolTime != null )
            //{
            //    StartCoroutine( CoolDown( cast.coolTime ) );
            //}

        }


        //IEnumerator AutoCasting( Cast cast, CastFuncSet castFunc )
        //{
        //    StartCasting();

        //    while ( slider.value < cast.destRatio )
        //    {
        //        slider.value += cast.deltaRatio * Time.deltaTime;
        //        if ( castFunc.SyncDataWith != null)
        //        {
        //            castFunc.SyncDataWith( slider.value );
        //        }
        //        yield return null;
        //    }
        //    EndCasting( cast, castFunc.FinishAction );
        //    yield return null;
        //}
        //IEnumerator AutoCasting( Cast cast, CastFuncSet castFunc )
        //{
        //    StartCasting();

        //    while ( slider.value < cast.destRatio )
        //    {
        //        slider.value += cast.deltaRatio * Time.deltaTime;
        //        if ( castFunc.SyncDataWith != null )
        //        {
        //            castFunc.SyncDataWith( slider.value );
        //        }

        //        if ( castFunc.PauseCond != null ) 
        //        {
        //            EndCasting( cast, castFunc.PauseAction );
        //        }

        //        yield return null;
        //    }
        //    EndCasting( cast, castFunc.FinishAction );
        //    yield return null;
        //}

        IEnumerator Casting( Cast cast, CastFuncSet castFunc )
        {
            castingSliderObj.SetActive( true );
            IsCoroutineRunning = true;

            while ( slider.value < cast.destRatio )
            {
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

            FinishCasting( cast, castFunc, true );
            yield return null;
        }


        void FinishCasting(Cast cast, CastFuncSet castFunc, bool isFinish)
        {
            castingSliderObj.SetActive( false );

            castingSliderObj.SetActive( false );
            IsFinshCasting = isFinish;

            if (isFinish)
            {
                if ( castFunc.FinishAction != null )
                {
                    castFunc.FinishAction();
                }
                slider.value = 0.0f;
                CurCoolTime = 0.0f;
            }
            else
            {
                if(castFunc.PauseAction != null)
                {
                    castFunc.PauseAction();
                }
            }

            if ( cast.coolTime != null )
            {
                StartCoroutine( CoolDown( cast.coolTime ) );
            }
            else
            {
                IsCoroutineRunning = false;
            }
        }

        //IEnumerator ManualCasting(Cast cast, Func<bool> IsInputNow, Action<float> SyncDataWith, Action PauseAction, Action FinishAction )
        //{
        //    castingSliderObj.SetActive( true );
        //    IsCoroutineRunning = true;

        //    while ( true )
        //    {
        //        if ( slider.value >= cast.destRatio )
        //        {
        //            Debug.Log( $"Value is {slider.value}" );
        //            IsCoolDown = true;
        //            EndCasting( cast, PauseAction );
        //            yield return null;
        //        }

        //        slider.value += cast.deltaRatio * Time.deltaTime;
        //        if (SyncDataWith != null)
        //        {
        //            SyncDataWith( slider.value );
        //        }
        //        yield return new WaitForEndOfFrame();

        //        if (!IsInputNow())
        //        {
        //            break;  
        //        }
        //    }

        //    if(FinishAction != null)
        //    {
        //        FinishAction();
        //    }
        //    IsCoroutineRunning = false;
        //    castingSliderObj.SetActive( false );
        //    yield return null;
        //}
        

        IEnumerator CoolDown(float? coolTime)
        {
            while(CurCoolTime < coolTime )
            {
                CurCoolTime += Time.deltaTime;
                ResetCasting();
                yield return null;
            }
            yield return null;
        }
    }
}