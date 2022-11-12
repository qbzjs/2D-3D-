using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using KSH_Lib.Data;
using LSH_Lib;
using MSLIMA.Serializer;

namespace KSH_Lib
{
	public class DataManager : MonoBehaviourPunCallbacks, IPunObservable
	{
		/*--- Singleton ---*/
		public static DataManager Instance
		{
			get
			{
				if (instance == null)
				{
					//GameObject obj = new GameObject("_DataManager");
					//instance = obj.AddComponent<DataManager>();
					//pv = obj.AddComponent<PhotonView>();
					//pv.ViewID = PhotonNetwork.AllocateViewID(0);

					//pv.observableSearch = (PhotonView.ObservableSearch.AutoFindActive);
					//pv.FindObservables();
					Debug.LogError( "No DataManager" );
				}
				return instance;
			}
		}
		static DataManager instance;
		//static PhotonView pv;


		/*--- Fields ---*/
		const string CharcterStatusCSV = "Prototype/Main/Resources/Datas/CharacterStatus";
		const string ItemDataCSV = "Prototype/Main/Resources/Datas/ItemData";

		// Role Informations
		public List<RoleData> RoleInfos { get { return roleInfos; } }
		List<RoleData> roleInfos = new List<RoleData>();

		// Item Datas
		public List<Item.ItemData> ItemInfos { get { return itemInfos; } }
		List<Item.ItemData> itemInfos = new List<Item.ItemData>();




		// This For Room Settings
		public RoleData.RoleType PreRoleType = RoleData.RoleType.Null;
		public RoleData.RoleTypeOrder PreRoleTypeOrder = RoleData.RoleTypeOrder.Null;

		public List<PlayerData> PlayerDatas
		{
			get
			{
				if(playerDatas == null)
                {
					Debug.LogError("DataManager: playerData is not set");
                }
				return playerDatas;
			}
		}
		List<PlayerData> playerDatas = new List<PlayerData>();

		public PlayerData LocalPlayerData = new PlayerData();
		public int PlayerIdx { get; private set; }
		public bool IsInited
        {
			get
            {
				bool flag = true;
				foreach(bool isInited in isInitedList)
                {
					flag &= isInited;
				}
				return flag;
            }
		}
		List<bool> isInitedList = new List<bool>();





		/*--- MonoBehaviour Callbacks ---*/
		private void Awake()
		{
			instance = this;
			DontDestroyOnLoad(gameObject);

			Serializer.RegisterCustomType<AccountData>( (byte)'A' );
			Serializer.RegisterCustomType<DollData>( (byte)'D' );
			Serializer.RegisterCustomType<ExorcistData>( (byte)'E' );
		}
		private void Start()
		{
			if (!SetRoleDatasFromCSV(CharcterStatusCSV))
			{
				Debug.LogError("DataManager.RoleDatas: Can't get role Datas from CSV");
			}
			if(!SetItemDatasFromCSV(ItemDataCSV))
            {
				Debug.LogError("DataManager.ItemDataInfos: Can't get item Datas from csv");
            }
			ResetLocalRoleData();
		}

		/*--- Public Methods ---*/
		public void SetLocalAccount(int sheetIdx, in string id, in string nickname)
		{
			LocalPlayerData.accountData.Init( sheetIdx, id, nickname);
		}
		public void ResetLocalAccount()
		{
			LocalPlayerData.accountData = new AccountData();
		}
		public void SetPlayerIdx()
		{
			var players = PhotonNetwork.PlayerList;

			int myNum = PhotonNetwork.LocalPlayer.ActorNumber;
			for (int i = 0; i < players.Length; ++i)
			{
				if (myNum == players[i].ActorNumber)
				{
					PlayerIdx = i;
					break;
				}
			}
		}

		public void InitLocalRoleData()
        {
			LocalPlayerData.roleData = roleInfos[(int)PreRoleTypeOrder].Clone();
        }
		public void ResetLocalRoleData()
        {
			PreRoleType = RoleData.RoleType.Null;
			PreRoleTypeOrder = RoleData.RoleTypeOrder.Null;
			LocalPlayerData.roleData = new RoleData();
			LocalPlayerData.roleData.Type = RoleData.RoleType.Null;
			LocalPlayerData.roleData.TypeOrder = RoleData.RoleTypeOrder.Null;
        }

		public void InitPlayerDatas()
        {
			if(playerDatas.Count == 0)
			{
				for ( int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i )
				{
					playerDatas.Add( new PlayerData() );
					isInitedList.Add( false );
				}
			}
		}
		public void ResetPlayerDatas()
		{
			playerDatas = new List<PlayerData>();
		}

