using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ResultUIManager : MonoBehaviour
{
    [SerializeField]
    string NextSceneName;
    public void ReturnToHome()
    {
        SceneManager.LoadScene(NextSceneName);
    }
}
