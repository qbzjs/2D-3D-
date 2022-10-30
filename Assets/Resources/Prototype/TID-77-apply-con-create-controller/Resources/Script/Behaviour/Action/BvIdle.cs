using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class BvIdle: Behavior<NetworkBaseController>
	{
        /*--- Public Fields ---*/


        /*--- Protected Fields ---*/


        /*--- Private Fields ---*/


        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/
        protected override void Activate(in NetworkBaseController actor)
        {
            if (actor is DollController)
            {
                actor.BaseAnimator.Play("Idle_A");
                
            }

            if (actor is ExorcistController)
            {
                actor.BaseAnimator.Play("Idle");
            }


            actor.SetMoveInput(true);
            
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            if (actor.photonView.IsMine)
            { 
                actor.DoSkill();
                if (actor is DollController)
                {
                    actor.DoSprint();
                
                    if (actor.CanInteract)
                    {
                        actor.DoInteract();
                    }
                    else
                    {
                    
                    }
                }

                if (actor is ExorcistController)
                {
                    ExorcistController exorcist = (actor as ExorcistController);

                    if (actor.CanInteract)
                    {
                        actor.DoInteract();
                    }
                    else
                    {
                        actor.DoAttack();
                    }
                    if (exorcist.pickUpBox.CanPickUp())
                    {
                        exorcist.DoPickUp();
                    }

                }
            }


            return PassIfHasSuccessor();
        }






        /*--- Private Methods ---*/
    }
}