using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
    public abstract class Item : MonoBehaviour
    {
        ItemData data;

        virtual protected void Start()
        {
            InitItemData();
        }
        protected abstract void InitItemData();

        virtual protected void DoAction(){ }
    }
}
