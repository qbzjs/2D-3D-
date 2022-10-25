using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
using Photon.Realtime;
namespace GHJ_Lib
{
	public class Interaction :MonoBehaviour,IPunObservable
	{
		/*--- Public Fields ---*/
		public bool CanActiveToExorcist = true;
		public bool CanActiveToDoll = true;
		public bool IsCasting = false;
		public float GetGaugeRate
		{
			get { return curGauge / maxGauge; }
		}
		/*--- Protected Fields ---*/
		[SerializeField]
		protected float maxGauge = 10.0f;
		[SerializeField]
		protected float reduction = 0.5f;
		protected float curGauge = 0;
		/*--- Private Fields ---*/


		/*--- Public Methods ---*/
		public void UpdateCurGauge(float Gauge)
		{
			curGauge = Gauge;
		}

		public void UpdateCurGaugeRate(float value)
		{
			curGauge = value*maxGauge;
		}

		virtual public void Interact(BasePlayerController controller)
		{
			if (controller is DollController)
			{
				
			}

			if (controller is ExorcistController)
			{
				
			}
		}

		/*--- Protected Methods ---*/
		virtual protected void Casting(BasePlayerController controller)
		{
			
		}
		virtual protected void AutoCasting(BasePlayerController controller)
		{
			
		}
		virtual protected void Immediate(BasePlayerController controller)
		{
			
		}

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.IsWriting)
			{
				stream.SendNext(curGauge);
				
			}
			if (stream.IsReading)
			{
				curGauge = (float)stream.ReceiveNext();
			}
        }
        /*--- Private Methods ---*/
    }
}