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
        protected WolfController wolfController;

        /*--- Private Fields ---*/


        /*--- Public Methods ---*/

        /*--- Protected Methods ---*/
        protected override void Activate(in NetworkBaseController actor)
        {
            wolfController = (actor as WolfController);
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            ExorcistController exorcistController = wolfController.actSkillBox.OntriigerExorcist();
            if (exorcistController != null)
            {
               exorcistController.HitSkillBy(Detected);
            }
            return PassIfHasSuccessor();

        }


        IEnumerator Detected(GameObject characterModel)
        {
            StageManager.CharacterLayerChange(characterModel, 6); //6 : �����°�
            yield return new WaitForSeconds(5);//�ð��� CSV�� ������ �Ǵ� �������� ���Ƿ� 5�� �س���
            StageManager.CharacterLayerChange(characterModel, 7); //7: �������·ε��ƿ�
        }

        /*--- Private Methods ---*/




    }
}