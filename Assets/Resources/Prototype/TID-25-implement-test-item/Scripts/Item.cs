using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
    public abstract class Item : MonoBehaviour
    {
        public Item() { }
        public Item(ItemData data)
        {
            this.data = data;
        }

        ItemData data;
        virtual protected void DoAction(){ }
    }
}
