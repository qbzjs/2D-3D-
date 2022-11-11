using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon;
using Photon.Pun;
using Photon.Realtime;

using KSH_Lib.Data;

namespace KSH_Lib.UI
{
    public class CharacterSelectCanvasController : MonoBehaviourPunCallbacks
    {
        /*--- Public Fields ---*/

        [SerializeField]
        private string loadSceneName = "02_MainGameScene";

        [Header("Character Select Buttons")]
        public GameObject DollButtons;
        public GameObject ExorcistButtons;

        /*--- Protected Fields ---*/
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

        [SerializeField]
        GameObject decideButtonObj;

        /*--- Private Fields ---*/

        /*--- MonoBehaviour Callbacks ---*/


        private void Start()
        {
            DisablAllInformation();
            OnSelectRole();

            DataManager.Instance.InitPlayerDatas();
            //bishopInformation.SetActive(true);
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
            Debug.Log($"Selected {DataManager.Instance.PreRoleTypeOrder}");
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
    }
}
