using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;

namespace KSH_Lib
{
    public class CastingSystem : MonoBehaviour
    {
        public enum CastingType
        {
            Manual,
            Auto,
        }

        /*--- Public Fields ---*/
        public bool IsFinshCasting { get; private set; }
        public bool IsCoolDown { get; private set; }
        public float CurCoolTime { get; private set; }

        public bool isManualCoroutineStarted;

        /*--- Protected Fields ---*/


        /*--- Private Fields ---*/
        [SerializeField]
        GameObject castingSliderObj;
        Slider slider;


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


        public void StartAutoCastingByRatio( float deltaRatio, float? coolTime = null, float destRatio = 1.0f )
        {
            if( IsCoolDown )
            {
                Debug.LogWarning( "CastingSystem.StartAutoCastingByRatio: castingSlider is already activated!" );
                return;
            }

            StartCoroutine( AutoCastingByRatio( deltaRatio, destRatio, coolTime ) );
        }
        public void StartAutoCastingByTime( float castTime, float? coolTime = null, float destRatio = 1.0f )
        {
            if ( IsCoolDown )
            {
                Debug.LogWarning( "CastingSystem.StartAutoCastingByTime: castingSlider is already activated!" );
                return;
            }
            StartCoroutine( AutoCastingByTime( castTime, destRatio, coolTime ) );
        }
        public void StartManualCastingByRatio( Func<bool> IsInputNow, float deltaRatio, float? coolTime = null,  float destRatio = 1.0f)
        {
            if ( IsCoolDown || !IsInputNow() )
            {
                return;
            }

            if( IsInputNow() && !isManualCoroutineStarted )
            {
                Debug.Log( $"Start Manual Casting Now  (delta = {deltaRatio}, CoolTime = {coolTime}) " );
                StartCoroutine( ManualCastingByRatio( IsInputNow, deltaRatio, destRatio, coolTime ) );
            }
        }

        /*--- Protected Methods ---*/


        /*--- Private Methods ---*/
        IEnumerator AutoCastingByRatio( float deltaCastingRatio, float destRatio, float? coolTime )
        {
            StartCasting();

            while ( slider.value < destRatio )
            {
                slider.value += deltaCastingRatio * Time.deltaTime;
                yield return null;
            }

            EndCasting( destRatio, coolTime );
            yield return null;
        }

        IEnumerator AutoCastingByTime( float castTime, float destRatio, float? coolTime )
        {
            float deltaCastingRatio = 1 / castTime;
            yield return AutoCastingByRatio( deltaCastingRatio, destRatio, coolTime );
        }
        IEnumerator ManualCastingByRatio( Func<bool> IsInputNow, float deltaCastingRatio, float destRatio, float? coolTime )
        {
            StartCasting();
            slider.enabled = true;
            isManualCoroutineStarted = true;

            while ( true )
            {
                Debug.Log( "ManualCastingByRatio Coroutine is working" );   

                if ( slider.value >= destRatio )
                {
                    Debug.Log( $"Value is {slider.value}" );

                    EndCasting( destRatio, coolTime );
                    yield return null;
                }

                slider.value += deltaCastingRatio * Time.deltaTime;
                Debug.Log( $"slider Value is {slider.value}" );
                yield return new WaitForEndOfFrame();

                if (!IsInputNow())
                {
                    break;  
                }
            }

            isManualCoroutineStarted = false;
            IsCoolDown = false;
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

        void StartCasting()
        {
            castingSliderObj.SetActive( true );
            IsCoolDown = true;
        }
        void EndCasting( float destRatio, float? coolTime = null)
        {
            slider.value = destRatio;
            IsFinshCasting = true;
            castingSliderObj.SetActive( false );

            if(coolTime != null)
            {
                StartCoroutine( CoolDown( coolTime ) );
            }
        }

        public void ResetCasting()
        {
            if ( !IsFinshCasting )
            {
                Debug.LogWarning( "CastingSystem.ResetCasting: Called Reset when not finishing Casting" );
                return;
            }

            IsFinshCasting = false;
            slider.value = 0.0f;
            IsCoolDown = false;
            CurCoolTime = 0.0f;
        }
    }
}