using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KSH_Lib.Test
{
	public class TestCasting : MonoBehaviour
	{
        public CastingSystem castingSystem;
        bool isInTrigger;

        private void Update()
        {
            // AutoCasting By Ratio (deltaRatio = 0.3, cooltime = 1.0f)
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log( "Start Auto Casting By Ratio (delta = 0.3, cooltime = 1.0f)" );
                castingSystem.StartAutoCastingByRatio( 0.3f, 1.0f );
            }

            // AutoCasting By Ratio (deltaRatio = 0.3, No CoolTime)
            if ( Input.GetKeyDown( KeyCode.Alpha2 ) )
            {
                Debug.Log( "Start Auto Casting Now  (delta = 0.3, No cooltime) " );
                castingSystem.StartAutoCastingByRatio( 0.3f );
            }

            // AutoCasting By Time (totalTime = 1, cooltime = 1.0f)
            if ( Input.GetKeyDown( KeyCode.Alpha3 ) )
            {
                Debug.Log( "Start Auto Casting By Time ( total time = 1, cooltime = 1.0f)" );
                castingSystem.StartAutoCastingByTime( 1.0f, 1.0f);
            }

            // AutoCasting By Time (totalTime = 1, No CoolTime)
            if ( Input.GetKeyDown( KeyCode.Alpha4 ) )
            {
                Debug.Log( "Start Auto Casting Now  (total time = 1, No cooltime) " );
                castingSystem.StartAutoCastingByRatio( 1.0f );
            }

            // ManualCasting By Ratio (totalTime = 1, deltaRatio = 0.3)
            castingSystem.StartManualCastingByRatio( IsInputNow, 0.3f);

            // Reset Casting System when No CoolTime
            if ( Input.GetKeyDown( KeyCode.Alpha0 ) )
            {
                Debug.Log( "Reset Casting" );
                castingSystem.ResetCasting();
            }
        }

        private void OnGUI()
        {
            GUI.Box( new Rect( 0, 0, 150, 30 ), $"IsFinishCasting: {castingSystem.IsFinshCasting}" );
            GUI.Box( new Rect( 0, 30, 150, 30 ), $"IsCoolDown: {castingSystem.IsCoolDown}" );
            GUI.Box( new Rect( 0, 60, 150, 30 ), $"Cooltime: {castingSystem.CurCoolTime}" );
            GUI.Box( new Rect( 0, 90, 150, 30 ), $"Slider: {castingSystem.SliderVal}" );
        }

        private void OnTriggerEnter( Collider other )
        {
            if ( other.gameObject.CompareTag( "Exorcist" ) )
            {
                isInTrigger = true;
            }
        }
        private void OnTriggerExit( Collider other )
        {
            if ( other.gameObject.CompareTag( "Exorcist" ) )
            {
                isInTrigger = false;
            }
        }
        bool IsInputNow()
        {
            return Input.GetKey( KeyCode.Alpha5 );
        }
    }
}