using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using KSH_Lib;
using Photon.Pun;


namespace KSH_Lib
{
	[RequireComponent( typeof( PhotonView ) )]
	public abstract class GauagedObject : MonoBehaviourPun, IPunObservable
	{
		/*--- Public Fields ---*/
		[SerializeField] public float MaxGauage { get; protected set; }
		[SerializeField] public float ReducedGauage { get; protected set; }
		public float Gauage { get; protected set; }
		public float RateOfGauage { get { return Gauage / MaxGauage; } }

		CastingSystem casting;

		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/


		/*--- MonoBehaviour Callbacks ---*/
		protected virtual void Update()
		{
			if ( ResultCondition() )
			{
				DoResult();
			}
		}

		/*--- Public Methods ---*/
		public void AddGauage( float value )
		{
			Gauage += value;
		}
		public void SetGauge( float gauge )
		{
			Gauage = gauge;
		}
		public void AddGaugeByRate( float rate )
		{
			Gauage = rate * MaxGauage;
		}

		public abstract void DoResult();
		public abstract bool ResultCondition();

		public void OnPhotonSerializeView( PhotonStream stream, PhotonMessageInfo info )
		{	
		}

		/*--- Protected Methods ---*/



		/*--- Private Methods ---*/
	}
}