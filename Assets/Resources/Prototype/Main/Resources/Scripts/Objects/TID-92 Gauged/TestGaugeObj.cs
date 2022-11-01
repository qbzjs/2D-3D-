using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KSH_Lib.Test
{
	public class TestGaugeObj : GaugedObject
	{
        protected override void Update()
        {
            if(CanInteract)
            {
                if(castingSystem.IsCoroutineRunning)
                {
                    InactiveText();
                }
                else
                {
                    ActiveText();
                }

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    StartAutoCasting();
                }

                if(Input.GetKeyDown(KeyCode.Mouse1))
                {
                    StartManualCasting();
                }
            }

            base.Update();
        }

        private void OnGUI()
        {
            GUI.Box( new Rect( 0, 0, 150, 30 ), $"IsCoolDown: {castingSystem.IsCoolDown}" );
            GUI.Box( new Rect( 0, 30, 150, 30 ), $"IsFinshCasting: {castingSystem.IsFinshCasting}" );
            GUI.Box( new Rect( 0, 60, 150, 30 ), $"IsReset: {castingSystem.IsReset}" );
            GUI.Box( new Rect( 0, 90, 150, 30 ), $"CanInteract: {CanInteract}" );
            GUI.Box( new Rect( 0, 120, 150, 30 ), $"IsInRange: {IsInRange}" );
            GUI.Box( new Rect( 0, 150, 150, 30 ), $"curCoolTime: {castingSystem.CurCoolTime}" );
            GUI.Box( new Rect( 0, 180, 150, 30 ), $"Slider: {castingSystem.SliderVal}" );
            GUI.Box( new Rect( 0, 210, 150, 30 ), $"Gauge: {RateOfGauge}" );
            GUI.Box( new Rect( 0, 240, 150, 30 ), $"RateOfGauge: {OriginGauge}" );
        }

        public override void DoResult()
        {
           // throw new System.NotImplementedException();
        }
        public override bool ResultCondition()
        {
            return false;
            //throw new System.NotImplementedException();
        }

        protected override void HandleTriggerEnter( Collider other )
        {
            if(other.gameObject.CompareTag("Exorcist"))
            {
                Debug.Log( "Trigger Entered" );
                IsInRange = true;
                ActiveText();
            }
        }
        protected override void HandleTriggerExit( Collider other )
        {
            if ( other.gameObject.CompareTag( "Exorcist" ) )
            {
                Debug.Log( "Trigger Exited" );
                IsInRange = false;
                InactiveText();
            }
        }

        void StartAutoCasting()
        {
            InactiveText();
            castingSystem.StartAutoCastingByTime( 3.0f, 0.1f, SyncDataWith: SyncGauge );
        }
        void StartManualCasting()
        {
            InactiveText();
            castingSystem.StartManualCastingByRatio( IsInputNow, 0.1f, SyncDataWith: SyncGauge );
        }
        bool IsInputNow()
        {
            return Input.GetKey( KeyCode.Mouse1 );
        }
        protected void SyncGaugeAndTurnOffText( float gauge )
        {
            base.SyncGauge( gauge );
            InactiveText();
        }
    }
}