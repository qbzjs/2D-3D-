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
            InstantiateExorcist();
        }
        private bool InstantiateExorcist()
        {
            box = PhotonNetwork.Instantiate("Assets/Resources/Prototype/TID-57-implement-purification-box/Resource/Prefabs/PurificationBox (6)", new Vector3(Vector3(-0.043333333, 1.5, 0.123333342)), Quaternion.identity, 0);
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
