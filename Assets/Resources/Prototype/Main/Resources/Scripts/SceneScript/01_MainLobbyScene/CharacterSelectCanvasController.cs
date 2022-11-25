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
    public class CharacterSelectCanvasController : MonoBehaviourPunCallbacks, IPunObservable
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
            public void ChangeTextColor( TextMeshProUGUI targetText, Color32 color)
            {
                targetText.color = color;
            }
        }

        [System.Serializable]
        public struct InfoMatch
        {
            public int playerIdx;
            //public SelectInfo info;
            public PlayerUi ui;
        }

        enum ImageOrder { Bishop, Hunter, Photographer, Priest, Penguin, Rabbit, Tortoise, Wolf }


        /*--- Public Fields ---*/
        public int playerIdx { get { return DataManager.Instance.PlayerIdx; } }
        public int exorcistIdx
        {
            get
            {
                if(DataManager.Instance.PreRoleGroup == RoleData.RoleGroup.Exorcist)
                {
                    return 0;
                }
                else if(DataManager.Instance.PreRoleGroup == RoleData.RoleGroup.Doll)
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
        [SerializeField] string nextSceneName;

        [Header("Character Select Buttons")]
        public GameObject DollButtons;
        public GameObject ExorcistButtons;
        [SerializeField] InfoMatch[] infoMatches;
        //SortedSet<InfoMatch> infoSets;

        [Header( "Debug" )]
        [SerializeField] bool[] isDecidedArr;

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


        [Header( "Text Color Setting" )]
        [SerializeField] Color32 exorcistColor;
        [SerializeField] Color32 roleNameColor;
        [SerializeField] Color32 watingColor;
        [SerializeField] Color32 localColor;

        /*--- Private Fields ---*/
        bool isStartChangeScene;


        /*--- MonoBehaviour Callbacks ---*/


        private void Start()
        {
            DisablAllInformation();
            OnSelectRole();

            DataManager.Instance.InitPlayerDatas();
            //bishopInformation.SetActive(true);
            isDecidedArr = new bool[PhotonNetwork.CurrentRoom.PlayerCount];
            IndexingInfo();
        }

        /*--- Public Methods ---*/
        private void Update()
        {
            if(PhotonNetwork.IsMasterClient && !isStartChangeScene )
            {
                if ( IsAllPlayerDecided() )
                {
                    GameManager.Instance.LoadPhotonScene( nextSceneName );
                    isStartChangeScene = true;
                }
            }
        }

        bool IsAllPlayerDecided()
        {
            bool flag = true;
            foreach(bool isDecided in isDecidedArr)
            {
                flag &= isDecided;
            }
            return flag;
        }


        public void OnSelectRole()
        {
            if (DataManager.Instance.PreRoleGroup == RoleData.RoleGroup.Doll)
            {
                EnableDollButtons();
            }
            if (DataManager.Instance.PreRoleGroup == RoleData.RoleGroup.Exorcist)
            {
                EnableExorcistButtons();
            }
        }

        public void OnSelectCharacter(string name)
        {
            DataManager.Instance.PreRoleName = (RoleData.RoleType)System.Enum.Parse(typeof(RoleData.RoleType), name);
            DataManager.Instance.InitLocalRoleData();
            DataManager.Instance.ShareRoleData();

            infoMatches[0] = GetSelectInfoByRoleType( infoMatches[0], DataManager.Instance.LocalPlayerData.roleData.Type );
            
            photonView.RPC("ChangeSelectInfosRPC", RpcTarget.Others, playerIdx, (int)DataManager.Instance.LocalPlayerData.roleData.Type);

            if(playerIdx != 0)
            {
                infoMatches[0].ui.ChangeTextColor( infoMatches[0].ui.nickname, localColor );
            }

            Debug.Log($"Selected {DataManager.Instance.PreRoleName}");
        }

        [PunRPC]
        public void ChangeSelectInfosRPC(int playerIdx, int type)
        {
            for(int i = 0; i < infoMatches.Length; ++i)
            {
                if(infoMatches[i].playerIdx == playerIdx)
                {
                    infoMatches[i] = GetSelectInfoByRoleType( infoMatches[i], (RoleData.RoleType)type);
                }
            }
        }
        public InfoMatch GetSelectInfoByRoleType( InfoMatch infoMatch, RoleData.RoleType type )
        {
            Color nameColor;
            switch ( type )
            {
                case RoleData.RoleType.Bishop:
                {
                    infoMatch.ui.Refresh( roleSprites[(int)ImageOrder.Bishop],
                        DataManager.Instance.LocalPlayerData.accountData.Nickname,
                        "아타나시오"
                        );
                    nameColor = exorcistColor;
                }
                break;
                case RoleData.RoleType.Hunter:
                {
                    infoMatch.ui.Refresh( roleSprites[(int)ImageOrder.Hunter],
                        DataManager.Instance.LocalPlayerData.accountData.Nickname,
                        "샬라이"
                        );
                    nameColor = exorcistColor;
                }
                break;
                case RoleData.RoleType.Photographer:
                {
                    infoMatch.ui.Refresh( roleSprites[(int)ImageOrder.Photographer],
                        DataManager.Instance.LocalPlayerData.accountData.Nickname,
                        "강채율"
                        );
                    nameColor = exorcistColor;
                }
                break;
                case RoleData.RoleType.Priest:
                {
                    infoMatch.ui.Refresh( roleSprites[(int)ImageOrder.Priest],
                        DataManager.Instance.LocalPlayerData.accountData.Nickname,
                        "알베르토 이든"
                        );
                    nameColor = exorcistColor;
                }
                break;
                case RoleData.RoleType.Wolf:
                {
                    infoMatch.ui.Refresh( roleSprites[(int)ImageOrder.Wolf],
                        DataManager.Instance.LocalPlayerData.accountData.Nickname,
                        "라이"
                        );
                    nameColor = roleNameColor;
                }
                break;
                case RoleData.RoleType.Rabbit:
                {
                    infoMatch.ui.Refresh( roleSprites[(int)ImageOrder.Rabbit],
                        DataManager.Instance.LocalPlayerData.accountData.Nickname,
                        "제니"
                        );
                    nameColor = roleNameColor;
                }
                break;
                case RoleData.RoleType.Tortoise:
                {
                    infoMatch.ui.Refresh( roleSprites[(int)ImageOrder.Tortoise],
                        DataManager.Instance.LocalPlayerData.accountData.Nickname,
                        "태오"
                        );
                    nameColor = roleNameColor;
                }
                break;
                case RoleData.RoleType.Penguin:
                {
                    infoMatch.ui.Refresh( roleSprites[(int)ImageOrder.Penguin],
                        DataManager.Instance.LocalPlayerData.accountData.Nickname,
                        "제임스"
                        );
                    nameColor = roleNameColor;
                }
                break;

                default:
                {
                    Debug.LogError( "GetSelectInfoByRoleType: No Selected " );
                    return new InfoMatch();
                }
            }

            infoMatch.ui.ChangeTextColor( infoMatch.ui.nickname, nameColor );
            return infoMatch;
        }
        void IndexingInfo()
        {
            if (DataManager.Instance.PreRoleGroup == RoleData.RoleGroup.Doll)
            {
                infoMatches[infoMatches.Length - 1].playerIdx = 0;

                for (int i = 0; i < infoMatches.Length - 1; ++i)
                {
                    if (infoMatches[i].playerIdx == 0)
                    {
                        infoMatches[i].playerIdx = -1;
                    }
                }
                for (int i = 0; i < infoMatches.Length - 1; ++i)
                {
                    if (i == playerIdx)
                    {
                        infoMatches[0].playerIdx = playerIdx;
                        break;
                    }
                }

                for (int i = 1, pIdx = 1; i < infoMatches.Length - 1; ++i, ++pIdx)
                {
                    if (infoMatches[i].playerIdx == -1)
                    {
                        if ( i == playerIdx )
                        {
                            pIdx++;
                        }
                        infoMatches[i].playerIdx = pIdx;
                    }
                }
            }
            else if ( DataManager.Instance.PreRoleGroup == RoleData.RoleGroup.Exorcist )
            {
                for(int i = 0; i < infoMatches.Length; ++i )
                {
                    infoMatches[i].playerIdx = i;
                }
            }
            else
            {
                Debug.LogError( "No RoleType set" );
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
        //void GameStart()
        //{
        //    LoadRoomScene();
        //}
        void LoadRoomScene()
        {
            DataManager.Instance.InitLocalRoleData();
            decideButtonObj.SetActive(false);
            PhotonNetwork.CurrentRoom.IsOpen = false;
            GameManager.Instance.LoadPhotonScene( nextSceneName );
        }
        void DecideRoleType()
        {
            if(DataManager.Instance.PreRoleName == RoleData.RoleType.Null)
            {
                return;
            }

            DataManager.Instance.InitLocalRoleData();
            decideButtonObj.SetActive(false);
            photonView.RPC( "ShareDecideRPC", RpcTarget.AllViaServer, playerIdx );
        }

        [PunRPC]
        void ShareDecideRPC(int index)
        {
            isDecidedArr[index] = true;
            for(int i = 0; i < infoMatches.Length; ++i )
            {
                if(infoMatches[i].playerIdx == index)
                {
                    // Do Somthing if Decide
                    infoMatches[i].ui.ChangeTextColor( infoMatches[i].ui.roleName, roleNameColor );
                }
            }
        }


        public void OnPhotonSerializeView( PhotonStream stream, PhotonMessageInfo info )
        {
        }
    }
}
