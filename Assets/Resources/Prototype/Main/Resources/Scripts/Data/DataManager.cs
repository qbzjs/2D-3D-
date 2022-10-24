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

		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/
		List<PlayerData> playerDatas = new List<PlayerData>();

		RoleData curRoleData;
		AccountData curAccount;


        /*--- MonoBehaviour Callbacks ---*/
        private void Awake()
		{
			DontDestroyOnLoad( gameObject );
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
		public void CreateRoleData(RoleData.RoleType roleType)
        {
			
        }


        /*--- Protected Methods ---*/


        /*--- Private Methods ---*/
    }
}