using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using Photon.Pun;
using Photon.Realtime;
using Photon.Chat;

using KSH_Lib.Data;
using KSH_Lib.Util;

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
					GameObject obj = new GameObject("_DataManager");
					instance = obj.AddComponent<DataManager>();
					pv = obj.AddComponent<PhotonView>();
					pv.ViewID = PhotonNetwork.AllocateViewID(1);

					pv.observableSearch = (PhotonView.ObservableSearch.AutoFindActive);
					pv.FindObservables();
				}
				return instance;
			}
		}
		static DataManager instance;


		/*--- Public Fields ---*/
		public List<RoleData> RoleInfos { get { return roleInfos; } }

		// This For Room Settings
		public RoleData.RoleType PreRoleType;
		public RoleData.RoleTypeOrder PreRoleTypeOrder;

		//public PlayerData LocalPlayerData { get { return LocalPlayerData; } }

		/*--- Private Fields ---*/
		const string CharcterStatusCSV = "Prototype/Main/Resources/Datas/CharacterStatus";

		static PhotonView pv;

		public int playerIdx;

		List<RoleData> roleInfos = new List<RoleData>();

		//public List<PlayerData> playerDatas = new List<PlayerData>();
		public PlayerData[] playerDatas;
		public PlayerData localPlayerData = new PlayerData();

		/*--- MonoBehaviour Callbacks ---*/
		private void Awake()
		{
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
			playerDatas = new PlayerData[GameManager.Instance.MaxPlayerCount];

		}

		/*--- Public Methods ---*/
		public void SetLocalAccount(int sheetIdx, in string id, in string nickname)
		{
			localPlayerData.accountData.Init( sheetIdx, id, nickname);
		}
		public void ResetLocalAccount()
		{
			localPlayerData.accountData = new AccountData();
		}
		public void SetPlayerIdx()
		{
			var players = PhotonNetwork.PlayerList;

			int myNum = PhotonNetwork.LocalPlayer.ActorNumber;
			for (int i = 0; i < players.Length; ++i)
			{
				if (myNum == players[i].ActorNumber)
				{
					playerIdx = i;
					break;
				}
			}
		}

		public void InitLocalRoleData()
        {
			localPlayerData.roleData = roleInfos[(int)PreRoleTypeOrder];
        }
		public void ResetLocalRoleData()
        {
			localPlayerData.roleData = new RoleData();
        }



		public void InitPlayerDatas()
        {
			//playerDatas = new List<PlayerData>( PhotonNetwork.CurrentRoom.PlayerCount );	
			for( int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i )
            {
				//playerDatas.Add( new PlayerData() );
            }
		}
		public void ResetPlayerDatas()
		{
			//	playerDatas = new List<PlayerData>();
		}

		public void ShareAllData()
        {
			ShareAccountData();
			ShareRoleData();
        }
		public void ShareAccountData()
		{
			pv.RPC( "ShareAccountDataRPC", RpcTarget.AllViaServer, playerIdx, AccountData.Serialize(localPlayerData.accountData) );
		}
		public void ShareRoleData()
        {
			if(localPlayerData.roleData is DollData)
            {
				pv.RPC( "ShareDollDataRPC", RpcTarget.AllViaServer, playerIdx, DollData.Serialize( localPlayerData.roleData ) );
            }
			else if(localPlayerData.roleData is ExorcistData)
			{
				pv.RPC( "ShareExorcistDataRPC", RpcTarget.AllViaServer, playerIdx, ExorcistData.Serialize( localPlayerData.roleData ) );
			}
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
				string name = data[i]["RoleName"].ToString();
				float moveSpeed = float.Parse(data[i]["MoveSpeed"].ToString());
				float interactionSpeed = float.Parse(data[i]["InteractionSpeed"].ToString());
				float projectileSpeed = float.Parse(data[i]["ProjectileSpeed"].ToString());

				if (type == "E")
				{
					float attackSpeed = float.Parse(data[i]["AttackSpeed"].ToString());
					float attackPower = float.Parse(data[i]["AttackPower"].ToString());
					roleInfos.Add(new ExorcistData( name, moveSpeed, interactionSpeed, projectileSpeed,  attackPower, attackSpeed));
				}
				else if (type == "D")
				{
					int dollHP = int.Parse(data[i]["DollHP"].ToString());
					int devilHP = int.Parse(data[i]["DevilHP"].ToString());
					roleInfos.Add(new DollData( name, moveSpeed, interactionSpeed, projectileSpeed, dollHP, devilHP));
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
			//playerDatas.RemoveAt( idx );
        }










		public void OnPhotonSerializeView( PhotonStream stream, PhotonMessageInfo info )
        {
        }
    }
}