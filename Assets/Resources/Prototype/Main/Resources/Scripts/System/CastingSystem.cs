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

        /*--- Public Fields ---*/
        public bool IsFinshCasting { get; private set; }
        public bool IsCoolDown { get; private set; }
        public bool IsReset { get { return !IsFinshCasting && !IsCoolDown; } }
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
        public void StartAutoCasting( Cast cast, Action<float> SyncDataWith = null )
        {
            if( IsCoolDown )
            {
                Debug.LogWarning( "CastingSystem.StartAutoCasting: castingSlider is already activated!" );
                return;
            }
            curCoroutine = StartCoroutine( AutoCasting( cast, SyncDataWith ) );
        }
        public void StartManualCasting( Cast cast, Func<bool> IsInputNow,  Action<float> SyncDataWith = null )
        {
            if ( IsCoolDown || !IsInputNow() )
            {
                return;
            }
            else if( !IsCoroutineRunning )
            {
                Debug.Log( $"Start Manual Casting Now  (delta = {cast.deltaRatio}");
                curCoroutine = StartCoroutine( ManualCasting( cast, IsInputNow, SyncDataWith ) );
            }
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
            if ( !IsFinshCasting )
            {
                Debug.LogWarning( "CastingSystem.ResetCasting: Called Reset when not finishing Casting" );
                return;
            }

            IsFinshCasting = false;
            IsCoroutineRunning = false;
            slider.value = 0.0f;
            IsCoolDown = false;
            CurCoolTime = 0.0f;
        }


        /*--- Private Methods ---*/
        void StartCasting()
        {
            castingSliderObj.SetActive( true );
            IsCoroutineRunning = true;
            IsCoolDown = true;
        }
        void EndCasting( float destRatio, float? coolTime = null )
        {
            slider.value = destRatio;
            IsFinshCasting = true;
            castingSliderObj.SetActive( false );

            if ( coolTime != null )
            {
                StartCoroutine( CoolDown( coolTime ) );
            }
        }

        IEnumerator AutoCasting( Cast cast, Action<float> SyncDataWith )
        {
            StartCasting();

            while ( slider.value < cast.destRatio )
            {
                slider.value += cast.deltaRatio * Time.deltaTime;
                if (SyncDataWith != null)
                {
                    SyncDataWith( slider.value );
                }
                yield return null;
            }

            EndCasting( cast.destRatio, cast.coolTime );
            yield return null;
        }
        IEnumerator ManualCasting(Cast cast, Func<bool> IsInputNow, Action<float> SyncDataWith )
        {
            castingSliderObj.SetActive( true );
            IsCoroutineRunning = true;

            while ( true )
            {
                if ( slider.value >= cast.destRatio )
                {
                    Debug.Log( $"Value is {slider.value}" );
                    IsCoolDown = true;
                    EndCasting(cast.destRatio );
                    yield return null;
                }

                slider.value += cast.deltaRatio * Time.deltaTime;
                if (SyncDataWith != null)
                {
                    SyncDataWith( slider.value );
                }
                yield return new WaitForEndOfFrame();

                if (!IsInputNow())
                {
                    break;  
                }
            }

            IsCoroutineRunning = false;
            castingSliderObj.SetActive( false );
            yield return null;
        }

        IEnumerator CoolDown(float? destTime)
        {
            while( CurCoolTime < destTime)
            {
                CurCoolTime += Time.deltaTime;
                yield return null;
            }
            ResetCasting();
            yield return null;
        }
    }
}