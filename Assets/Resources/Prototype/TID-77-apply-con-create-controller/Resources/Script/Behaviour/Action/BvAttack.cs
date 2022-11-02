using KSH_Lib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class BvAttack: Behavior<NetworkBaseController>
    {
        const float animationEndPoint = 0.9f;
        protected override void Activate(in NetworkBaseController actor)
        {
            PlayAnimation( actor );

            ( actor as ExorcistController).AttackBox.gameObject.SetActive(true);

            actor.ChangeMoveFunc(false);
        }
        
        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            if (actor.BaseAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime>= animationEndPoint )
            {
                (actor as ExorcistController).AttackBox.gameObject.SetActive(false);
                return new BvIdle();
            }
            return null;
        }

        void PlayAnimation( in NetworkBaseController actor )
        {
            if ( actor.BaseAnimator.GetCurrentAnimatorStateInfo( 0 ).IsName( "Attack" ) )
            {
                return;
            }
            actor.BaseAnimator.Play( "Attack" );
        }
    }
}