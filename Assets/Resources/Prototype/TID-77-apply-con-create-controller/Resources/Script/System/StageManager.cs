using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using KSH_Lib;
using LSH_Lib;
using KSH_Lib.Object;

using Cinemachine;
using TMPro;
namespace GHJ_Lib
{
	public class StageManager : MonoBehaviourPunCallbacks, IPunObservable
	{
		/*--- Fields ---*/
		public static StageManager Instance
		{
			get
			{
				if ( instance == null )
				{
					Debug.LogError( "Not Exist StageManger!" );
				}
				return instance;
			}
		}

		/*--- Prefabs ---*/
		[SerializeField] float waitTime = 3.0f;
		[SerializeField] float rotateSpeed = 1.0f;
		[SerializeField] float camResetTime = 2.0f;
		public bool IsGameStart;

		[Header( "Prefabs" )]
		public GameObject[] DollPrefabs;
		public GameObject[] ExorcistPrefabs;
		public GameObject NormalAltarPrefab;
		public GameObject ExitAltarPrefab;
		public GameObject FinalAltarPrefab;
		public GameObject PurificationBoxPrefab;
		public GameObject DoorPrefab;
		public GameObject[] FakeDollPrefabs;

		[Header( "GenPos" )]
		public Transform[] PlayerGenPos;
		public Transform[] NormalAltarGenPos;
		public Transform[] ExitAltarGenPos;
		public Transform FinalAltarGenPos;
		public Transform[] PurificationBoxGenPos;
		public Transform[] CrowGenPos;
		public Transform[] DoorGenPos;
		public Transform[] FakeDollsGenPos;

		[Header( "NormalAltarSetting" )]
		public int Count;
		public float InitAreaRadius;
		public Vector3 CenterPosition;

		[Header("FakeDollsSpawnSetting")]
		public int CountofFakeDolls;

		[Header( "UI" )]
		public DollUI dollUI;
		public ExorcistUI exorcistUI;
		public GameObject BloodUI_Obj;
		public GameObject InteractTextUI;
		public InteractionPromptUI InteractionPrompt;
		public GameObject introUIObj;
		public CanvasGroup introUICanvasGroup;

		[field: SerializeField] public CastingSystem CastSystem { get; private set; }

		NetworkGenerator networkGenerator;
		public NetworkBaseController[] PlayerControllers { get; private set; } = new NetworkBaseController[5];
		public ExorcistController ExorcistCon
		{
			get
			{
				if(exorcistController == null)
                {
					exorcistController = (ExorcistController)PlayerControllers[0];
				}
				return exorcistController;
			}
		}
		ExorcistController exorcistController;


		/*--- Private Fields ---*/
		static StageManager instance;
		bool activeDebugGUI;
		public NetworkBaseController LocalController { get; private set; }


		/*--- MonoBehaviour Callbacks ---*/
		void Awake()
		{
			instance = this;
		}

		void Start()
		{
			DollCount = PhotonNetwork.CurrentRoom.PlayerCount - 1;
			DataManager.Instance.SetNullPlayerToReady();
			InitGenerateor();
			GeneratePlayerCharacter();
			if ( PhotonNetwork.IsMasterClient )
			{
				GenerateObjects();
			}
			StartCoroutine( GameStartSequence() );
		}

