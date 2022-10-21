using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class Item : MonoBehaviour
	{
        virtual protected void OnTriggerEnter(Collider other){}
        virtual protected void DoAction() { }
    }
}
