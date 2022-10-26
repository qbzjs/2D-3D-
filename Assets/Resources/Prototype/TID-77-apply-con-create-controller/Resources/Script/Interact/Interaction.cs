using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
using Photon.Realtime;
namespace GHJ_Lib
{
	public enum CastingType { Casting, AutoCasting, AutoCastingNull,NotCasting };

	public class Interaction :MonoBehaviourPun,IPunObservable
	{
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

		public virtual CastingType GetCastingType(BasePlayerController player)
		{
			if (player is DollController)
			{
				return CastingType.Casting;
			}

			if (player is ExorcistController)
			{
				return CastingType.Casting;
			}
			return CastingType.Casting;
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

		virtual public void Interact(BasePlayerController controller)
		{
			if (controller is DollController)
			{
				
			}

			if (controller is ExorcistController)
			{
				
			}
		}

		public void FinishAutoCasting()
		{
			StartCoroutine("StartInteractCoolTime", 10);
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

		protected virtual IEnumerator StartInteractCoolTime(float CoolTime)
		{
			CanActiveToExorcist = false;
			yield return new WaitForSeconds(CoolTime);
			CanActiveToExorcist = true;
		}

		protected virtual IEnumerator AutoCasting(float CoolTime)
		{
			yield return new WaitForSeconds(CoolTime);
		}

		protected virtual IEnumerator AutoCastingNull(float CoolTime)
		{
			yield return new WaitForSeconds(CoolTime);
		}



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