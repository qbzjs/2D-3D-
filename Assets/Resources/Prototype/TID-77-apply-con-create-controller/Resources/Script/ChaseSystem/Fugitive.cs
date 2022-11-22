using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
namespace GHJ_Lib
{
    public class Fugitive : MonoBehaviourPun
    {
        public bool IsChased { get; private set; }
        public bool IsWatched { get; set; }
        [SerializeField] protected float ChaseGauge = 0.0f;
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
        public void SetWatch(bool IsWatchTarget)
        {
            if (!dollController.IsMine)
            {
                return;
            }
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
                    ChaseGauge += 5*Time.deltaTime;
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

            if (ChaseGauge >= 50.0f)
            {
                IsChased = true;
            }
            else
            {
                IsChased = false;
            }

            if (dollController.IsMine)
            { 
                Log.Instance.WriteLog("IsChased" + IsChased.ToString(), 2);
                Log.Instance.WriteLog("IsWatched" + IsWatched.ToString(), 3);
                Log.Instance.WriteLog("ChaseGauge" + ChaseGauge.ToString(), 4);
            }
        }
    }

}


