using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
using Photon.Realtime;
namespace GHJ_Lib
{

	public class InteractionObj :MonoBehaviourPun,IPunObservable
	{
		public enum CastingType { ManualCasting, SharedAutoCasting, LocalAutoCasting,NotCasting };
		
		/*--- Public Fields ---*/
		public bool CanActiveToExorcist = true;
		public bool CanActiveToDoll = true;
		public bool IsCasting = false;
		public bool IsAutoCasting = false;
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

		public virtual CastingType GetCastingType(NetworkBaseController player)
		{
			if (player is DollController)
			{
				return CastingType.ManualCasting;
			}

			if (player is ExorcistController)
			{
				return CastingType.ManualCasting;
			}
			return CastingType.ManualCasting;
		}

		public void AddGauge(float Gauge)
		{
			curGauge += Gauge;
		}			
		public void UpdateCurGauge(float Gauge)
		{
			curGauge = Gauge;
		}

		public void UpdateCurGaugeRate(float valueRate)
		{
			curGauge = valueRate * maxGauge;
		}



		/*--- Protected Methods ---*/


		public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
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