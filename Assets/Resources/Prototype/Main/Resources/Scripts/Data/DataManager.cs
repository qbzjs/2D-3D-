using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using KSH_Lib.Data;


namespace KSH_Lib
{
	public class DataManager : MonoBehaviourPun
	{
		/*--- Singleton ---*/
		public DataManager Instance
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
		DataManager instance;


		/*--- Public Fields ---*/


		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/
		List<PlayerData> playerDatas = new List<PlayerData>();
		PlayerData localPlayerData;

		RoleData curRoleData;
		

		/*--- MonoBehaviour Callbacks ---*/
		private void Start()
        {
			DontDestroyOnLoad( gameObject );
        }

        /*--- Public Methods ---*/
		public void OnVerifyAccount(int sheetIdx, in string id, in string nickname)
        {
			localPlayerData.accountData = new AccountData( sheetIdx, id, nickname );
        }
		public void AssignAccount(in PlayerData playerData)
        {
			playerData.Index = playerDatas.Count;
			playerDatas.Add( playerData );
        }

		public void SetRoleDataToLocalPlayer()
        {

        }

        /*--- Protected Methods ---*/


        /*--- Private Methods ---*/
    }
}