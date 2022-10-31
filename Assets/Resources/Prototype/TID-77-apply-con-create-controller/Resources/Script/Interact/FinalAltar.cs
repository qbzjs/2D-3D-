using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using KSH_Lib;

namespace GHJ_Lib
{
	public class FinalAltar: InteractionObj
	{
		/*--- Public Fields ---*/
		
		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/
		
		bool canOpenDoor = false;
		ExitAltar exitAltar;
		/*--- MonoBehaviour Callbacks ---*/
		void OnEnable()
		{
			StageManager.Instance.SetAltar(this);
			curGauge = 0.0f;
			CanActiveToExorcist = false;
			CanActiveToDoll = false;
		}

        void Update()
        {
            if (!canOpenDoor)
            {
                return;
            }

            if (curGauge >= 1.0f)
            {
                OpenDoor();
            }
        }

        public override CastingType GetCastingType(NetworkBaseController player)
		{
			if (player is DollController)
			{
				if (curGauge >= 1.0f)
				{
					return CastingType.NotCasting;
				}

				if (canOpenDoor)
				{ 
					return CastingType.ManualCasting;
				}

				
			}		
			return CastingType.NotCasting;
		}

		/*--- Interaction Methods ---*/

		public void CanOpenDoor()
		{
			canOpenDoor = true;
		}

	

		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
		void OpenDoor()
		{
			if (this.transform.position.y < -this.transform.localScale.y)
			{
				return;
			}
			this.transform.position -= new Vector3(0, Time.deltaTime, 0);
		}
	}
}