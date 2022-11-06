using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using KSH_Lib.Object;

namespace KSH_Lib.Test
{
    public class TestInteractionDoor : MonoBehaviour, IInteractable
    {
        [SerializeField] string prompt;
        string IInteractable.InteractionPrompt => prompt;

        public bool Interact(Interactor interactor)
        {
            Debug.Log("Interact With Test Door");
            return true;
        }
    }

}
