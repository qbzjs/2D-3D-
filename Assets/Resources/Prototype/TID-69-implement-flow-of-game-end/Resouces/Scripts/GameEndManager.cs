using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GHJ_Lib;
using KSH_Lib;
using Photon.Pun;

namespace LSH_Lib{
    public class GameEndManager : MonoBehaviour
    {
        public static GameEndManager Instance { get { return instance; } }
        public int DollCount
        {
            get { return dollCount; }
        }

        private static GameEndManager instance;   
        int dollCount;

        private void Start()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }
            
            dollCount = PhotonNetwork.CurrentRoom.PlayerCount;
        }
        public void DollCountDecrease()
        {
            if (dollCount > 0)
            {
                --dollCount;
            }
            else if(dollCount == 0)
            {
                DoGameEnd();
            }
        }
        private void DoGameEnd()
        {
            GameManager.Instance.LoadPhotonScene("99_GameResultScene");
        }


        public void EndGame(GameObject other)
        {
            if (!other.CompareTag("Doll"))
            {
                return;
            }

            if (other.GetComponent<PhotonView>().IsMine)
            {
                PhotonNetwork.LeaveRoom();
                DoGameEnd();
            }
            else
            {
                other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }

        }

        public void EndGame(Collider other)
        {
            EndGame(other.gameObject);

        }

    }
}
