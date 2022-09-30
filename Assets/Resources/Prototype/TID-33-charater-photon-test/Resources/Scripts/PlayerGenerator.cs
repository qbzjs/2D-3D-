using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using DEM;
using KSH_Lib;

namespace GHJ_Lib
{ 

    public class PlayerGenerator : MonoBehaviour
    {
        #region Public Fields
        #endregion

        #region Private Fields
        RoleType role;
        [Header("Generate Setting")]
        [SerializeField]
        private Vector3[] genPos;
        [Header("Camera Setting")]
        [SerializeField]
        private GameObject virtualCamera;
        private FPV_CameraController camControllerFPV;
        private TPV_CameraController camControllerTPV;
        #endregion

        #region MonoBehaviour CallBacks
        private void Awake()
        {
            camControllerFPV = virtualCamera.GetComponent<FPV_CameraController>();
            if (!camControllerFPV)
            {
                Debug.LogError("Missing FPV_CameraController");
            }
            camControllerTPV = virtualCamera.GetComponent<TPV_CameraController>();
            if (!camControllerTPV)
            {
                Debug.LogError("Missing TPV_CameraController");
            }

        }
        void Start()
        {
            role = GameManager.Instance.Data.Role;
            if (role == RoleType.Doll)
            {
                camControllerFPV.enabled = false;
                camControllerTPV.enabled = true;
                InstantiateDoll();
            }
            else if (role == RoleType.Exorcist)
            {
                camControllerFPV.enabled = true;
                camControllerTPV.enabled = false;
                
                InstantiateExorcist();
            }
        }

        void Update()
        {

        }
        #endregion	

        #region Public Methods
        #endregion	

        #region Private Methods
        private bool InstantiateDoll()
        {
            for (int i = 0; i < GameManager.Instance.MaxPlayerCount - 1; ++i)
            {
                RaycastHit hit;
                Ray ray = new Ray(genPos[i] + new Vector3(0, 10, 0), Vector3.down);
                Physics.Raycast(ray, out hit);


                if (hit.collider.CompareTag("Untagged"))
                {
                    PhotonNetwork.Instantiate("Prototype/TID-33-charater-photon-test/Resources/Prefabs/TID_33_Doll", genPos[i], Quaternion.identity, 0);
                    return true; ;
                }

            }
            return false;
        }

        private bool InstantiateExorcist()
        {
            if (PhotonNetwork.Instantiate("Prototype/TID-33-charater-photon-test/Resources/Prefabs/TID_33_Exorcist", genPos[0], Quaternion.identity, 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion	

    }
}
