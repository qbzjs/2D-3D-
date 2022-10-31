using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSH_Lib
{
    public interface IInteractable
    {
        public bool InteractCondition();
        public void Interact();
    }
}