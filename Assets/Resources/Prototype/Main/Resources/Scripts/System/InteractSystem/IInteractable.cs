using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSH_Lib.Object
{
    public interface IInteractable
    {
        public string InteractionPrompt { get; }
        public bool CanInteract { get; }

        public bool ActiveInteractPrompt( Interactor interactor, InteractionPromptUI promptUI );
        public bool Interact(Interactor interactor);
    }
}
