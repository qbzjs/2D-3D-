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
		/*--- Public Fields ---*/
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
		/*--- Protected Fields ---*/
		protected NetworkGenerator networkGenerator;

		[Header("UI")]
		public DollUI dollUI;
		public ExorcistUI exorcistUI;
		public BarUI_Controller BarUI;

		//protected NetworkGenerator playerGenerator;
		//protected NetworkGenerator normalAltarGenerator;
		//protected NetworkGenerator exitAltarGenerator;
		//protected NetworkGenerator finalAltarGenerator;
		//protected NetworkGenerator purificationBoxGenerator;
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


			//PlayerData 받아온정보를 토대로 어떤 퇴마사인지, 어떤 인형인지.. 결정
			//임시

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

			//int number = PhotonNetwork.LocalPlayer.ActorNumber;
			//if (number == 1)
			//{
			//	playerGenerator = new NetworkGenerator(DollPrefabs[0]);
			//	playerGenerator = new NetworkGenerator(ExorcistPrefabs[0]);

			//}
			//else
			//{
			//	playerGenerator = new NetworkGenerator(ExorcistPrefabs[0]);
			//	playerGenerator = new NetworkGenerator(DollPrefabs[0]);
			//}


			//PlayerData 에서 퇴마사라면 퇴마사의 위치 0 , 인형이라면 순서대로 1,2,3,4 위치로
			//각 인형들이 들어온 순서대로 배열또는 리스트에 넣었기 때문에 해당 인덱스를 이용.
			//GameObject Player = playerGenerator.Generate(PlayerGenPos[number]);
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



			//normalAltarGenerator = new NetworkGenerator(NormalAltarPrefab);
			//exitAltarGenerator = new NetworkGenerator(ExitAltarPrefab);
			//finalAltarGenerator = new NetworkGenerator(FinalAltarPrefab);
			//purificationBoxGenerator = new NetworkGenerator(PurificationBoxPrefab);
			//normalAltarGenerator.GenerateByAlgorithm(Count, NormalAltarGenPos, InitAreaRadius, CenterPosition);
			//exitAltarGenerator.GenerateRandomly(ExitAltarGenPos);
			//finalAltarGenerator.Generate(FinalAltarGenPos.transform.position, Quaternion.Euler(FinalAltarGenPos.transform.rotation.eulerAngles));

			foreach ( var purificationBoxGenPos in PurificationBoxGenPos )
			{
				networkGenerator.Generate( PurificationBoxPrefab, purificationBoxGenPos.transform.position, purificationBoxGenPos.transform.rotation );
			}


			//foreach ( GameObject purificationBoxGenPos in PurificationBoxGenPos)
			//{
			//	purificationBoxGenerator.Generate(purificationBoxGenPos.transform.position, Quaternion.Euler(purificationBoxGenPos.transform.rotation.eulerAngles));
			//}


			
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
		public void SetAltar(InteractionObj InterObj)
		{
			if (InterObj is FinalAltar)
			{
				finalAltar = InterObj as FinalAltar;
			}

			if (InterObj is ExitAltar)
			{
				exitAltar = InterObj as ExitAltar;
			}
		}


		public void DecreaseAltarCount() //normalAltar활성화 됐을때
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


		public void DollCountDecrease() //Ghost가 될떄 (단 부를때 객체에서 바로부르는것이 아닌 RPC로 불러야함)
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

		public void DoExit(NetworkBaseController controller) // 비상탈출구로 나갈때, 탈출구로 나갈때, 빡종할때 (단 부를때 객체에서 바로부르는것이 아닌 RPC로 불러야함)
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