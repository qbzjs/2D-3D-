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
        [SerializeField] List<Fugitive> Fugitives = new List<Fugitive>();
        [SerializeField] SphereCollider sphereCollider;
        Camera mainCamera;
        [SerializeField] float CoolDowntime;
        [SerializeField] private PhotonView photonView;
        
        private void OnEnable()
        {
            mainCamera = Camera.main;
            sphereCollider = GetComponent<SphereCollider>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(GameManager.DollTag))
            {
                Fugitive fugitive = other.GetComponent<Fugitive>();
                if (Fugitives.Contains(fugitive))
                {
                    return;
                }
                Fugitives.Add(fugitive);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(GameManager.DollTag))
            {
                Fugitive fugitive = other.GetComponent<Fugitive>();
                if (Fugitives.Contains(fugitive))
                {
                    fugitive.CanWatch(false);
                    Fugitives.Remove(fugitive);
                }
                
            }
        }
        private void Update()
        {
            if (photonView.IsMine)
            {
                if (Fugitives.Count == 0)
                {
                    return;
                }

                foreach (Fugitive fugitive in Fugitives)
                {
                    Transform objTransform = fugitive.transform;
                    if (IsInCameraView(objTransform) && 
                        CheckObstacle(objTransform))
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

                
                Log.Instance.WriteLog(chaseState.ToString(),0);
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

        protected bool CheckObstacle(Transform targetTransform)
        {
            RaycastHit[] Hits;
            Vector3 CamPos = mainCamera.transform.position;
            Ray ray = new Ray(CamPos, targetTransform.position - CamPos);

            Hits = Physics.RaycastAll(ray, sphereCollider.radius);

            foreach (RaycastHit hit in Hits)
            {
                Debug.Log($"Hits : {hit.collider.name}");
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer(GameManager.EnvironmentLayer))
                {
                    return false;
                }
            }
            return true;
        }
        protected bool CheckFugitivesIsChasedOnView()
        {
            foreach (Fugitive fugitive in Fugitives)
            {
                if (!fugitive)
                {
                    break;
                }
                if (fugitive.IsChased&&fugitive.IsWatched)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

