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
        protected string FloorTag = "Floor";
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
                Log.Instance.WriteLog($"CoolDowntime : {CoolDowntime}" , 1);
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
                                    DataManager.Instance.LocalPlayerData.roleData.MoveSpeed = DataManager.Instance.RoleInfos[(int)DataManager.Instance.GetLocalRoleType].MoveSpeed;
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

            Hits = Physics.RaycastAll(ray, sphereCollider.radius, LayerMask.NameToLayer(GameManager.EnvironmentLayer));

            foreach (RaycastHit hit in Hits)
            {
                Debug.Log($"Hits : {hit.collider.name}");
                if (!hit.collider.gameObject.CompareTag(FloorTag))
                {
                    return false;
                }
            }
            Debug.Log("return True");
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
            Debug.Log($"Check : {check}");
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

            foreach (Fugitive fugitive in Fugitives)
            {
                if (fugitive == null)
                {
                    continue;
                }
                Vector3 CamPos = mainCamera.transform.position;
                Ray ray = new Ray(CamPos, fugitive.transform.position - CamPos);

                Hits = Physics.RaycastAll(ray, sphereCollider.radius, LayerMask.NameToLayer(GameManager.EnvironmentLayer));

                Gizmos.color = Color.red;
                Gizmos.DrawLine(ray.origin, ray.origin +ray.direction*40.0f);

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

