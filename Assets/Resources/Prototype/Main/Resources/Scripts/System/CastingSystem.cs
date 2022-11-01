using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;

namespace KSH_Lib
{
    public class CastingSystem : MonoBehaviour
    {
        /*--- Public Fields ---*/
        public bool IsFinshCasting { get; private set; }
        public bool IsCoolDown { get; private set; }
        public bool IsReset { get { return !IsFinshCasting && !IsCoolDown; } }
        public float CurCoolTime { get; private set; }
        public float SliderVal { get { return slider.value; } }
        public bool IsCoroutineRunning { get; private set; }
    

        /*--- Private Fields ---*/
        [SerializeField]
        GameObject castingSliderObj;
        Slider slider;
        //bool isManualCoroutineStarted;
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
        public void StartAutoCastingByRatio( float deltaRatio, float? coolTime = null, float destRatio = 1.0f, Action<float> SyncDataWith = null )
        {
            if( IsCoolDown )
            {
                Debug.LogWarning( "CastingSystem.StartAutoCastingByRatio: castingSlider is already activated!" );
                return;
            }
            curCoroutine = StartCoroutine( AutoCastingByRatio( deltaRatio, coolTime, destRatio, SyncDataWith ) );
        }
        public void StartAutoCastingByTime( float castTime, float? coolTime = null, float destRatio = 1.0f, Action<float> SyncDataWith = null )
        {
            if ( IsCoolDown )
            {
                Debug.LogWarning( "CastingSystem.StartAutoCastingByTime: castingSlider is already activated!" );
                return;
            }
            curCoroutine = StartCoroutine( AutoCastingByTime( castTime, coolTime, destRatio, SyncDataWith ) );
        }
        public void StartManualCastingByRatio( Func<bool> IsInputNow, float deltaRatio, float destRatio = 1.0f, Action<float> SyncDataWith = null )
        {
            if ( IsCoolDown || !IsInputNow() )
            {
                return;
            }

            else if( !IsCoroutineRunning )
            {
                Debug.Log( $"Start Manual Casting Now  (delta = {deltaRatio}");
                curCoroutine = StartCoroutine( ManualCastingByRatio( IsInputNow, deltaRatio, destRatio, SyncDataWith ) );
            }
        }
        public void ForceSetCastingTo(float ratio)
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

        IEnumerator AutoCastingByRatio( float deltaCastingRatio, float? coolTime, float destRatio, Action<float> SyncDataWith )
        {
            StartCasting();

            while ( slider.value < destRatio )
            {
                slider.value += deltaCastingRatio * Time.deltaTime;
                if (SyncDataWith != null)
                {
                    SyncDataWith( slider.value );
                }
                yield return null;
            }

            EndCasting( destRatio, coolTime );
            yield return null;
        }

        IEnumerator AutoCastingByTime( float castTime, float? coolTime, float destRatio, Action<float> SyncDataWith )
        {
            float deltaCastingRatio = 1 / castTime;
            yield return AutoCastingByRatio( deltaCastingRatio, coolTime, destRatio, SyncDataWith );
        }
        IEnumerator ManualCastingByRatio( Func<bool> IsInputNow, float deltaCastingRatio, float destRatio, Action<float> SyncDataWith )
        {
            castingSliderObj.SetActive( true );
            IsCoroutineRunning = true;

            while ( true )
            {
                if ( slider.value >= destRatio )
                {
                    Debug.Log( $"Value is {slider.value}" );
                    IsCoolDown = true;
                    EndCasting( destRatio );
                    yield return null;
                }

                slider.value += deltaCastingRatio * Time.deltaTime;
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