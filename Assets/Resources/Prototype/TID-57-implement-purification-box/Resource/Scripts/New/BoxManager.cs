using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GHJ_Lib;

namespace LSH_Lib
{
    public class BoxManager : MonoBehaviour
    {
        public NetworkExorcistController Exorcist
        {
            get
            {
                if( exorcist == null)
                {
                    GameObject exor = GameObject.FindGameObjectWithTag("Exorcist");
                    exorcist = exor.GetComponent<NetworkExorcistController>();
                }
                return exorcist;
            }
        }
        public NetworkDollController Doll
        {
            get
            {
                if (doll == null)
                {
                    GameObject[] dolls = GameObject.FindGameObjectsWithTag("Doll");
                    foreach(var d in dolls)
                    {
                        NetworkDollController controller = d.GetComponent<NetworkDollController>();
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
        NetworkExorcistController exorcist;
        NetworkDollController doll;


    }
}
