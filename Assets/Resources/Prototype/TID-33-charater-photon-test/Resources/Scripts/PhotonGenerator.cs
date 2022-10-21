using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using DEM;
using LSH_Lib;

namespace GHJ_Lib
{ 

    public class PhotonGenerator : MonoBehaviour
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
        [SerializeField]
        private GameObject[] PurificationBoxGenPos;
        [SerializeField]
        private Vector3 CenterPosition;
        [SerializeField]
        private float CenterDistance;
        [SerializeField]
        private int NormalAltarCount;
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
        [Header("PhotonNetwork.RegisterPrefab")]
        [SerializeField]
        private GameObject puricationBoxModel;
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
            PhotonNetwork.PrefabPool.RegisterPrefab("purificationBoxModel", puricationBoxModel);
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

                GenerateNormalAltar(NormalAltarCount);
                InstantiateExitAltar(2);
                InstantiateFinalAltar();
                InstantiatePurificationBox();
            }
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

        private GameObject InstantiateNormalAltar(int i)
        {
            GameObject NormalAltar = PhotonNetwork.Instantiate("Prototype/TID-71-merge-character-and-object/Resources/Prefabs/NormalAltar", NormalAltarGenPos[i].transform.position, Quaternion.Euler(NormalAltarGenPos[i].transform.rotation.eulerAngles), 0);
            if (NormalAltar)
            {
                return NormalAltar;
            }
            else
            {
                Debug.LogError("can't instantiate NormalAltar");
                return null;
            }
        }
        private GameObject InstantiateNormalAltar(GameObject gen)
        {

            GameObject NormalAltar = PhotonNetwork.Instantiate("Prototype/TID-71-merge-character-and-object/Resources/Prefabs/NormalAltar", gen.transform.position, Quaternion.Euler(gen.transform.rotation.eulerAngles), 0);
            if (NormalAltar)
            {
                Debug.Log("instantiate NormalAltar");
                return NormalAltar;
            }
            else
            {
                Debug.LogError("can't instantiate NormalAltar");
                return null;
            }
        }


        private GameObject InstantiateExitAltar(int i)
        {
            GameObject exitAltar = PhotonNetwork.Instantiate("Prototype/TID-71-merge-character-and-object/Resources/Prefabs/ExitAltar", ExitAltarGenPos[i].transform.position, Quaternion.Euler(ExitAltarGenPos[i].transform.rotation.eulerAngles), 0);
            if (exitAltar)
            {
                return exitAltar;
            }
            else
            {
                Debug.LogError("can't instantiate ExitAltar");
                return null;
            }
        }

        private GameObject InstantiateFinalAltar()
        {
            GameObject finalAltar = PhotonNetwork.Instantiate("Prototype/TID-71-merge-character-and-object/Resources/Prefabs/FinalAltar", FinalAltarGenPos.transform.position, Quaternion.Euler(FinalAltarGenPos.transform.rotation.eulerAngles), 0);
            if (finalAltar)
            {
                return finalAltar;
            }
            else
            {
                Debug.LogError("can't instantiate FinalAltar");
                return null;
            }
        }

        private bool InstantiatePurificationBox()
        {

            for (int i = 0;i< PurificationBoxGenPos.Length;++i)
            {
                GameObject purificationBox = PhotonNetwork.Instantiate("purificationBoxModel", PurificationBoxGenPos[i].transform.position, Quaternion.Euler(PurificationBoxGenPos[i].transform.rotation.eulerAngles), 0);

                if (!purificationBox)
                {
                    Debug.LogError("can't instantiate");
                    return false;
                }
            }

            return true;
        }


        void GenerateNormalAltar(int AltarCount)
        {
            List<GameObject> AltarGenPos = new List<GameObject>();
            List<int> inCenterAltars = new List<int>();
            List<int> outCenterAltars = new List<int>();
            List<GameObject> Altars = new List<GameObject>();

            for (int i = 0; i < NormalAltarGenPos.Length; i++)
            {
                AltarGenPos.Add(NormalAltarGenPos[i]);
                if (CenterDistance > (NormalAltarGenPos[i].gameObject.transform.position - CenterPosition).magnitude)
                {
                    inCenterAltars.Add(i);
                }
                else 
                {
                    outCenterAltars.Add(i);
                }

            }

            int index = inCenterAltars[Random.Range(0, inCenterAltars.Count)];
            GameObject A = InstantiateNormalAltar(index);
            AltarGenPos.Remove(NormalAltarGenPos[index]);
            Altars.Add(A);

            index = outCenterAltars[Random.Range(0, outCenterAltars.Count)];
            GameObject B = InstantiateNormalAltar(index);
            AltarGenPos.Remove(NormalAltarGenPos[index]);
            Altars.Add(B);

            for (int i = 0; i < AltarCount - 2; ++i)
            {
                GameObject GenPos = GetMaxDistancePos(AltarGenPos, Altars);
                GameObject C = InstantiateNormalAltar(GenPos);
                AltarGenPos.Remove(GenPos);
                Altars.Add(C);
            }
        }


        GameObject GetMaxDistancePos(List<GameObject> altarGenPos, List<GameObject> altars)
        {
            float maxDistance = 0;
            float curDistance = 0;
            int MaxIndex = 0;
            for (int i = 0;i< altarGenPos.Count;++i)
            {
                curDistance = 0;
                for (int j = 0; j < altars.Count; ++j)
                {
                    curDistance += (altarGenPos[i].transform.position - altars[j].transform.position).magnitude;
                }

                if (curDistance > maxDistance)
                {
                    MaxIndex = i;
                    maxDistance = curDistance;
                }
            }

            return altarGenPos[MaxIndex];
        }
        
    

        
        #endregion

    }
}
