using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

using Photon.Pun;
using Photon.Realtime;

using KSH_Lib.Data;
using KSH_Lib.Util;


namespace KSH_Lib
{
	public class DataManager : MonoBehaviourPun
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
				}
				return instance;
			}
        }
		static DataManager instance;


		/*--- Public Fields ---*/
		public RoleData.RoleType CurRoleType;
		public List<RoleData> RoleDatas { get { return roleDatas; } }
		public RoleData.RoleTypeOrder CurCharacterOrder;


		/*--- Private Fields ---*/
		const string CharcterStatusCSV = "Prototype/Main/Resources/Datas/CharacterStatus";

		int playerIdx;

		List<RoleData> roleDatas = new List<RoleData>();
		AccountData curAccount;

		PlayerData[] playerDatas;
		PlayerData localPlayerData;


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
        private void OnGUI()
        {
			//if ( photonView.IsMine )
			//{
			//	GUI.Box( new Rect( 0, 0, 200, 50 ), $"datas[0]: {playerDatas[0].roleData.RoleName}" );
			//	GUI.Box( new Rect( 0, 50, 200, 50 ), $"datas[0]: {playerDatas[1].roleData.RoleName}" );
			//	GUI.Box( new Rect( 0, 100, 200, 50 ), $"datas[0]: {playerDatas[2].roleData.RoleName}" );
			//	GUI.Box( new Rect( 0, 150, 200, 50 ), $"datas[0]: {playerDatas[3].roleData.RoleName}" );
			//	GUI.Box( new Rect( 0, 200, 200, 50 ), $"datas[0]: {playerDatas[4].roleData.RoleName}" );
			//	GUI.Box( new Rect( 210, 0, 200, 50 ), $"local: {localPlayerData.roleData.RoleName}" );
			//}
		}

        /*--- Public Methods ---*/
        public void SetLocalAccount(int sheetIdx, in string id, in string nickname)
        {
			curAccount = new AccountData( sheetIdx, id, nickname );
        }
		public void ResetLocalAccount()
        {
			curAccount = new AccountData();
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
		public void StartGame()
        {
			ChangePlayerData();
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

		[PunRPC]
		void ChangePlayerData()
        {
			if(photonView.IsMine)
            {
				playerDatas[playerIdx] = localPlayerData;
			}
        }
	}
}