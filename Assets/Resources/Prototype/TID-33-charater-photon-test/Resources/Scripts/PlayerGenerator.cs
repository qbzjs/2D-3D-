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
        private GameObject localPlayerObj = null;
        #endregion

        #region MonoBehaviour CallBacks
        private void Awake()
        {
            camControllerFPV = virtualCamera.GetComponent<Network_FPV_CameraController>();
            if (!camControllerFPV)
            {
                Debug.LogError("Missing FPV_CameraController");
            }
            camControllerTPV = virtualCamera.GetComponent<Network_TPV_CameraController>();
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
                camControllerTPV.SendMessage("SetCamTarget", localPlayerObj.transform.GetChild(1).gameObject);
                camControllerTPV.SendMessage("SetModeTPV");
            }
            else if (role == RoleType.Exorcist)
            {
                camControllerFPV.enabled = true;
                camControllerTPV.enabled = false;

                InstantiateExorcist();
                camControllerFPV.SendMessage("SetModeFPV", localPlayerObj.transform.GetChild(1).gameObject);
                
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
            for (int i = 1; i < GameManager.Instance.MaxPlayerCount - 1; ++i)
            {
                RaycastHit hit;
                Ray ray = new Ray(genPos[i] + new Vector3(0, 10, 0), Vector3.down);
                Physics.Raycast(ray, out hit);


                if (hit.collider.CompareTag("Untagged"))
                {
                    localPlayerObj = PhotonNetwork.Instantiate("Prototype/TID-48-implement-hit-system/Resources/Prefabs/Doll", genPos[i], Quaternion.identity, 0);
                    return true;
                }

            }
            Debug.LogError("can't instantiate Doll character");
            return false;
        }

        private bool InstantiateExorcist()
        {
            localPlayerObj = PhotonNetwork.Instantiate("Prototype/TID-48-implement-hit-system/Resources/Prefabs/Exorcist", genPos[0], Quaternion.identity, 0);
            if (localPlayerObj)
            {
                return true;
            }
            else
            {
                Debug.LogError("can't instantiate Exorcist character");
                return false;
            }
        }
        #endregion	

    }
}
