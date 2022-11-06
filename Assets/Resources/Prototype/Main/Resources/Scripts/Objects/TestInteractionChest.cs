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
        public bool IsInteractNow { get => castingSystem.IsCoroutineRunning; }
        public bool Interact(Interactor interactor)
        {
            castingSystem.StartCasting(CastingSystem.Cast.CreateByTime(1.0f, coolTime: 1.0f), new CastingSystem.CastFuncSet(FinishAction: InteractAction));

            return true;
        }

        void InteractAction()
        {
            Debug.Log("Interact With Test Chest");
        }
    }
}