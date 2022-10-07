using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;
public class currentRoomOption : MonoBehaviour
{
    #region Public Fields
    public Text RoomNumber;
    #endregion	

    #region Private Fields
    #endregion	

    #region MonoBehaviour CallBacks
    void Start()
    {
        int number = (int)PhotonNetwork.CurrentRoom.CustomProperties["roomNumber"];
        RoomNumber.text = "RoomNumber : "+number.ToString();
    }

    void Update()
    {
        
    }
    #endregion	

    #region Public Methods
    #endregion	

    #region Private Methods
    #endregion	
}