		IEnumerator GameStartSequence()
		{
			introUIObj.SetActive(true);
			introUICanvasGroup.alpha = 0.0f;

			introUICanvasGroup.LeanAlpha(1.0f, 0.5f);

			while ( true )
			{
				if ( LocalController != null )
				{
					LocalController.ChangeCameraTo( false );
					LocalController.ChangeMoveFunc( NetworkBaseController.MoveType.Stop );

					LocalController.FPVCam.ActiveCameraUpdate( false );
					LocalController.TPVCam.ActiveCameraUpdate( true );
					LocalController.FPVCam.ActiveCameraControl( false );
					LocalController.TPVCam.ActiveCameraControl( false );
					break;
				}
				yield return null;
			}

			DataManager.Instance.ShareAllData();

			LocalController.TPVCam.SetAxis( new Vector2( rotateSpeed, 0.0f ) );
			while ( true )
			{
				if ( DataManager.Instance.IsAllClientInited )
				{
					break;
				}
				yield return null;
			}

			introUICanvasGroup.LeanAlpha(0.0f, 0.5f);

			yield return new WaitForSeconds( waitTime );
			LocalController.TPVCam.ResetCamTarget( camResetTime );
			yield return new WaitForSeconds( camResetTime );
			introUIObj.SetActive(false);

			LocalController.InitCameraSetting();
			LocalController.ChangeMoveFunc( NetworkBaseController.MoveType.Input );
			LocalController.CurBehavior.PushSuccessorState( new BvIdle() );
			IsGameStart = true;
		}


		private void OnDrawGizmosSelected()
		{
			Gizmos.DrawWireSphere( CenterPosition, InitAreaRadius );
		}

		public override void OnLeftRoom()
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;


			GameManager.Instance.LoadScene( "99_GameResultScene" );
		}

		/*--- Public Methods ---*/
		public void RegisterPlayer( GameObject playerObj )
		{
			NetworkBaseController PlayerController = playerObj.GetComponent<NetworkBaseController>();
			if ( !PlayerController )
			{
				Debug.LogError( "RegisterPlayer() : Player Controller is Null" );
			}
			PlayerControllers[PlayerController.PlayerIndex] = PlayerController;
		}
		public static void CharacterLayerChange( GameObject Model, int layer )
		{
			Model.layer = layer;
			int count = Model.transform.childCount;
			if ( count != 0 )
			{
				for ( int i = 0; i < count; ++i )
				{
					CharacterLayerChange( Model.transform.GetChild( i ).gameObject, layer );
				}
			}
			else
			{
				return;
			}
		}


		/*---End Field---*/
		KSH_Lib.Object.FinalAltar finalAltar;
		KSH_Lib.Object.ExitAltar exitAltar;
		[field: SerializeField] public int DollCount {get; private set;}
		int needAltarCount = 4;

		//>>suhyeon
		public int AltarCount = 4;
		//<<suhyeon

		/*---End Func---*/
		public void SetAltar(GaugedObj gaugedObj)
		{
			if ( gaugedObj is KSH_Lib.Object.FinalAltar)
			{
				finalAltar = gaugedObj as KSH_Lib.Object.FinalAltar;
			}

			if ( gaugedObj is KSH_Lib.Object.ExitAltar )
			{
				exitAltar = gaugedObj as KSH_Lib.Object.ExitAltar;
			}
		}

		public void DecreaseAltarCount() //normalAltar활성화 됐을때
		{
			if (needAltarCount > 0)
			{
				needAltarCount--;
				AltarCount--;
			}

			if (needAltarCount <= 0)
			{
				finalAltar.SetDoorState(KSH_Lib.Object.FinalAltar.AltarState.CanOpen);
			}
		}


        public override void OnMasterClientSwitched( Player newMasterClient )
        {
			if(newMasterClient == PhotonNetwork.LocalPlayer)
			{
				GameManager.Instance.DisconnectAllPlayer();
			}
        }
        public override void OnPlayerLeftRoom( Player otherPlayer )
        {
            base.OnPlayerLeftRoom( otherPlayer );
        }

        public void DecereseDollCount()
		{
			if ( !PhotonNetwork.InRoom )
			{
				return;
			}
			photonView.RPC( "DecreseDollCountRPC", RpcTarget.AllViaServer );
		}
		[PunRPC]
		void DecreseDollCountRPC()
        {
			--DollCount;
			if (DollCount == 1 && exitAltar.ExitAltarModel.activeInHierarchy==false)
			{
				exitAltar.OpenExitAltar();
			}

			if ( DollCount < 1 )
			{
				GameManager.Instance.DisconnectAllPlayer();
			}
		}

