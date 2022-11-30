using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using KSH_Lib;
using LSH_Lib;

namespace GHJ_Lib
{
    public class Fugitive : MonoBehaviourPun
    {
        public bool IsChased { get; private set; }
        [field: SerializeField] public bool IsWatched { get; set; }
        [SerializeField] protected float ChaseGauge = 0.0f;
        public AudioPlayer AudioPlayer;
        public int GetStack
        {
            get {
                if (dollController == null)
                {
                    dollController = GetComponent<DollController>();
                }
                return dollController.CrossStack;
            }
        }
        [SerializeField] protected DollController dollController;
        public Behavior<NetworkBaseController> curBehaviour
        {
            get { return dollController.CurBehavior; }
        }
        public void SetWatch(bool IsWatchTarget)
        {
            if (IsWatchTarget)
            {
                photonView.RPC("StartWatched", RpcTarget.All);
            }
            else
            {
                photonView.RPC("EndWatched", RpcTarget.All);
            }
        }
        [PunRPC]
        public void StartWatched()
        {
            IsWatched = true;
        }
        [PunRPC]
        public void EndWatched()
        {
            IsWatched = false;
        }
        void Update()
        {
            if (DataManager.Instance.PlayerIdx == 0)
            {
                if (IsWatched)
                {
                    if (ChaseGauge < 150.0f)
                    {
                        ChaseGauge += 5 * Time.deltaTime;
                    }
                    else
                    {
                        ChaseGauge = 150.0f;
                    }
                }
                else
                {
                    if (ChaseGauge > 0.0f)
                    {
                        ChaseGauge -= 5 * Time.deltaTime;
                    }
                    else
                    {
                        ChaseGauge = 0.0f;
                    }
                }
                photonView.RPC("SyncChaseGauge", RpcTarget.All, ChaseGauge);
            }

            if (ChaseGauge >= 50.0f)
            {
                if (!IsChased)
                { 
                    IsChased = true;
                    if (photonView.IsMine)
                    { 
                        AudioPlayer.Play("ChasingBGM"); //<<: ChaseSound 
                    }
                }
            }
            else
            {
                if (IsChased)
                { 
                    IsChased = false;
                    if (photonView.IsMine)
                    {
                        AudioPlayer.Stop("ChasingBGM"); //<<:ChaseSound
                    }
                }
            }

            if (dollController.IsMine)
            { 
                Log.Instance.WriteLog("IsChased" + IsChased.ToString(), 2);
                Log.Instance.WriteLog("IsWatched" + IsWatched.ToString(), 3);
                Log.Instance.WriteLog("ChaseGauge" + ChaseGauge.ToString(), 4);
            }
        }

        [PunRPC]
        public void SyncChaseGauge(float curGauge)
        {
            ChaseGauge = curGauge;
        }

    }

}


