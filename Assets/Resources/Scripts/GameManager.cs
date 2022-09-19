using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Public Fields
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject gameManagerObj = new GameObject("_GameManager");
                instance = gameManagerObj.AddComponent<GameManager>();
            }
            return instance;
        }
    }
    #endregion


    #region Private Fields
    static GameManager instance;
    #endregion


    #region MonoBehaviour Callbacks
    private void Start()
    {
        DontDestroyOnLoad( gameObject );
    }
    private void Update()
    {
	
    }
    #endregion


    #region Public Methods
    public void LoadScene(string sceneName)
    {
    }
    #endregion


    #region Private Methods
    #endregion
}
