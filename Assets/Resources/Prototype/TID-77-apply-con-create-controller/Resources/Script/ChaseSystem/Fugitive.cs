using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
namespace GHJ_Lib
{
    public class Fugitive : MonoBehaviourPun
    {
        [SerializeField] public bool IsChased { get; private set; }
        [SerializeField] public bool IsWatched { get; set; }
        [SerializeField] protected float ChaseGauge = 0.0f;
        public void CanWatch(bool IsWatchTarget)
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
            if (IsWatched)
            {
                if (ChaseGauge < 150.0f)
                {
                    ChaseGauge+= Time.deltaTime;
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
                    ChaseGauge -= Time.deltaTime;
                }
                else
                {
                    ChaseGauge = 0.0f;
                }
            }

            if (ChaseGauge >= 50.0f)
            {
                IsChased = true;
            }
            else
            {
                IsChased = false;
            }
            Log.Instance.WriteLog("IsChased" + IsChased.ToString(),1);
            Log.Instance.WriteLog("IsWatched" + IsWatched.ToString(),2);
        }
    }

}

