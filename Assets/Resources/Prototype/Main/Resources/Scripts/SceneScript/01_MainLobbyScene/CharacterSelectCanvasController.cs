using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using KSH_Lib.Data;

namespace KSH_Lib.UI
{
    public class CharacterSelectCanvasController : MonoBehaviour
    {
        /*--- Public Fields ---*/

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

        /*--- Private Fields ---*/


        /*--- MonoBehaviour Callbacks ---*/

        private void Start()
        {
            DisablAllInformation();
            bishopInformation.SetActive(true);
        }

        /*--- Public Methods ---*/
        public void OnSelectRole()
        {
            if (DataManager.Instance.PreRoleType == RoleData.RoleType.Doll)
            {
                EnableDollButtons();
            }
            else if (DataManager.Instance.PreRoleType == RoleData.RoleType.Exorcist)
            {
                EnableExorcistButtons();
            }
        }

        public void OnSelectCharacter(string name)
        {
            DataManager.Instance.PreRoleTypeOrder = (RoleData.RoleTypeOrder)System.Enum.Parse(typeof(RoleData.RoleTypeOrder), name);
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
            }
        }
    }
}