		public void ShareAllData()
		{
			ShareRoleData();
			ShareAccountData();
			photonView.RPC("ShareInitedRPC", RpcTarget.AllViaServer, PlayerIdx, true);
        }
		public void ShareAccountData()
		{
			photonView.RPC( "ShareAccountDataRPC", RpcTarget.AllViaServer, PlayerIdx, AccountData.Serialize(LocalPlayerData.accountData) );
		}
		public void ShareRoleData()
        {
			if(LocalPlayerData.roleData is DollData)
            {
				photonView.RPC( "ShareDollDataRPC", RpcTarget.AllViaServer, PlayerIdx, DollData.Serialize( LocalPlayerData.roleData ) );
            }
			else if(LocalPlayerData.roleData is ExorcistData)
			{
				photonView.RPC( "ShareExorcistDataRPC", RpcTarget.AllViaServer, PlayerIdx, ExorcistData.Serialize( LocalPlayerData.roleData ) );
			}
			else
            {
				Debug.LogError( "DataManger.ShareRoleData: localPlayerData.roleData type error" );
            }
        }
		public void DisconnectAllData()
		{
			photonView.RPC( "DisconnectAllDataRPC", RpcTarget.AllViaServer, PlayerIdx );
		}

		/*--- Private Methods ---*/
		bool SetRoleDatasFromCSV(string csvPath)
		{
			List<Dictionary<string, object>> data = Util.CSVReader.Read(csvPath);
			if (data == null)
			{
				return false;
			}

			for (int i = 0; i < data.Count; ++i)
			{
				string type = data[i]["RoleType"].ToString();
				//string name = data[i]["RoleName"].ToString();
				float moveSpeed = float.Parse(data[i]["MoveSpeed"].ToString());
				float interactionSpeed = float.Parse(data[i]["InteractionSpeed"].ToString());
				float projectileSpeed = float.Parse(data[i]["ProjectileSpeed"].ToString());

				if (type == "E")
				{
					float attackSpeed = float.Parse(data[i]["AttackSpeed"].ToString());
					float attackPower = float.Parse(data[i]["AttackPower"].ToString());
					roleInfos.Add(new ExorcistData( (Data.RoleData.RoleTypeOrder)i, moveSpeed, interactionSpeed, projectileSpeed,  attackPower, attackSpeed));
				}
				else if (type == "D")
				{
					int dollHP = int.Parse(data[i]["DollHP"].ToString());
					int devilHP = int.Parse(data[i]["DevilHP"].ToString());
					roleInfos.Add(new DollData( (Data.RoleData.RoleTypeOrder)i, moveSpeed, interactionSpeed, projectileSpeed, dollHP, devilHP));
				}
			}
			return true;
		}
		bool SetItemDatasFromCSV(string csvPath)
		{
			List<Dictionary<string, object>> data = Util.CSVReader.Read(csvPath);
			if (data == null)
			{
				return false;
			}

			for (var i = 0; i < data.Count; i++)
			{
				string type = data[i]["Type"].ToString();
				string name = data[i]["Name"].ToString();
				string number = data[i]["Number"].ToString();
				string isUsing = data[i]["isUsing"].ToString();
				int frequency = int.Parse(data[i]["Frequency"].ToString());

				if (type == "D")
				{
					itemInfos.Add(new Item.ItemData(type, name, number, isUsing, frequency));
				}
				else if(type == "E")
				{
					itemInfos.Add(new Item.ItemData(type, name, number, isUsing, frequency));
				}
			}

			return true;
		}


		/*--- RPC ---*/
		[PunRPC]
		void ShareAccountDataRPC(int idx, byte[] data)
        {
			playerDatas[idx].accountData = (AccountData)AccountData.Deserialize(data);
        }

		[PunRPC]
		void ShareDollDataRPC(int idx, byte[] data)
		{
			playerDatas[idx].roleData = (DollData)DollData.Deserialize( data );
		}

		[PunRPC]
		void ShareExorcistDataRPC( int idx, byte[] data )
		{
			playerDatas[idx].roleData = (ExorcistData)ExorcistData.Deserialize( data );
		}

		[PunRPC]
		void DisconnectAllDataRPC(int idx)
        {
			playerDatas.RemoveAt( idx );
		}

		[PunRPC]
		void ShareInitedRPC(int idx, bool isInited)
        {
			isInitedList[idx] = isInited;
        }


		/***************************************************
		 * 
		 *  Data Manager 사용 예시다
		 * 
		 * *************************************************
		 * 
		 * Write: LocalData를 업데이트 한 후 playerDatas에서 자신의 index를 공유하는 것
		 * Read: 자신의 Index의 playerDatas를 보면 됨
		 * 
		 * 
		 * Read 예시
		 * var dollHp = (DataManager.Instance.LocalPlayerData.roleData as DollData).DollHP;
		 * var exorcistAttackPower = (DataManager.Instance.LocalPlayerData.roleData as ExorcistData).AttackPower;
		 * var myMoveSpeed = (DataManager.Instance.LocalPlayerData.roleData).MoveSpeed;
		 * 
		 * DataManager.Instance.LocalPlayerData.roleData.MoveSpeed += 3;
		 * (DataManager.Instance.LocalPlayerData.roleData as DollData).DollHP += 200;
		 *	...
		 *	...
		 *	많은 데이터 수정 후 마지막으로
		 *	DataManager.Instance.ShareRoleData();
		 *	
		 *	*****************************************/
		






		public void OnPhotonSerializeView( PhotonStream stream, PhotonMessageInfo info )
        {
        }
    }
}