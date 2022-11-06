using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSH_Lib.Object
{
    public interface IInteractable
    {
        public string InteractionPrompt { get; }

        public bool Interact(Interactor interactor);
    }
}
