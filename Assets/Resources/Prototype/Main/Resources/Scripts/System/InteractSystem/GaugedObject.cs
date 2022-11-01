using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using TMPro;

using KSH_Lib.Util;

namespace KSH_Lib
{
	[RequireComponent( typeof( PhotonView ) )]
	public abstract class GaugedObject : MonoBehaviourPun, IPunObservable
	{
		/*--- Public Fields ---*/
		[field: SerializeField] public float MaxGauge { get; protected set; }
		[field: SerializeField] public float AddedGauge { get; protected set; }
		[field: SerializeField] public float ReducedGauge { get; protected set; }
		[field: SerializeField] public float CoolTime { get; protected set; }
		[field: SerializeField] public bool IsInRange { get; protected set; }
		public float RateOfGauge { get; protected set; }
		public float OriginGauge { get { return RateOfGauge * MaxGauge; } }
		public bool CanInteract { get { return IsInRange && castingSystem.IsReset; } }

		[SerializeField]
		protected CastingSystem castingSystem;

		[Header( "Trigger Setting" )]
		[SerializeField]
		protected GameObject textUI;
		[SerializeField]
		protected string message = "Press L Click to Interact";
		

		/*--- Protected Fields ---*/
		protected TextMeshProUGUI textTMP;


		/*--- MonoBehaviour Callbacks ---*/
		protected virtual void Start()
        {
			textTMP = textUI.GetComponent<TextMeshProUGUI>();
			if(textTMP == null)
            {
				Debug.LogError( "GaugedObject.Start: Can not find textTMP" );
            }
		}
        protected virtual void Update()
		{
			if ( ResultCondition() )
			{
				DoResult();
			}
		}

        /*--- Public Methods ---*/

		/* Examples
        public void StartAutoCasting()
        {
			castingSystem.StartAutoCastingByRatio( rateAddGauge, CoolTime, SyncDataWith: AddGauage );
        }

		public void DoManualCasting( System.Func<bool> IsInputNow )
        {
			castingSystem.StartManualCastingByRatio( IsInputNow, rateAddGauge, SyncDataWith: AddGauage );
        }
		*/
		public abstract void DoResult();
		public abstract bool ResultCondition();

		public void OnPhotonSerializeView( PhotonStream stream, PhotonMessageInfo info )
		{	
		}


		/*--- Protected Methods ---*/
		protected virtual void ActiveText()
        {
			textTMP.text = message;
			textUI.SetActive( true );
		}
		protected virtual void InactiveText()
        {
			textUI.SetActive( false );
		}

		protected virtual void SyncGauge( float gauge )
		{
			RateOfGauge = gauge;
			photonView.RPC( "ShareGauge", RpcTarget.AllViaServer, RateOfGauge );
		}


		/*--- Private Methods ---*/
		[PunRPC]
		public void ShareGauge(float gauge_in)
        {
			RateOfGauge = gauge_in;
        }

		protected virtual void HandleTriggerEnter( Collider other ){}
		protected virtual void HandleTriggerStay( Collider other ){}
		protected virtual void HandleTriggerExit( Collider other ){}
	}
}