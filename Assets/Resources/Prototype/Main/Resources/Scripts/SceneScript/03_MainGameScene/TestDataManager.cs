using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using KSH_Lib;

namespace KSH_Lib.Test
{
	public class TestDataManager : MonoBehaviourPunCallbacks, IPunObservable
	{
		/*--- Public Fields ---*/


		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/
		public int[] datas;
		public int localData;
		public int index;

		PhotonView pv;
		/*--- MonoBehaviour Callbacks ---*/
		void Start()
		{
			datas = new int[5];
			pv = GetComponent<PhotonView>();
			index = DataManager.Instance.playerIdx;
			localData = index;
		}

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
				localData++;
				pv.RPC("AddData", RpcTarget.All, localData, index);
            }
        }

        private void OnGUI()
        {
			GUI.Box(new Rect(0, 0, 150, 30), $"index: {index.ToString()}");
			GUI.Box(new Rect(0, 30, 150, 30), $"localData: {localData.ToString()}");
		}

        /*--- Public Methods ---*/
        [PunRPC]
		void AddData(int data, int idx)
		{
			datas[idx] = data;
		}

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        }

        /*--- Protected Methods ---*/


        /*--- Private Methods ---*/
    }
}