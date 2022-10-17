using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib{
    public class InteractDollandBox : MonoBehaviour
    {
        string playerTag;
        BoxController boxController;
        private void Start()
        {
            boxController = GetComponent<BoxController>();
        }
        private void OnTriggerStay(Collider other)
        {
            playerTag = other.ToString();
            if(playerTag == "Doll")
            {
                if(Input.GetKey(KeyCode.Mouse0))
                {
                    boxController.DoInteraction();
                }
            }
        }
    }
}
