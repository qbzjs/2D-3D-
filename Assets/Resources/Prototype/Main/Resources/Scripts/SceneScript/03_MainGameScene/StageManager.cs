using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GHJ_Lib;

namespace KSH_Lib
{
	public class StageManager : MonoBehaviour
	{
		/*--- Public Fields ---*/
		NetworkGenerator networkGenerator;

        /*--- Protected Fields ---*/


        /*--- Private Fields ---*/

        /*--- MonoBehaviour Callbacks ---*/
        private void Awake()
        {
            //DataManager.Instance.InitPlayerDatas();

        }
        void Start()
        {
            DataManager.Instance.ShareAllData();
		}

        private void Update()
        {
			if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                DataManager.Instance.localPlayerData.roleData.MoveSpeed += 1;
                DataManager.Instance.ShareRoleData();
            }
            if ( Input.GetKeyDown( KeyCode.Alpha2 ) )
            {
                DataManager.Instance.localPlayerData.accountData.SheetIdx++;
                DataManager.Instance.ShareAccountData();
            }
        }

 

        private void OnGUI()
        {
            //if ( DataManager.Instance.localPlayerData != null )
            //{
            //    GUI.Box( new Rect( 200, 0, 150, 30 ), $"0.localData = {DataManager.Instance.localTestDatas}" );
            //}
            //GUI.Box( new Rect( 0, 0, 150, 30 ), $"0.TestDatas = {DataManager.Instance.testDatas[0]}" );
            //GUI.Box( new Rect( 0, 30, 150, 30 ), $"1.TestDatas = {DataManager.Instance.testDatas[1]}" );
            //GUI.Box( new Rect( 0, 60, 150, 30 ), $"2.TestDatas = {DataManager.Instance.testDatas[2]}" );
            //GUI.Box( new Rect( 0, 90, 150, 30 ), $"3.TestDatas = {DataManager.Instance.testDatas[3]}" );
            //GUI.Box( new Rect( 0, 120, 150, 30 ), $"4.TestDatas = {DataManager.Instance.testDatas[4]}" );

            if ( DataManager.Instance.localPlayerData != null )
            {
                GUI.Box( new Rect( 0, 160, 150, 30 ), $"0.MoveSpeed = {DataManager.Instance.localPlayerData.roleData.MoveSpeed}" );
            }
            GUI.Box( new Rect( 0, 0, 150, 30 ), $"0.MoveSpeed = {DataManager.Instance.playerDatas[0].roleData.MoveSpeed}" );
            GUI.Box( new Rect( 0, 30, 150, 30 ), $"1.MoveSpeed = {DataManager.Instance.playerDatas[1].roleData.MoveSpeed}" );
            GUI.Box( new Rect( 0, 60, 150, 30 ), $"2.MoveSpeed = {DataManager.Instance.playerDatas[2].roleData.MoveSpeed}" );
            GUI.Box( new Rect( 0, 90, 150, 30 ), $"3.MoveSpeed = {DataManager.Instance.playerDatas[3].roleData.MoveSpeed}" );
            GUI.Box( new Rect( 0, 120, 150, 30 ), $"4.MoveSpeed = {DataManager.Instance.playerDatas[4].roleData.MoveSpeed}" );

            if ( DataManager.Instance.localPlayerData != null )
            {
                GUI.Box( new Rect( 160, 160, 150, 30 ), $"0.sheetIdx = {DataManager.Instance.localPlayerData.accountData.SheetIdx}" );
            }
            GUI.Box( new Rect( 160, 0, 150, 30 ), $"0.sheetIdx = {DataManager.Instance.playerDatas[0].accountData.SheetIdx}" );
            GUI.Box( new Rect( 160, 30, 150, 30 ), $"1.sheetIdx = {DataManager.Instance.playerDatas[1].accountData.SheetIdx}" );
            GUI.Box( new Rect( 160, 60, 150, 30 ), $"2.sheetIdx = {DataManager.Instance.playerDatas[2].accountData.SheetIdx}" );
            GUI.Box( new Rect( 160, 90, 150, 30 ), $"3.sheetIdx = {DataManager.Instance.playerDatas[3].accountData.SheetIdx}" );
            GUI.Box( new Rect( 160, 120, 150, 30 ), $"4.sheetIdx = {DataManager.Instance.playerDatas[4].accountData.SheetIdx}" );
        }


        /*--- Public Methods ---*/
        public void OnExitGame()
        {
            //DataManager.Instance.DisconnectAllData();
            //DataManager.Instance.ResetPlayerDatas();
            //DataManager.Instance.ResetLocalRoleData();
        }

        /*--- Protected Methods ---*/


        /*--- Private Methods ---*/
    }
}