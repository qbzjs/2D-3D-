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
            if (other.gameObject.CompareTag(GameManager.DollTag))
            {
                Fugitive fugitive = other.GetComponent<Fugitive>();
                if (fugitive ==null|| Fugitives.Contains(fugitive))
                {
                    return;
                }
                Fugitives.Add(fugitive);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(GameManager.DollTag))
            {
                Fugitive fugitive = other.GetComponent<Fugitive>();
                
                if (fugitive==null|| Fugitives.Contains(fugitive))
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
                    if (IsInCameraView(fugitive.gameObject) &&
                        CheckObstacle(fugitive.gameObject))
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


                Log.Instance.WriteLog(chaseState.ToString(), 0);
                Log.Instance.WriteLog(CoolDowntime.ToString(), 1);
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

        protected bool IsInCameraView(GameObject targetObject)
        {
            Vector3 targetViewPort = mainCamera.WorldToViewportPoint(targetObject.transform.position);

            return (targetViewPort.x <= 1.0f &&
                    targetViewPort.x >= 0.0f &&
                    targetViewPort.y <= 1.0f &&
                    targetViewPort.y >= 0.0f &&
                    targetViewPort.z > 0.0f);
        }

        protected bool CheckObstacle(GameObject targetObject)
        {
            RaycastHit[] Hits;
            Vector3 CamPos = mainCamera.transform.position;
            Ray ray = new Ray(CamPos, targetObject.transform.position - CamPos);

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

        private void OnDrawGizmos()
        {
            if (Fugitives.Count == 0)
            {
                return;
            }
            RaycastHit[] Hits;
            Vector3 CamPos = mainCamera.transform.position;

            foreach (Fugitive fugitive in Fugitives)
            {
                Debug.Log(fugitive);
                Debug.Log(fugitive.gameObject.name);
                Ray ray = new Ray(CamPos, fugitive.transform.position - CamPos);
                Hits = Physics.RaycastAll(ray, sphereCollider.radius);

                Gizmos.color = Color.red;
                Gizmos.DrawRay(ray);

                Gizmos.color = Color.cyan;
                if (Hits.Length > 0)
                {
                    foreach (RaycastHit hit in Hits)
                    {
                        Gizmos.DrawSphere(hit.transform.position, 1.0f);
                    }
                }
            }

        }
    }
}

