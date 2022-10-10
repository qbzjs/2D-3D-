using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace LSH_Lib{
    public class UIController : MonoBehaviour
    {
        public TMP_Text text;
        private void Start()
        {
            text.enabled = false;
        }
        public void OnMouseClick()
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                Debug.Log("Up");
                text.enabled = false;
            }
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Debug.Log("down");
                text.enabled = true;
            }
            
        }
    }
}
