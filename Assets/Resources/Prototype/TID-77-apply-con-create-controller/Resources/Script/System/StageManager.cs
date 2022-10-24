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

		[Header("GenPos")]
		public Vector3[] PlayerGenPos;
		public GameObject[] NormalAltarGenPos;
		public GameObject[] ExitAltarGenPos;
		public GameObject FinalAltarGenPos;

		[Header("NormalAltarSetting")]
		public int Count;
		public float InitAreaRadius;
		public Vector3 CenterPosition;
		/*--- Protected Fields ---*/
		protected NetworkGenerator PlayerGenerator;
		protected NetworkGenerator NormalAltarGenerator;
		protected NetworkGenerator ExitAltarGenerator;
		protected NetworkGenerator FinalAltarGenerator;
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
			//PlayerData �޾ƿ������� ���� � �𸶻�����, � ��������.. ����
			PlayerGenerator = new NetworkGenerator(DollPrefabs[0]);
			NormalAltarGenerator = new NetworkGenerator(NormalAltarPrefab);
			ExitAltarGenerator = new NetworkGenerator(ExitAltarPrefab);
			FinalAltarGenerator = new NetworkGenerator(FinalAltarPrefab);

			
			//PlayerData ���� �𸶻��� �𸶻��� ��ġ 0 , �����̶�� ������� 1,2,3,4 ��ġ��
			//�� �������� ���� ������� �迭�Ǵ� ����Ʈ�� �־��� ������ �ش� �ε����� �̿�.
			PlayerGenerator.Generate(PlayerGenPos[0]);
			




			NormalAltarGenerator.GenerateByAlgorithm(Count, NormalAltarGenPos, InitAreaRadius, CenterPosition);

		}
		void Update()
		{
		
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

		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
	}
}