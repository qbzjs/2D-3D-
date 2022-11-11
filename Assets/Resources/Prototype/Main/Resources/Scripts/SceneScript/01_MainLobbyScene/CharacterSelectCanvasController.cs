using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon;
using Photon.Pun;
using Photon.Realtime;

using TMPro;

using KSH_Lib.Data;

namespace KSH_Lib.UI
{
    public class CharacterSelectCanvasController : MonoBehaviourPun, IPunObservable
    {
        [System.Serializable]
        public struct SelectInfo
        {
            public Sprite image;
            public string playerName;
            public string selectedRole;
            public bool isSelected;

            public SelectInfo( Sprite image, string playerName, string selectedRole, bool isSelected)
            {
                this.image = image;
                this.playerName = playerName;
                this.selectedRole = selectedRole;
                this.isSelected = isSelected;
            }
        }


        /*--- Public Fields ---*/
        public int playerIdx { get { return DataManager.Instance.PlayerIdx; } }
        public int exorcistIdx
        {
            get
            {
                if(DataManager.Instance.PreRoleType == RoleData.RoleType.Exorcist)
                {
                    return 0;
                }
                else if(DataManager.Instance.PreRoleType == RoleData.RoleType.Doll)
                {
                    return 4;
                }
                else
                {
                    Debug.LogError( "No PreRoleType set" );
                    return -1;
                }
            }
        }

        [SerializeField]
        private string loadSceneName = "02_MainGameScene";

        [Header("Character Select Buttons")]
        public GameObject DollButtons;
        public GameObject ExorcistButtons;
        [SerializeField] SelectInfo[] selectInfos;// = new SelectInfo[GameManager.Instance.CurPlayerCount];

        /*--- Protected Fields ---*/
        [SerializeField]
        GameObject decideButtonObj;

        [Header("Exorcist Select UI")]
        [SerializeField]
        GameObject bishopButton;
        [SerializeField]
        GameObject hunterButton;
        [SerializeField]
        GameObject photographerButton;
        [SerializeField]
        GameObject priestButton;

        [SerializeField]
        GameObject bishopInformation;
        [SerializeField]
        GameObject hunterInformation;
        [SerializeField]
        GameObject photographerInformation;
        [SerializeField]
        GameObject priestInformation;

        [Header("Doll Select UI")]
        [SerializeField]
        GameObject wolfButton;
        [SerializeField]
        GameObject rabbitButton;
        [SerializeField]
        GameObject tortoiseButton;
        [SerializeField]
        GameObject penguinButton;

        [SerializeField]
        GameObject wolfInformation;
        [SerializeField]
        GameObject rabbitInformation;
        [SerializeField]
        GameObject tortoiseInformation;
        [SerializeField]
        GameObject penguinInformation;

        [Header( "Icon Images" )]
        [SerializeField] Sprite[] roleSprites;
        

        /*--- Private Fields ---*/

        /*--- MonoBehaviour Callbacks ---*/


        private void Start()
        {
            DisablAllInformation();
            OnSelectRole();

            DataManager.Instance.InitPlayerDatas();
            //bishopInformation.SetActive(true);
        }
        private void Update()
        {
            selectInfos = new SelectInfo[GameManager.Instance.CurPlayerCount];
        }
        /*--- Public Methods ---*/
        public void OnSelectRole()
        {
            if (DataManager.Instance.PreRoleType == RoleData.RoleType.Doll)
            {
                EnableDollButtons();
            }
            if (DataManager.Instance.PreRoleType == RoleData.RoleType.Exorcist)
            {
                EnableExorcistButtons();
            }
        }

