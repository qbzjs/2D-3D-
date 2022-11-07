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

        public float RateOfGauge;

        public string InteractionPrompt => prompt;
        public bool IsInteractNow { get => castingSystem.IsCoroutineRunning; }
        public bool Interact(Interactor interactor)
        {
            //castingSystem.StartCasting(CastingSystem.Cast.CreateByTime(1.0f, coolTime: 1.0f), new CastingSystem.CastFuncSet(FinishAction: InteractAction));

            castingSystem.ForceSetRatioTo( RateOfGauge );
            castingSystem.StartCasting( CastingSystem.Cast.CreateByRatio( 0.2f, coolTime: 1.0f ),
                new CastingSystem.CastFuncSet( SyncGauge, IsKeyHold, PauseAction, InteractAction ) );

            return true;
        }

        void SyncGauge(float val)
        {
            RateOfGauge = val;
        }

        bool IsKeyHold()
        {
            return Input.GetKey( KeyCode.G );
        }
        void PauseAction()
        {
            Debug.Log( "Interact Paused" );
        }

        void InteractAction()
        {
            Debug.Log("Interact With Test Chest");
        }

        public bool ActiveInteractPrompt( Interactor interactor, InteractionPromptUI promptUI )
        {
            if(IsInteractNow)
            {
                promptUI.Inactivate();
                return false;
            }
            promptUI.Activate(prompt);
            return true;
        }
    }
}