using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using KSH_Lib;
using LSH_Lib;
namespace GHJ_Lib
{
	public class StageManager : MonoBehaviour
	{

		/*--- Fields ---*/
		public static StageManager Instance
		{
			get
			{
				if ( instance == null )
				{
					Debug.LogError("Not Exist StageManger!");
				}
				return instance; 
			}
		}

		/*--- Prefabs ---*/
		[Header("Prefabs")]
		public GameObject[] DollPrefabs;
		public GameObject[] ExorcistPrefabs;
		public GameObject NormalAltarPrefab;
		public GameObject ExitAltarPrefab;
		public GameObject FinalAltarPrefab;
		public GameObject PurificationBoxPrefab;

		[Header("GenPos")]
		public Transform[] PlayerGenPos;
		public Transform[] NormalAltarGenPos;
		public Transform[] ExitAltarGenPos;
		public Transform FinalAltarGenPos;
		public Transform[] PurificationBoxGenPos;

		[Header("NormalAltarSetting")]
		public int Count;
		public float InitAreaRadius;
		public Vector3 CenterPosition;

		[Header("UI")]
		public DollUI dollUI;
		public ExorcistUI exorcistUI;
		public GameObject InteractTextUI;

		[field: SerializeField] public CastingSystem CastSystem { get; private set; }

		NetworkGenerator networkGenerator;

		/*--- Private Fields ---*/
		static StageManager instance;
		bool activeDebugGUI;


		/*--- MonoBehaviour Callbacks ---*/
		void Awake()
        {
			DataManager.Instance.InitPlayerDatas();
			DataManager.Instance.ShareAllData();
		}
        void Start()
		{
			instance = this;


			//PlayerData �޾ƿ������� ���� � �𸶻�����, � ��������.. ����
			//�ӽ�

			networkGenerator = new NetworkGenerator(
				new GameObject[]{
					DollPrefabs[0], ExorcistPrefabs[0],
					DollPrefabs[1], ExorcistPrefabs[1],
					DollPrefabs[2], ExorcistPrefabs[2],
					DollPrefabs[3], ExorcistPrefabs[3],
					DollPrefabs[4], ExorcistPrefabs[4],
					NormalAltarPrefab, ExitAltarPrefab, FinalAltarPrefab,
					PurificationBoxPrefab
					}
				); ;
			int number = DataManager.Instance.PlayerIdx;

			GameObject targetPrefab;
			if (number == 0)
            {
				targetPrefab = ExorcistPrefabs[(int)DataManager.Instance.LocalPlayerData.roleData.TypeOrder];
				exorcistUI.gameObject.SetActive(true);
			}
			else
			{
				targetPrefab = DollPrefabs[(int)DataManager.Instance.LocalPlayerData.roleData.TypeOrder - 5];
				dollUI.gameObject.SetActive(true);
			}

			GameObject Player = networkGenerator.Generate( targetPrefab, PlayerGenPos[number].position, PlayerGenPos[number].rotation );
			Log.Instance.SetPlayer(Player.transform.GetChild(0).gameObject);


			// end Filed 
			dollCount = PhotonNetwork.CurrentRoom.PlayerCount;
			if (!EscMenu)
			{
				Debug.LogError(" EscMenu is Null");
			}
			EscMenu.controller = Player.transform.GetChild(0).gameObject.GetComponent<NetworkBaseController>();
			// 

			if (!PhotonNetwork.IsMasterClient)
			{
				return;
			}

			networkGenerator.GenerateSpread( NormalAltarPrefab, NormalAltarGenPos, Count, InitAreaRadius, CenterPosition );
			networkGenerator.GenerateRandomly( ExitAltarPrefab, ExitAltarGenPos );
			networkGenerator.Generate( FinalAltarPrefab, FinalAltarGenPos.transform.position, FinalAltarGenPos.rotation );

			foreach ( var purificationBoxGenPos in PurificationBoxGenPos )
			{
				networkGenerator.Generate( PurificationBoxPrefab, purificationBoxGenPos.transform.position, purificationBoxGenPos.transform.rotation );
			}


			
		}

		private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha0))
            {
				activeDebugGUI = !activeDebugGUI;
			}
        }

        private void OnGUI()
		{
            if (activeDebugGUI)
            {
                GUI.Box(new Rect(300, 160, 150, 30), $"Local MoveSpeed: {DataManager.Instance.LocalPlayerData.roleData.MoveSpeed}");
                GUI.Box(new Rect(460, 160, 150, 30), $"Local sheetIdx: {DataManager.Instance.LocalPlayerData.accountData.SheetIdx}");

                for (int i = 0; i < DataManager.Instance.PlayerDatas.Count; ++i)
                {
                    GUI.Box(new Rect(300, i * 30, 150, 30), $"MoveSpeed[{i}]: {DataManager.Instance.PlayerDatas[i].roleData.MoveSpeed}");
                    GUI.Box(new Rect(460, i * 30, 150, 30), $"sheetIdx[{i}]: {DataManager.Instance.PlayerDatas[i].accountData.SheetIdx}");
                }
            }
        }

		/*--- Public Methods ---*/
		public static void CharacterLayerChange(GameObject Model, int layer)
		{
			Model.layer = layer;
			int count = Model.transform.childCount;
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

		public void Disappear(GameObject gameObject)
		{
			gameObject.SetActive(false);
		}






		/*---End Field---*/

		public EscMenu EscMenu;
		FinalAltar finalAltar;
		ExitAltar exitAltar;
		[SerializeField]
		protected int dollCount;
		int needAltarCount = 4;
		/*---End Func---*/
		public void SetAltar(GaugedObject gaugedObj)
		{
			if ( gaugedObj is FinalAltar)
			{
				finalAltar = gaugedObj as FinalAltar;
			}

			if ( gaugedObj is ExitAltar)
			{
				exitAltar = gaugedObj as ExitAltar;
			}
		}


		public void DecreaseAltarCount() //normalAltarȰ��ȭ ������
		{
			if (needAltarCount > 0)
			{
				needAltarCount--;
			}

			if (needAltarCount <= 0)
			{
				finalAltar.CanOpenDoor();
			}
		}


		public void DollCountDecrease() //Ghost�� �ɋ� (�� �θ��� ��ü���� �ٷκθ��°��� �ƴ� RPC�� �ҷ�����)
		{
			if (dollCount > 0)
			{
				--dollCount;
			}

			if (dollCount == 2)
			{
				exitAltar.OpenExitAltar();
			}

			if (dollCount == 1)
			{
				EndGame();
			}
		}

		public void EndGame()
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			PhotonNetwork.LeaveRoom();
			GameManager.Instance.LoadScene("99_GameResultScene");
		}

		public void DoExit(NetworkBaseController controller) // ���Ż�ⱸ�� ������, Ż�ⱸ�� ������, �����Ҷ� (�� �θ��� ��ü���� �ٷκθ��°��� �ƴ� RPC�� �ҷ�����)
		{
			if (controller is DollController)
			{

				DollCountDecrease();
				if (controller.photonView.IsMine)
				{
					EndGame();
				}
				else
				{
					controller.transform.parent.gameObject.SetActive(false);
				}

			}


		}
		/*
		public static void CharacterLayerChange(GameObject Model, int layer)
		{
			Model.layer = layer;
			int count = Model.transform.childCount;
			//Debug.Log("count : " + count);
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
		*/
		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
	}
}