		public void ExitGame(NetworkBaseController controller) // 비상탈출구로 나갈때, 탈출구로 나갈때, 빡종할때 (단 부를때 객체에서 바로부르는것이 아닌 RPC로 불러야함)
		{
			if(controller is DollController)
			{
				if ( !controller.IsMine )
				{
					controller.gameObject.SetActive( false );
					return;
				}
				if(PhotonNetwork.InRoom)
				{
					if(controller.CurBehavior is not BvGhost)
					{
						DecereseDollCount();
					}
					PhotonNetwork.LeaveRoom();
				}
			}
			else if(controller is ExorcistController)
            {
				GameManager.Instance.DisconnectAllPlayer();
			}
		}


		void InitGenerateor()
        {
			networkGenerator = new NetworkGenerator(
				new GameObject[]{
					DollPrefabs[0], ExorcistPrefabs[0],
					DollPrefabs[1], ExorcistPrefabs[1],
					DollPrefabs[2], ExorcistPrefabs[2],
					DollPrefabs[3], ExorcistPrefabs[3],
					DollPrefabs[4], ExorcistPrefabs[4],
					NormalAltarPrefab, ExitAltarPrefab, FinalAltarPrefab,
					PurificationBoxPrefab,DoorPrefab
					}
				);
		}

		void GenerateObjects()
		{
			networkGenerator.GenerateSpread(NormalAltarPrefab, NormalAltarGenPos, Count, InitAreaRadius, CenterPosition);
			networkGenerator.GenerateRandomly(ExitAltarPrefab, ExitAltarGenPos);
			networkGenerator.Generate(FinalAltarPrefab, FinalAltarGenPos.transform.position, FinalAltarGenPos.rotation);

			foreach (var purificationBoxGenPos in PurificationBoxGenPos)
			{
				networkGenerator.Generate(PurificationBoxPrefab, purificationBoxGenPos.transform.position, purificationBoxGenPos.transform.rotation);
			}
			foreach (var DoorPos in DoorGenPos)
			{
				networkGenerator.Generate(DoorPrefab, DoorPos.transform.position, DoorPos.transform.rotation);
			}

			for (int i = 0; i < FakeDollsGenPos.Length; ++i)
			{
				if ((CountofFakeDolls > 0 && Random.Range(0, 1) > 0)|| FakeDollsGenPos.Length-i == CountofFakeDolls)
				{
					CountofFakeDolls--;
					continue;
				}
				
				networkGenerator.Generate(FakeDollPrefabs[Random.Range(0, FakeDollPrefabs.Length - 1)], FakeDollsGenPos[i].position, FakeDollsGenPos[i].rotation);
			}

		}
		void GeneratePlayerCharacter()
		{
			int clientIdx = DataManager.Instance.PlayerIdx;
			GameObject targetPrefab;

			//if (clientIdx == 0)
			if(DataManager.Instance.LocalPlayerData.roleData.Group == KSH_Lib.Data.RoleData.RoleGroup.Exorcist)
			{
				targetPrefab = ExorcistPrefabs[(int)DataManager.Instance.GetLocalRoleType];
				exorcistUI.gameObject.SetActive(true);
			}
			else
			{
				targetPrefab = DollPrefabs[(int)DataManager.Instance.GetLocalRoleType - 5];
				dollUI.gameObject.SetActive(true);
			}

			GameObject playerObj = networkGenerator.Generate(targetPrefab, PlayerGenPos[clientIdx].position, PlayerGenPos[clientIdx].rotation);
			Log.Instance.SetPlayer(playerObj.transform.GetChild(0).gameObject);

			LocalController = playerObj.transform.GetChild(0).gameObject.GetComponent<NetworkBaseController>();
		}

        public void OnPhotonSerializeView( PhotonStream stream, PhotonMessageInfo info )
        {
        }
    }
}