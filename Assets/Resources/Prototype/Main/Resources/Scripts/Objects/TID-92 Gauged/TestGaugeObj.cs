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
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    StartInteract();
                }
            }

            base.Update();
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
                ActiveText();
            }
        }
        protected override void HandleTriggerExit( Collider other )
        {
            if ( other.gameObject.CompareTag( "Exorcist" ) )
            {
                Debug.Log( "Trigger Exited" );
                InactiveText();
            }
        }

        void StartInteract()
        {
            InactiveText();
            castingSystem.StartAutoCastingByTime( 3.0f, 0.1f, SyncDataWith: AddGauage );
        }
    }
}