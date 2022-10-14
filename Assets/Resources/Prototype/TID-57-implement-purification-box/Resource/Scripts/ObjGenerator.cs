using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace LSH_Lib{
    public class ObjGenerator : MonoBehaviour
    {
        private GameObject box;
        //>>LSH
        [SerializeField]
        private GameObject BoxUI;
        //<<LSH
        private void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                InstantiateExorcist();
            }
            if (BoxUI != null)
            {
                GameObject boxUI = Instantiate(BoxUI);
                boxUI.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            }
            else
            {
                Debug.LogWarning("Is it right?");
            }
        }
        private bool InstantiateExorcist()
        {
            box = PhotonNetwork.Instantiate("Prototype/TID-57-implement-purification-box/Resource/Prefabs/PurificationBox (6)", new Vector3(-0.043333333f, 1.5f, 0.123333342f), Quaternion.identity, 0);
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
