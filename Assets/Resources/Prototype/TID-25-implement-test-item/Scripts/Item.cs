using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public abstract class Item : MonoBehaviour
	{

        virtual protected void OnTriggerEnter(Collider other){}
        public abstract void DoAction();
    }
}
