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
        //[System.Serializable]
        //public struct SelectInfo
        //{
        //    public Sprite image;
        //    public string playerName;
        //    public string selectedRole;
        //    public bool isSelected;

        //    public SelectInfo(Sprite image, string playerName, string selectedRole, bool isSelected)
        //    {
        //        this.image = image;
        //        this.playerName = playerName;
        //        this.selectedRole = selectedRole;
        //        this.isSelected = isSelected;
        //    }
        //}

        [System.Serializable]
        public struct PlayerUi
        {
            public Image icon;
            public TextMeshProUGUI nickname;
            public TextMeshProUGUI roleName;

            public void Refresh(Sprite sprite, string nicknameStr, string roleNameStr)
            {
                icon.sprite = sprite;
                nickname.text = nicknameStr;
                roleName.text = roleNameStr;
            }
        }

        [System.Serializable]
        public struct InfoMatch
        {
            public int playerIdx;
            //public SelectInfo info;
            public PlayerUi ui;
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
        [SerializeField] InfoMatch[] infoMatches;
        //[SerializeField] PlayerUi[] playerUis;

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
            infoMatches = new InfoMatch[GameManager.Instance.CurPlayerCount];
            IndexingInfo();
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

            //infoMatches[0].info = GetSelectInfoByRoleTypeOrder(DataManager.Instance.LocalPlayerData.roleData.TypeOrder);
            infoMatches[0] = GetSelectInfoByRoleTypeOrder( infoMatches[0], DataManager.Instance.LocalPlayerData.roleData.TypeOrder );

            photonView.RPC("ChangeSelectInfosRPC", RpcTarget.AllViaServer, playerIdx, (int)DataManager.Instance.LocalPlayerData.roleData.TypeOrder);

            Debug.Log($"Selected {DataManager.Instance.PreRoleTypeOrder}");
        }

        [PunRPC]
        public void ChangeSelectInfosRPC(int playerIdx, int typeOrder)
        {
            for(int i = 0; i < infoMatches.Length; ++i)
            {
                if(infoMatches[i].playerIdx == playerIdx)
                {
                    infoMatches[i] = GetSelectInfoByRoleTypeOrder( infoMatches[i], DataManager.Instance.LocalPlayerData.roleData.TypeOrder );
                }
            }
        }
        public InfoMatch GetSelectInfoByRoleTypeOrder( InfoMatch infoMatch, RoleData.RoleTypeOrder typeOrder )
        {
            switch ( typeOrder )
            {
                case RoleData.RoleTypeOrder.Bishop:
                {
                    infoMatch.ui.Refresh( roleSprites[(int)RoleData.RoleTypeOrder.Bishop],
                        DataManager.Instance.LocalPlayerData.accountData.Nickname,
                        "아타나시오"
                        );
                    return infoMatch;
                }
                case RoleData.RoleTypeOrder.Hunter:
                {
                    infoMatch.ui.Refresh( roleSprites[(int)RoleData.RoleTypeOrder.Hunter],
                        DataManager.Instance.LocalPlayerData.accountData.Nickname,
                        "샬라이"
                        );
                    return infoMatch;
                }
                case RoleData.RoleTypeOrder.Photographer:
                {
                    infoMatch.ui.Refresh( roleSprites[(int)RoleData.RoleTypeOrder.Photographer],
                        DataManager.Instance.LocalPlayerData.accountData.Nickname,
                        "강채율"
                        );
                    return infoMatch;
                }
                case RoleData.RoleTypeOrder.Priest:
                {
                    infoMatch.ui.Refresh( roleSprites[(int)RoleData.RoleTypeOrder.Priest],
                        DataManager.Instance.LocalPlayerData.accountData.Nickname,
                        "알베르토 이든"
                        );
                    return infoMatch;
                }
                case RoleData.RoleTypeOrder.Wolf:
                {
                    infoMatch.ui.Refresh( roleSprites[(int)RoleData.RoleTypeOrder.Wolf],
                        DataManager.Instance.LocalPlayerData.accountData.Nickname,
                        "라이"
                        );
                    return infoMatch;
                }
                case RoleData.RoleTypeOrder.Rabbit:
                {
                    infoMatch.ui.Refresh( roleSprites[(int)RoleData.RoleTypeOrder.Rabbit],
                        DataManager.Instance.LocalPlayerData.accountData.Nickname,
                        "제니"
                        );
                    return infoMatch;
                }
                case RoleData.RoleTypeOrder.Tortoise:
                {
                    infoMatch.ui.Refresh( roleSprites[(int)RoleData.RoleTypeOrder.Tortoise],
                        DataManager.Instance.LocalPlayerData.accountData.Nickname,
                        "태오"
                        );
                    return infoMatch;
                }
                case RoleData.RoleTypeOrder.Penguin:
                {
                    infoMatch.ui.Refresh( roleSprites[(int)RoleData.RoleTypeOrder.Penguin],
                        DataManager.Instance.LocalPlayerData.accountData.Nickname,
                        "제임스"
                        );
                    return infoMatch;
                }

                default:
                {
                    Debug.LogError( "GetSelectInfoByRoleTypeOrder: No Selected " );
                    return new InfoMatch();
                }
            }
        }
        void IndexingInfo()
        {
            if (DataManager.Instance.PreRoleType == RoleData.RoleType.Doll)
            {
                infoMatches[0].playerIdx = infoMatches.Length - 1;

                for (int i = 1; i < infoMatches.Length; ++i)
                {
                    if (infoMatches[i].playerIdx == 0)
                    {
                        infoMatches[i].playerIdx = -1;
                    }
                }
                for (int i = 1; i < infoMatches.Length; ++i)
                {
                    if (i == playerIdx)
                    {
                        infoMatches[i].playerIdx = 0;
                        break;
                    }
                }
                for (int i = 1; i < infoMatches.Length; ++i)
                {
                    if (infoMatches[i].playerIdx == -1)
                    {
                        infoMatches[i].playerIdx = i;
                        break;
                    }
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
