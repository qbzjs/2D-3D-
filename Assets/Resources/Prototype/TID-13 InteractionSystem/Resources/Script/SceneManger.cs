using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManger : MonoBehaviour
{
    #region Public Fields
    public static SceneManger Instance;
    #endregion

    #region Private Fields
    private List<GameObject> objList;
    #endregion

    #region MonoBehaviour CallBacks
    void Start()
    {
        Instance = this;

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
