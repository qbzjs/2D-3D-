using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;


namespace KSH_Lib.Test
{
    public class RoomChanger : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI roomName;
        [SerializeField] TextMeshProUGUI inputFieldText;
        const string RoomKey = "RoomName";

        private void OnEnable()
        {
            roomName.text = PlayerPrefs.GetString( RoomKey );
        }
        public void OnSaveButtonClick( )
        {
            PlayerPrefs.SetString( RoomKey, inputFieldText.text );
            roomName.text = PlayerPrefs.GetString( RoomKey );
        }
    }
}