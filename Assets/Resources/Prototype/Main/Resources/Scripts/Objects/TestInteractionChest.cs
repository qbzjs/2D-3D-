using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using KSH_Lib.Object;

namespace KSH_Lib.Test
{
    public class TestInteractionChest : MonoBehaviour, IInteractable
    {
        [SerializeField] string prompt;
        [SerializeField] CastingSystem castingSystem;

        public string InteractionPrompt => prompt;


        public bool Interact(Interactor interactor)
        {
            Debug.Log("Interact With Test Chest");
            return true;
        }

        IEnumerator InteractWhenFinishCasting()
        {
            while(!castingSystem.IsFinshCasting)
            {
                yield return null;
            }
            
            
        }
    }
}