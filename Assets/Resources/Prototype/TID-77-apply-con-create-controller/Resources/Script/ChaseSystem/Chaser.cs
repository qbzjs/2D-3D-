using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using KSH_Lib;
using KSH_Lib.Data;
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
        [SerializeField] private float CrossStackSpeedUpRate = 0.2f;
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
                    if(fugitive == null)
                    {
                        continue;
                    }
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
                    if (chaseState != ChaseState.Chasing)
                    {
                        chaseState = ChaseState.Chasing;
                    }
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
                                DataManager.Instance.LocalPlayerData.roleData.MoveSpeed = DataManager.Instance.RoleInfos[(int)DataManager.Instance.GetLocalRoleType].MoveSpeed;
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
        protected void BuffExorcistByCrossStack(Fugitive fugitive)
        {
            if (fugitive.GetStack < 2)
            {

            }
            else if (fugitive.GetStack < 5)
            {
                DataManager.Instance.LocalPlayerData.roleData.MoveSpeed = DataManager.Instance.RoleInfos[(int)DataManager.Instance.GetLocalRoleType].MoveSpeed * CrossStackSpeedUpRate;
            }
            else
            { 
                DataManager.Instance.LocalPlayerData.roleData.MoveSpeed = DataManager.Instance.RoleInfos[(int)DataManager.Instance.GetLocalRoleType].MoveSpeed * CrossStackSpeedUpRate*2;
            }
        }
        protected bool CheckFugitivesIsChasedOnView()
        {
            bool check = false;
            foreach (Fugitive fugitive in Fugitives)
            {
                if (!fugitive)
                {
                    continue;
                }
                if (fugitive.IsChased&&fugitive.IsWatched)
                {
                    BuffExorcistByCrossStack(fugitive);
                    check = true;
                }
            }
            return check;
        }

        private void OnDrawGizmos()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (Fugitives.Count == 0)
            {
                return;
            }
            RaycastHit[] Hits;
            Vector3 CamPos = mainCamera.transform.position;
            foreach (Fugitive fugitive in Fugitives)
            {
                if (fugitive == null)
                {
                    continue;
                }
                Ray ray = new Ray(CamPos, fugitive.transform.position - CamPos);
                Hits = Physics.RaycastAll(ray, sphereCollider.radius);

                Gizmos.color = Color.red;
                Gizmos.DrawLine(ray.origin, Hits[Hits.Length-1].point);

                Gizmos.color = Color.cyan;
                if (Hits.Length > 0)
                {
                    foreach (RaycastHit hit in Hits)
                    {
                        Gizmos.DrawSphere(hit.point, 0.3f);
                    }
                }
            }
            

        }
    }
}

