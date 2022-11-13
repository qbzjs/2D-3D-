using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
namespace GHJ_Lib
{
    public class Chaser : MonoBehaviour
    {
        Fugitive[] Fugitives = new Fugitive[4];
        int[] IDs = new int[4];
        Camera mainCamera = Camera.main;
        [SerializeField] private PhotonView photonView;
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
                            fugitive.CanChase(true);
                        }
                    }
                    else
                    {
                        if (fugitive.IsWatched)
                        {
                            fugitive.CanChase(false);
                        }
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
       
    }
}

