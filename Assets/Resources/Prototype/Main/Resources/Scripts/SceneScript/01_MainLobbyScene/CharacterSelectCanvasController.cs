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

        [Header( "Character Select Buttons" )]
        public GameObject DollButtons;
        public GameObject ExorcistButtons;

        /*--- Protected Fields ---*/


        /*--- Private Fields ---*/


        /*--- MonoBehaviour Callbacks ---*/



        /*--- Public Methods ---*/
        public void OnSelectRole()
        {
            if ( DataManager.Instance.PreRoleType == RoleData.RoleType.Doll )
            {
                EnableDollButtons();
            }
            else if ( DataManager.Instance.PreRoleType == RoleData.RoleType.Exorcist )
            {
                EnableExorcistButtons();
            }
        }

        public void OnSelectCharacter(string name)
        {
            DataManager.Instance.PreRoleTypeOrder = (RoleData.RoleTypeOrder)System.Enum.Parse( typeof( RoleData.RoleTypeOrder ), name );
            Debug.Log( $"Selected {DataManager.Instance.PreRoleTypeOrder}" );
        }


        /*--- Protected Methods ---*/


        /*--- Private Methods ---*/
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
    }
}
