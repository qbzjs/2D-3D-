using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using Photon.Pun;
using Photon.Realtime;
using Photon.Chat;

using KSH_Lib.Data;
using KSH_Lib.Util;


namespace KSH_Lib
{
	public class DataManager : MonoBehaviourPun, IPunObservable
	{
		/*--- Singleton ---*/
		public static DataManager Instance
        {
			get
            {
				if(instance == null)
                {
					GameObject obj = new GameObject( "_DataManager" );
					instance = obj.AddComponent<DataManager>();
					pv = obj.AddComponent<PhotonView>();
					pv.ViewID = PhotonNetwork.AllocateViewID(0);

					pv.observableSearch = (PhotonView.ObservableSearch.AutoFindActive);
					pv.FindObservables();
				}
				return instance;
			}
        }
		static DataManager instance;


		/*--- Public Fields ---*/
		public List<RoleData> RoleDatas { get { return roleDatas; } }
		public RoleData.RoleType CurRoleType;
		public RoleData.RoleTypeOrder CurRoleTypeOrder;
		
		//public PlayerData[] PlayerDatas { get { return playerDatas; } }

		/*--- Private Fields ---*/
		const string CharcterStatusCSV = "Prototype/Main/Resources/Datas/CharacterStatus";

		static PhotonView pv;

		int playerIdx;

		List<RoleData> roleDatas = new List<RoleData>();
		public AccountData curAccount;

		public PlayerData[] playerDatas;
		public PlayerData localPlayerData = new PlayerData();


        /*--- MonoBehaviour Callbacks ---*/
        private void Awake()
		{
			DontDestroyOnLoad( gameObject );

		}
        private void Start()
        {
			if ( !SetRoleDatasFromCSV( CharcterStatusCSV ) )
			{
				Debug.LogError( "DataManager.RoleDatas: Can't get role Datas from CSV" );
			}
			playerDatas = new PlayerData[GameManager.Instance.MaxPlayerCount];
		}

        /*--- Public Methods ---*/
        public void SetLocalAccount(int sheetIdx, in string id, in string nickname)
        {
			//curAccount = new AccountData( sheetIdx, id, nickname );
			localPlayerData.accountData = new AccountData( sheetIdx, id, nickname );
		}
		public void ResetLocalAccount()
        {
			//curAccount = new AccountData();
			localPlayerData.accountData = new AccountData();

		}
		public void SetPlayerIdx()
        {
			var players = PhotonNetwork.PlayerList;

			int myNum = PhotonNetwork.LocalPlayer.ActorNumber;
			for(int i = 0; i < players.Length; ++i )
            {
				if(myNum == players[i].ActorNumber)
                {
					playerIdx = i;
					break;
                }
            }
        }
		public void SetRoleType(in RoleData.RoleType type)
        {
			localPlayerData.roleData.Type = type;
        }
		public void SetRoleName(in string name)
        {
			localPlayerData.roleData.RoleName = name;
        }

		public void StartGame()
        {
			localPlayerData.roleData = roleDatas[(int)CurRoleTypeOrder];
			pv.RPC( "ChangePlayerData", RpcTarget.AllViaServer, localPlayerData );
        }

		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
		bool SetRoleDatasFromCSV( string csvPath )
		{
			List<Dictionary<string, object>> data = Util.CSVReader.Read( csvPath );
			if ( data == null )
			{
				return false;
			}

			for ( int i = 0; i < data.Count; ++i )
			{
				string type = data[i]["RoleType"].ToString();
				string name = data[i]["RoleName"].ToString();
				float moveSpeed = float.Parse( data[i]["MoveSpeed"].ToString() );
				float interactionSpeed = float.Parse( data[i]["InteractionSpeed"].ToString() );
				float projectileSpeed = float.Parse( data[i]["ProjectileSpeed"].ToString() );

				if ( type == "E" )
				{
					float attackSpeed = float.Parse( data[i]["AttackSpeed"].ToString() );
					float attackPower = float.Parse( data[i]["AttackPower"].ToString() );
					roleDatas.Add( new ExorcistData( moveSpeed, interactionSpeed, projectileSpeed, name, attackPower, attackSpeed ) );
				}
				else if ( type == "D" )
				{
					int dollHP = int.Parse( data[i]["DollHP"].ToString() );
					int devilHP = int.Parse( data[i]["DevilHP"].ToString() );
					roleDatas.Add( new DollData( moveSpeed, interactionSpeed, projectileSpeed, name, dollHP, devilHP ) );
				}
			}
			return true;
		}

		/*--- RPC ---*/

		[PunRPC]
		void InitPlayerData()
        {
			//if(photonView.IsMine)
            {
				localPlayerData.roleData = roleDatas[(int)CurRoleTypeOrder];
				playerDatas[playerIdx] = localPlayerData;
			}
        }
		[PunRPC]
		void ChangePlayerData( PlayerData data )
        {
			//if(photonView.IsMine)
            {
				//playerDatas[playerIdx] = localPlayerData;
				playerDatas[0] = data;
			}
        }



        public void OnPhotonSerializeView( PhotonStream stream, PhotonMessageInfo info )
        {
        }
    }
}