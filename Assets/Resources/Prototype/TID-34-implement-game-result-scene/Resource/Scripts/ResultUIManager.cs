using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using KSH_Lib;
public class ResultUIManager : MonoBehaviour
{
    [SerializeField]
    string NextSceneName;
    public void ReturnToHome()
    {
        DataManager.Instance.ResetPlayerDatas();
        SceneManager.LoadScene(NextSceneName);
    }
}
