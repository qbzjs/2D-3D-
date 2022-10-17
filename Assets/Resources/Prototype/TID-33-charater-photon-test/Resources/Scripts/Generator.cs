using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using DEM;
using LSH_Lib;

namespace GHJ_Lib
{ 

    public class Generator : MonoBehaviour
    {
        #region Public Fields
        #endregion

        #region Private Fields
        RoleType role;
        [Header("Generate Setting")]
        [SerializeField]
        private Vector3[] PlayerGenPos;
        [SerializeField]
        private GameObject[] NormalAltarGenPos;
        [SerializeField]
        private GameObject[] ExitAltarGenPos;
        [SerializeField]
        private GameObject FinalAltarGenPos;
        [Header("Camera Setting")]
        [SerializeField]
        private GameObject virtualCamera;
        private FPV_CameraController camControllerFPV;
        private TPV_CameraController camControllerTPV;
        private GameObject localPlayerObj = null;
        [Header("UI Setting")]
        [SerializeField]
        private DollUI dollUI;
        [SerializeField]
        private ExorcistUI exorcistUI;
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
            /*
            dollUI = GameObject.Find("DollUI").GetComponent<DollUI>();
            if (dollUI == null)
            {
                Debug.LogError("Missing DollUI");
            }
            */

        }
        void Start()
        {
            role = GameManager.Instance.Data.Role;
            if (role == RoleType.Doll)
            {
                camControllerFPV.enabled = false;
                camControllerTPV.enabled = true;
                exorcistUI.gameObject.SetActive(false);

                InstantiateDoll();
                //dollUI.SetStatus(localPlayerObj.GetComponent<DollStatus>());
                camControllerTPV.SendMessage("SetCamTarget", localPlayerObj.transform.GetChild(1).gameObject);
                camControllerTPV.SendMessage("SetModeTPV");
            }
            else if (role == RoleType.Exorcist)
            {
                camControllerFPV.enabled = true;
                camControllerTPV.enabled = false;
                dollUI.gameObject.SetActive(false);

                InstantiateExorcist();
                GameObject head = GameObject.Find("B-head").gameObject;
                if (head == null)
                {
                    Debug.LogError("can't find head");
                }
                camControllerFPV.SendMessage("SetModeFPV", head);

                InstantiateNormalAltar(8);

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
                if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[i])
                {
                    localPlayerObj = PhotonNetwork.Instantiate("Prototype/TID-71-merge-character-and-object/Resources/Prefabs/Doll", PlayerGenPos[i], Quaternion.identity, 0);
                    return true;
                }
        
            }
            Debug.LogError("can't instantiate Doll character");
            return false;
        }

        private bool InstantiateExorcist()
        {
            localPlayerObj = PhotonNetwork.Instantiate("Prototype/TID-71-merge-character-and-object/Resources/Prefabs/Exorcist", PlayerGenPos[0], Quaternion.identity, 0);
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

        private bool InstantiateNormalAltar(int i)
        {
            GameObject NormalAltar = PhotonNetwork.Instantiate("Prototype/TID-71-merge-character-and-object/Resources/Prefabs/NormalAltar", NormalAltarGenPos[i].transform.position, Quaternion.Euler(NormalAltarGenPos[i].transform.rotation.eulerAngles), 0);
            if (NormalAltar)
            {
                return true;
            }
            else
            {
                Debug.LogError("can't instantiate NormalAltar");
                return false;
            }
        }
        #endregion	

    }
}
