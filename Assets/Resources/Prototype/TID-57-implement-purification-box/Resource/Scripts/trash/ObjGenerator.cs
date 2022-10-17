using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace LSH_Lib{
    public class ObjGenerator : MonoBehaviour
    {
        private GameObject box;
        private void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                InstantiateBox();
            }
            else
            {
                Debug.LogWarning("I'm not master client");
            }
        }
        private bool InstantiateBox()
        {
            box = PhotonNetwork.Instantiate("Prototype/TID-57-implement-purification-box/Resource/Prefabs/PurificationBox", new Vector3(0,1,0), Quaternion.identity, 0);
            if (box)
            {
                return true;
            }
            else
            {
                Debug.LogError("can't instantiate");
                return false;
            }
        }
    }
}
