using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using DEM;

public class PlayerGenerator : MonoBehaviour
{
    #region Public Fields
    #endregion

    #region Private Fields
    RoleType role;
    [Header("Generate Setting")]
    [SerializeField]
    private Vector3[] genPos;

    #endregion

    #region MonoBehaviour CallBacks
    void Start()
    {
        role = GameManager.Instance.Data.Role;
        if (role == RoleType.Doll)
        {
            for (int i = 0; i < GameManager.Instance.MaxPlayerCount - 1; ++i)
            {
                RaycastHit hit;
                Ray ray = new Ray(genPos[i] + new Vector3(0, 10, 0), Vector3.down);
                Physics.Raycast(ray, out hit);


                if (hit.collider.CompareTag("Untagged"))
                {
                    PhotonNetwork.Instantiate("Prototype/TID-33-charater-photon-test/Resources/Prefabs/TID_33_Doll", genPos[i], Quaternion.identity, 0);
                    return;
                }
                
            }
            
        }
        else if (role == RoleType.Exorcist)
        {
            PhotonNetwork.Instantiate("Prototype/TID-33-charater-photon-test/Resources/Prefabs/TID_33_Exorcist", genPos[0],Quaternion.identity,0);
        }
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
