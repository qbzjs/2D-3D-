using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GHJ_Lib;

using KSH_Lib.Data;

namespace KSH_Lib
{
	public class StageManager : MonoBehaviour
	{
        /*--- Public Fields ---*/
        public bool isOnDebugGUI;
		NetworkGenerator networkGenerator;

        /*--- Protected Fields ---*/


        /*--- Private Fields ---*/

        /*--- MonoBehaviour Callbacks ---*/
        private void Awake()
        {
            DataManager.Instance.InitPlayerDatas();

        }
        void Start()
        {
            DataManager.Instance.ShareAllData();
		}

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha0))
            {
                isOnDebugGUI = !isOnDebugGUI;
            }
			if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                //DataManager.Instance.localPlayerData.roleData.MoveSpeed += 1;
                DataManager.Instance.ShareRoleData();
            }
            if ( Input.GetKeyDown( KeyCode.Alpha2 ) )
            {
                //DataManager.Instance.localPlayerData.accountData.SheetIdx++;
                DataManager.Instance.ShareAccountData();
            }
        }

 

        private void OnGUI()
        {
            //if ( isOnDebugGUI )
            //{
            //    GUI.Box( new Rect( 0, 160, 150, 30 ), $"Local MoveSpeed: {DataManager.Instance.localPlayerData.roleData.MoveSpeed}" );
            //    GUI.Box( new Rect( 160, 160, 150, 30 ), $"Local sheetIdx: {DataManager.Instance.localPlayerData.accountData.SheetIdx}" );

            //    for (int i = 0; i < DataManager.Instance.playerDatas.Count; ++i )
            //    {
            //        GUI.Box( new Rect( 0, i * 30, 150, 30 ), $"MoveSpeed[{i}]: {DataManager.Instance.playerDatas[i].roleData.MoveSpeed}" );
            //        GUI.Box( new Rect( 160, i * 30, 150, 30 ), $"sheetIdx[{i}]: {DataManager.Instance.playerDatas[i].accountData.SheetIdx}" );
            //    }
            //}
        }


        /*--- Public Methods ---*/
        public void OnExitGame()
        {
            DataManager.Instance.DisconnectAllData();
            DataManager.Instance.ResetPlayerDatas();
            DataManager.Instance.ResetLocalRoleData();
        }

        /*--- Protected Methods ---*/


        /*--- Private Methods ---*/
    }
}