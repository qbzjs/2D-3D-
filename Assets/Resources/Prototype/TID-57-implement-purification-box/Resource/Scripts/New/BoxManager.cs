using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TID42;
using GHJ_Lib;

namespace LSH_Lib
{
    public class BoxManager : MonoBehaviour
    {
        public FPV_CharacterController1 Exorcist
        {
            get
            {
                if( exorcist == null)
                {
                    GameObject exor = GameObject.FindGameObjectWithTag("Exorcist");
                    exorcist = exor.GetComponent<FPV_CharacterController1>();
                }
                return exorcist;
            }
        }
        public NetworkTPV_CharacterController Doll
        {
            get
            {
                if (doll == null)
                {
                    GameObject[] dolls = GameObject.FindGameObjectsWithTag("Doll");
                    foreach(var d in dolls)
                    {
                        NetworkTPV_CharacterController controller = d.GetComponent<NetworkTPV_CharacterController>();
                        if(controller.CurBehavior is BvGrabbed)
                        {
                            doll = controller;
                            break;
                        }
                    }
                }
                return doll;
            }
        }
        FPV_CharacterController1 exorcist;
        NetworkTPV_CharacterController doll;


    }
}
