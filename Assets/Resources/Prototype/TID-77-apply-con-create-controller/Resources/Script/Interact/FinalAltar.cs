using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
	public class FinalAltar: GaugedObject
	{
		bool canOpenDoor = false;

		/*--- MonoBehaviour Callbacks ---*/
		void OnEnable()
		{
			StageManager.Instance.SetAltar(this);
			castingSystem.ResetCasting();
		}

        protected override void Update()
        {
            if ( !canOpenDoor )
            {
                return;
            }

            if ( castingSystem.IsFinshCasting )
            {
                OpenDoor();
            }
        }

        protected override void DoResult()
        {
        }
        protected override bool ResultCondition()
        {
            return true;
        }

        /*--- Interaction Methods ---*/
        public void CanOpenDoor()
		{
			canOpenDoor = true;
		}

        void OpenDoor()
        {
            if ( this.transform.position.y < -this.transform.localScale.y )
            {
                return;
            }
            this.transform.position -= new Vector3( 0, Time.deltaTime, 0 );
        }
    }
}