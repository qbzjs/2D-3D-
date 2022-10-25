using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using KSH_Lib;
namespace GHJ_Lib
{
	public class StageManager : MonoBehaviour
	{
		/*--- Public Fields ---*/
		public static StageManager Instance
		{
			get 
			{
				if (instance == null)
				{
					Debug.LogError("Not Exist StageManger!");
				}
				return instance; 
			}
		}
		[Header("Camera")]
		public GameObject FPV_Cam;
		public GameObject TPV_Cam;

		[Header("Prefabs")]
		public GameObject[] DollPrefabs;
		public GameObject[] ExorcistPrefabs;
		public GameObject NormalAltarPrefab;
		public GameObject ExitAltarPrefab;
		public GameObject FinalAltarPrefab;
		public GameObject PurificationBoxPrefab;

		[Header("GenPos")]
		public Vector3[] PlayerGenPos;
		public GameObject[] NormalAltarGenPos;
		public GameObject[] ExitAltarGenPos;
		public GameObject FinalAltarGenPos;
		public GameObject[] PurificationBoxGenPos;

		[Header("NormalAltarSetting")]
		public int Count;
		public float InitAreaRadius;
		public Vector3 CenterPosition;
		/*--- Protected Fields ---*/
		protected NetworkGenerator playerGenerator;
		protected NetworkGenerator normalAltarGenerator;
		protected NetworkGenerator exitAltarGenerator;
		protected NetworkGenerator finalAltarGenerator;
		protected NetworkGenerator purificationBoxGenerator;
		/*--- Private Fields ---*/
		static StageManager instance;

        /*--- MonoBehaviour Callbacks ---*/
        void Awake()
        {
			Instantiate(FPV_Cam);
			Instantiate(TPV_Cam);
		}
        void Start()
		{
			//PlayerData 받아온정보를 토대로 어떤 퇴마사인지, 어떤 인형인지.. 결정
			playerGenerator = new NetworkGenerator(DollPrefabs[0]);

			//PlayerData 에서 퇴마사라면 퇴마사의 위치 0 , 인형이라면 순서대로 1,2,3,4 위치로
			//각 인형들이 들어온 순서대로 배열또는 리스트에 넣었기 때문에 해당 인덱스를 이용.
			playerGenerator.Generate(PlayerGenPos[PhotonNetwork.LocalPlayer.ActorNumber]);

			normalAltarGenerator = new NetworkGenerator(NormalAltarPrefab);
			exitAltarGenerator = new NetworkGenerator(ExitAltarPrefab);
			finalAltarGenerator = new NetworkGenerator(FinalAltarPrefab);
			purificationBoxGenerator = new NetworkGenerator(PurificationBoxPrefab);

			if (!PhotonNetwork.IsMasterClient)
			{
				return;
			}


			normalAltarGenerator.GenerateByAlgorithm(Count, NormalAltarGenPos, InitAreaRadius, CenterPosition);
			exitAltarGenerator.GenerateRandomly(ExitAltarGenPos);
			finalAltarGenerator.Generate(FinalAltarGenPos.transform.position, Quaternion.Euler(FinalAltarGenPos.transform.rotation.eulerAngles));

			foreach (GameObject purificationBoxGenPos in PurificationBoxGenPos)
			{
				purificationBoxGenerator.Generate(purificationBoxGenPos.transform.position, Quaternion.Euler(purificationBoxGenPos.transform.rotation.eulerAngles));
			}
		}



		/*--- Public Methods ---*/
		public static void CharacterLayerChange(GameObject Model, int layer)
		{
			Model.layer = layer;
			int count = Model.transform.childCount;
			Debug.Log("count : " + count);
			if (count != 0)
			{
				for (int i = 0; i < count; ++i)
				{
					CharacterLayerChange(Model.transform.GetChild(i).gameObject, layer);
				}
			}
			else
			{
				return;
			}
		}

		public void ExitMapAlone()
		{
			
		}
		public void EndStage()
		{
			
		}
		
		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
	}
}