using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
	public class Sk_Detected: Behavior<NetworkBaseController>
	{
        /*--- Public Fields ---*/


        /*--- Protected Fields ---*/
        protected EffectArea effectArea;

        /*--- Private Fields ---*/


        /*--- Public Methods ---*/

        /*--- Protected Methods ---*/
        protected override void Activate(in NetworkBaseController actor)
        {
            effectArea = actor.actSkillArea;
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
           
            if (effectArea.CanGetTarget())
            {
               effectArea.Targets[0].GetComponent<ExorcistController>().DoActionBy(Detected);
            }
            return PassIfHasSuccessor();

        }


        IEnumerator Detected(GameObject characterModel)
        {
            StageManager.CharacterLayerChange(characterModel, 6); //6 : 빛나는거
            yield return new WaitForSeconds(5);//시간은 CSV로 받을것 또는 문서참조 임의로 5로 해놓음
            StageManager.CharacterLayerChange(characterModel, 7); //7: 원래상태로돌아옴
        }

        /*--- Private Methods ---*/




    }
}