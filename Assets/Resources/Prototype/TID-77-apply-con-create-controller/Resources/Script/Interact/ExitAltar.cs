using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
	public class ExitAltar : GaugedObject
	{
		/*--- Public Fields ---*/
		public GameObject ExitAltarModel;

		/*--- Protected Fields ---*/
		protected bool isOpen = false;

        /*--- Private Fields ---*/


        /*--- MonoBehaviour Callbacks ---*/
        protected override void OnEnable()
		{
			base.OnEnable();
			castingSystem = StageManager.Instance.CastSystem;
			StageManager.Instance.SetAltar( this );
			ExitAltarModel.SetActive( false );
			//castingSystem.ResetCasting();
		}

		protected override void DoResult()
		{
			isOpen = false;
		}
		protected override bool ResultCondition()
		{
			return isOpen && castingSystem.IsFinshCasting;
		}
        protected override bool InteractCondition()
		{
			return false;
		}
        protected override void TryInteract()
        {
            throw new System.NotImplementedException();
        }


        /*--- Public Methods ---*/
        public void OpenExitAltar()
		{
			isOpen = true;
			ExitAltarModel.SetActive( true );
		}
	
	}
}