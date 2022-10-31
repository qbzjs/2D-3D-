using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KSH_Lib.Test
{
	public class TestCasting : MonoBehaviour
	{
        /*--- Public Fields ---*/
        public CastingSystem castingSystem;
        bool isInTrigger;

        /*--- Protected Fields ---*/


        /*--- Private Fields ---*/
        /*--- MonoBehaviour Callbacks ---*/

        private void Update()
        {
            //if(isInTrigger)
            //{
                
            //}

            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log( "Start Auto Casting By Ratio (delta = 0.3, cooltime = 1.0f)" );
                castingSystem.StartAutoCastingByRatio( 0.3f, 1.0f );
            }
            if ( Input.GetKeyDown( KeyCode.Alpha2 ) )
            {
                Debug.Log( "Start Auto Casting Now  (delta = 0.3, No Colltime) " );
                castingSystem.StartAutoCastingByRatio( 0.3f );
            }
            if ( Input.GetKeyDown( KeyCode.Alpha3 ) )
            {
                Debug.Log( "Start Auto Casting By Time ( total time = 1, cooltime = 1.0f)" );
                castingSystem.StartAutoCastingByTime( 1.0f, 1.0f);
            }
            if ( Input.GetKeyDown( KeyCode.Alpha4 ) )
            {
                Debug.Log( "Start Auto Casting Now  (total time = 1, No Colltime) " );
                castingSystem.StartAutoCastingByRatio( 1.0f );
            }
         
             castingSystem.StartManualCastingByRatio( IsInputNow, 0.1f );


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
        /*--- Public Methods ---*/

        bool IsInputNow()
        {
            return Input.GetKey( KeyCode.Alpha5 );
        }

        /*--- Protected Methods ---*/


        /*--- Private Methods ---*/
    }
}