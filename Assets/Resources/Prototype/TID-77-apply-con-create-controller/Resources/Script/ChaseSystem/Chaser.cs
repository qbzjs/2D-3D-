using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
namespace GHJ_Lib
{
    public class Chaser : MonoBehaviour
    {
        public enum ChaseState { Wait,CoolDown,Chasing}
        public ChaseState chaseState = ChaseState.Wait;
        Fugitive[] Fugitives = new Fugitive[4];
        int[] IDs = new int[4];
        Camera mainCamera;
        float CoolDowntime;
        [SerializeField] private PhotonView photonView;
        
        private void OnEnable()
        {
            mainCamera = Camera.main;

        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(GameManager.DollTag))
            {
                int newID = other.GetInstanceID();
                int index = 0;
                foreach (int ID in IDs)
                {
                    if (ID == newID)
                    {
                        return;
                    }
                    index++;
                }
                IDs[index] = newID;
                Fugitives[index] = other.GetComponent<Fugitive>();
            }
        }

        private void Update()
        {
            if (photonView.IsMine)
            { 
                foreach (Fugitive fugitive in Fugitives)
                {
                    if (IsInCameraView(fugitive.transform))
                    {
                        if (!fugitive.IsWatched)
                        {
                            fugitive.CanWatch(true);
                        }
                    }
                    else
                    {
                        if (fugitive.IsWatched)
                        {
                            fugitive.CanWatch(false);
                        }
                    }
                }

                if (CheckFugitivesIsChasedOnView())
                {
                    chaseState = ChaseState.Chasing;
                }
                else
                {
                    switch (chaseState)
                    {
                        case ChaseState.Chasing:
                            {
                                CoolDowntime = 2.0f;
                                chaseState = ChaseState.CoolDown;
                            }
                            break;
                        case ChaseState.CoolDown:
                            {
                                CoolDowntime -= Time.deltaTime;
                                if (CoolDowntime <= 0.0f)
                                {
                                    CoolDowntime = 0.0f;
                                    chaseState = ChaseState.Wait;
                                }
                            }
                            break;
                        case ChaseState.Wait:
                            {

                            }
                            break;
                    }
                }
            }
        }

        protected bool IsInCameraView(Transform targetTransform)
        {
            Vector3 targetViewPort = mainCamera.WorldToViewportPoint(targetTransform.position);

            return (targetViewPort.x <= 1.0f &&
                    targetViewPort.x >= 0.0f &&
                    targetViewPort.y <= 1.0f &&
                    targetViewPort.y >= 0.0f &&
                    targetViewPort.z > 0.0f);
        }

        protected bool CheckFugitivesIsChasedOnView()
        {
            foreach (Fugitive fugitive in Fugitives)
            {
                if (fugitive.IsChased&&fugitive.IsWatched)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

