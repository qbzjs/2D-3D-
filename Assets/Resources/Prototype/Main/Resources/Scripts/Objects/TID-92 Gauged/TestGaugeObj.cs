using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KSH_Lib.Test
{
	public class TestGaugeObj : GaugedObject
	{
        protected override void Update()
        {
            //if(CanInteract)
            //{
            //    if(castingSystem.IsCoroutineRunning)
            //    {
            //        ActivateText( false );
            //    }
            //    else
            //    {
            //        ActivateText( true);
            //    }

            //    if (Input.GetKeyDown(KeyCode.Mouse0))
            //    {
            //        StartAutoCasting();
            //    }

            //    if(Input.GetKeyDown(KeyCode.Mouse1))
            //    {
            //        StartManualCasting();
            //    }
            //}

            base.Update();
        }

        private void OnGUI()
        {
            //GUI.Box( new Rect( 0, 0, 150, 30 ), $"IsCoolDown: {castingSystem.CanCasting}" );
            //GUI.Box( new Rect( 0, 30, 150, 30 ), $"IsFinshCasting: {castingSystem.IsFinshCasting}" );
            ////GUI.Box( new Rect( 0, 60, 150, 30 ), $"IsReset: {castingSystem.IsReset}" );
            //GUI.Box( new Rect( 0, 90, 150, 30 ), $"CanInteract: {CanInteract}" );
            //GUI.Box( new Rect( 0, 120, 150, 30 ), $"IsInRange: {IsInRange}" );
            //GUI.Box( new Rect( 0, 150, 150, 30 ), $"curCoolTime: {castingSystem.CurCoolTime}" );
            //GUI.Box( new Rect( 0, 180, 150, 30 ), $"Slider: {castingSystem.SliderVal}" );
            //GUI.Box( new Rect( 0, 210, 150, 30 ), $"Gauge: {RateOfGauge}" );
            //GUI.Box( new Rect( 0, 240, 150, 30 ), $"RateOfGauge: {OriginGauge}" );
        }
        protected override bool InteractCondition()
        {
            throw new System.NotImplementedException();
        }
        protected override void DoResult()
        {
            Debug.Log("Finish Casting!!!");
        }
        protected override bool ResultCondition()
        {
            return castingSystem.IsFinshCasting;
        }

        protected override void HandleTriggerEnter( Collider other )
        {
            if(other.gameObject.CompareTag("Exorcist"))
            {
                Debug.Log( "Trigger Entered" );
                IsInRange = true;
                ActivateText( true );
            }
        }
        protected override void HandleTriggerExit( Collider other )
        {
            if ( other.gameObject.CompareTag( "Exorcist" ) )
            {
                Debug.Log( "Trigger Exited" );
                IsInRange = false;
                ActivateText( false );
            }
        }

        protected override void TryInteract()
        {
            throw new System.NotImplementedException();
        }

        void StartAutoCasting()
        {
            ActivateText( false );
            //castingSystem.StartAutoCasting( CastingSystem.Cast.CreateByTime(3.0f, coolTime: CoolTime), SyncDataWith: SyncGauge );
        }
        void StartManualCasting()
        {
            ActivateText( false );
            //castingSystem.StartManualCasting( CastingSystem.Cast.CreateByRatio(0.3f), IsInputNow, SyncDataWith: SyncGauge );
        }
        bool IsInputNow()
        {
            return Input.GetKey( KeyCode.Mouse1 );
        }
        protected void SyncGaugeAndTurnOffText( float gauge )
        {
            base.SyncGauge( gauge );
            ActivateText( false );
        }
    }
}