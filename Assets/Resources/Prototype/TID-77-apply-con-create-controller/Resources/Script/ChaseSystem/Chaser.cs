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
        [SerializeField] protected int environmentLayer;
        [SerializeField] protected int CameraLayer;
        protected ExorcistData exorcistData, initData;
        protected RoleData.RoleType roleType;
        private void OnEnable()
        {
            CameraLayer = LayerMask.NameToLayer("Camera");
            environmentLayer = LayerMask.NameToLayer(GameManager.EnvironmentLayer);
            mainCamera = Camera.main;
            sphereCollider = GetComponent<SphereCollider>();
            if (photonView.IsMine)
            {
                exorcistData = (DataManager.Instance.LocalPlayerData.roleData as ExorcistData);
                roleType = DataManager.Instance.GetLocalRoleType;
                initData = DataManager.Instance.RoleInfos[(int)roleType] as ExorcistData;
            }
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

                if (Fugitives.Contains(fugitive))
                {
                    if (photonView.IsMine)
                    {
                        fugitive.SetWatch(false);
                    }
                    Fugitives.Remove(fugitive);
                }
                
            }
        }
        private void Update()
        {
            if (!photonView.IsMine)
            {
                return;
            }

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

                if (!(fugitive.curBehaviour is BvIdle ||
                    fugitive.curBehaviour is BvInteract ||
                    fugitive.curBehaviour is BvGetHit))
                {
                    if (Fugitives.Contains(fugitive))
                    {
                        fugitive.SetWatch(false);
                        Fugitives.Remove(fugitive);
                    }
                    continue;
                }

                if (IsInCameraView(fugitive.gameObject) &&
                    CheckObstacle(fugitive.gameObject))
                {
                    if (!fugitive.IsWatched)
                    {
                        fugitive.SetWatch(true);
                    }
                }
                else
                {
                    if (fugitive.IsWatched)
                    {
                        fugitive.SetWatch(false);
                    }
                }
            }
            Log.Instance.WriteLog(chaseState.ToString(), 0);
            Log.Instance.WriteLog($"CoolDowntime : {CoolDowntime}" , 1);

            if (CheckFugitivesIsChasedOnView())
            {
                switch (roleType)
                {
                    case RoleData.RoleType.Bishop:
                        {
                            int crossStack = 0;
                            foreach (Fugitive fugitive in Fugitives)
                            {
                                if (crossStack < fugitive.GetStack)
                                {
                                    crossStack = fugitive.GetStack;
                                }
                            }
                            BuffExorcistByCrossStack(crossStack);
                        }
                        break;
                }


                if (chaseState != ChaseState.Chasing)
                {
                    chaseState = ChaseState.Chasing;
                    //DataManager.Instance.ShareRoleData();
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
                                exorcistData.MoveSpeed = initData.MoveSpeed;
                                DataManager.Instance.ShareRoleData();
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

        protected bool IsInCameraView(GameObject targetObject)
        {
            Vector3 targetViewPort = mainCamera.WorldToViewportPoint(targetObject.transform.position);
            bool IsInView = (targetViewPort.x <= 1.0f &&
                    targetViewPort.x >= 0.0f &&
                    targetViewPort.y <= 1.0f &&
                    targetViewPort.y >= 0.0f &&
                    targetViewPort.z > 0.0f);
            //Debug.Log($"IsInView : {IsInView}");
            return IsInView;
        }

        protected bool CheckObstacle(GameObject targetObject)
        {
            RaycastHit[] Hits;
            Vector3 CamPos = mainCamera.transform.position;
            Ray ray = new Ray(CamPos, targetObject.transform.position - CamPos);

            
            Hits = Physics.RaycastAll(ray, sphereCollider.radius, CameraLayer);
            

            foreach (RaycastHit hit in Hits)
            {
                GameObject hitObj = hit.collider.gameObject;
                
                if (hitObj.layer == environmentLayer && !hitObj.CompareTag(FloorTag))
                {
                    //Debug.Log("CheckObstacle : false");
                    return false;
                }
            }
            //Debug.Log("CheckObstacle : true");
            return true;
            
        }
        protected void BuffExorcistByCrossStack(int stack)
        {
            if (stack < 2)
            {

            }
            else if (stack < 5)
            {
                exorcistData.MoveSpeed = initData.MoveSpeed * (1+CrossStackSpeedUpRate);
            }
            else
            {
                exorcistData.MoveSpeed = initData.MoveSpeed * (1+CrossStackSpeedUpRate)*2;
            }
            DataManager.Instance.ShareRoleData();
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
                    
                    check = true;
                }
            }
           // Debug.Log($"Check : {check}");
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
            };

            RaycastHit[] Hits;
            foreach (Fugitive fugitive in Fugitives)
            {
                if (fugitive == null)
                {
                    continue;
                }

                
                Vector3 CamPos = mainCamera.transform.position;
                Ray ray = new Ray(CamPos, fugitive.transform.position - CamPos);

                Hits = Physics.RaycastAll(ray, sphereCollider.radius, CameraLayer);

                Gizmos.color = Color.red;
                Gizmos.DrawLine(ray.origin, ray.origin +ray.direction*40.0f);
                if (Hits.Length > 0)
                {
                    foreach (RaycastHit hit in Hits)
                    {
                        GameObject hitObj = hit.collider.gameObject;
                        
                        if (hitObj.layer == environmentLayer && !hitObj.CompareTag(FloorTag))
                        {
                            //Debug.Log($"Hits Enviroment : {hit.collider.name}");
                            Gizmos.color = Color.green;
                            Gizmos.DrawSphere(hit.point, 0.3f);
                        }
                    }
                }
            }
        }
    }
}

