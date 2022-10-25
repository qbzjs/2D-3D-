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
        }
        void Start()
		{
			DataManager.Instance.StartGame();
		}

        private void Update()
        {
			if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                DataManager.Instance.localPlayerData.roleData.MoveSpeed += 1;
                DataManager.Instance.localint += 1;
                DataManager.Instance.UpdateData();
            }
        }

        private void OnGUI()
        {
            if(DataManager.Instance.localPlayerData != null)
            {
                //GUI.Box( new Rect( 200, 0, 150, 30 ), $"0.MoveSpeed = {DataManager.Instance.localPlayerData.roleData.MoveSpeed}" );
            }
            //GUI.Box( new Rect( 0, 0, 150, 30 ), $"0.MoveSpeed = {DataManager.Instance.playerDatas[0].roleData.MoveSpeed}" );
            //GUI.Box( new Rect( 0, 30, 150, 30 ), $"1.MoveSpeed = {DataManager.Instance.playerDatas[1].roleData.MoveSpeed}" );
            //GUI.Box( new Rect( 0, 60, 150, 30 ), $"2.MoveSpeed = {DataManager.Instance.playerDatas[2].roleData.MoveSpeed}" );
            //GUI.Box( new Rect( 0, 90, 150, 30 ), $"3.MoveSpeed = {DataManager.Instance.playerDatas[3].roleData.MoveSpeed}" );
            //GUI.Box( new Rect( 0, 120, 150, 30 ), $"4.MoveSpeed = {DataManager.Instance.playerDatas[4].roleData.MoveSpeed}" );
        }


        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/


        /*--- Private Methods ---*/
    }
}