        public void OnSelectCharacter(string name)
        {
            DataManager.Instance.PreRoleTypeOrder = (RoleData.RoleTypeOrder)System.Enum.Parse(typeof(RoleData.RoleTypeOrder), name);
            DataManager.Instance.InitLocalRoleData();
            DataManager.Instance.ShareRoleData();

            selectInfos[0] = GetSelectInfoByRoleTypeOrder( DataManager.Instance.LocalPlayerData.roleData.TypeOrder );


            Debug.Log($"Selected {DataManager.Instance.PreRoleTypeOrder}");
        }
        public SelectInfo GetSelectInfoByRoleTypeOrder( RoleData.RoleTypeOrder typeOrder )
        {
            switch(typeOrder)
            {
                case RoleData.RoleTypeOrder.Bishop:
                {
                    return new SelectInfo( roleSprites[(int)RoleData.RoleTypeOrder.Bishop],
                        DataManager.Instance.LocalPlayerData.accountData.Nickname,
                        "아타나시오", true
                        );
                }
                case RoleData.RoleTypeOrder.Hunter:
                    {
                        return new SelectInfo(roleSprites[(int)RoleData.RoleTypeOrder.Hunter],
                            DataManager.Instance.LocalPlayerData.accountData.Nickname,
                            "샬라이", true
                            );
                    }
                case RoleData.RoleTypeOrder.Photographer:
                    {
                        return new SelectInfo(roleSprites[(int)RoleData.RoleTypeOrder.Photographer],
                            DataManager.Instance.LocalPlayerData.accountData.Nickname,
                            "강채율", true
                            );
                    }
                case RoleData.RoleTypeOrder.Priest:
                    {
                        return new SelectInfo(roleSprites[(int)RoleData.RoleTypeOrder.Priest],
                            DataManager.Instance.LocalPlayerData.accountData.Nickname,
                            "알베르토 이든", true
                            );
                    }
                case RoleData.RoleTypeOrder.Wolf:
                    {
                        return new SelectInfo(roleSprites[(int)RoleData.RoleTypeOrder.Wolf],
                            DataManager.Instance.LocalPlayerData.accountData.Nickname,
                            "라이", true
                            );
                    }
                case RoleData.RoleTypeOrder.Rabbit:
                    {
                        return new SelectInfo(roleSprites[(int)RoleData.RoleTypeOrder.Rabbit],
                            DataManager.Instance.LocalPlayerData.accountData.Nickname,
                            "제니", true
                            );
                    }
                case RoleData.RoleTypeOrder.Tortoise:
                    {
                        return new SelectInfo(roleSprites[(int)RoleData.RoleTypeOrder.Tortoise],
                            DataManager.Instance.LocalPlayerData.accountData.Nickname,
                            "태오", true
                            );
                    }
                case RoleData.RoleTypeOrder.Penguin:
                    {
                        return new SelectInfo(roleSprites[(int)RoleData.RoleTypeOrder.Penguin],
                            DataManager.Instance.LocalPlayerData.accountData.Nickname,
                            "제임스", true
                            );
                    }

                default:
                {
                    Debug.LogError( "GetSelectInfoByRoleTypeOrder: No Selected " );
                    return new SelectInfo();
                }
            }
        }

            /*--- Protected Methods ---*/

            /*--- Private Methods ---*/
        void DisablAllInformation()
        {
            bishopInformation.SetActive(false);
            hunterInformation.SetActive(false);
            photographerInformation.SetActive(false);
            priestInformation.SetActive(false);

            wolfInformation.SetActive(false);
            rabbitInformation.SetActive(false);
            tortoiseInformation.SetActive(false);
            penguinInformation.SetActive(false);

        }
        void EnableDollButtons()
        {
            DollButtons.SetActive( true );
            ExorcistButtons.SetActive( false );
        }
        void EnableExorcistButtons()
        {
            DollButtons.SetActive( false );
            ExorcistButtons.SetActive( true );
        }
        public void EnableCharacterInformation(string RoleType)
        {
            switch (RoleType)
            {
                case "Bishop":
                    DisablAllInformation();
                    bishopInformation.SetActive(true);
                    break;
                case "Hunter":
                    DisablAllInformation();
                    hunterInformation.SetActive(true);
                    break;
                case "Photographer":
                    DisablAllInformation();
                    photographerInformation.SetActive(true);
                    break;
                case "Priest":
                    DisablAllInformation();
                    priestInformation.SetActive(true);
                    break;

                case "Wolf":
                    DisablAllInformation();
                    wolfInformation.SetActive(true);
                    break;

                case "Rabbit":
                    DisablAllInformation();
                    rabbitInformation.SetActive(true);
                    break;

                case "Tortoise":
                    DisablAllInformation();
                    tortoiseInformation.SetActive(true);
                    break;

                case "Penguin":
                    DisablAllInformation();
                    penguinInformation.SetActive(true);
                    break;
            }
        }
        void GameStart()
        {
            LoadRoomScene();
        }
        void LoadRoomScene()
        {
            DataManager.Instance.InitLocalRoleData();
            decideButtonObj.SetActive(false);
            PhotonNetwork.CurrentRoom.IsOpen = false;
            GameManager.Instance.LoadPhotonScene(loadSceneName);
        }
        void DecideRoleType()
        {
            DataManager.Instance.InitLocalRoleData();
            decideButtonObj.SetActive(false);
        }

        public void OnPhotonSerializeView( PhotonStream stream, PhotonMessageInfo info )
        {
            throw new System.NotImplementedException();
        }
    }
}
