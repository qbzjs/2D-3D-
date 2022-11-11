using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;

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
            actor.BaseAnimator.Play("Skill");
            effectArea = actor.skill.actSkillArea;
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
           
            if (effectArea.CanGetTarget())
            {
               effectArea.Targets[0].GetComponent<ExorcistController>().DoActionBy(DetectedTo_RPC);
            }
            return PassIfHasSuccessor();

        }

        void DetectedTo_RPC(PhotonView TargetPhotonView)
        {
            if (TargetPhotonView.IsMine)
            {
                TargetPhotonView.RPC("Detected", RpcTarget.Others);
            }
        }

        [PunRPC]
        IEnumerator Detected(GameObject characterModel)
        {
            StageManager.CharacterLayerChange(characterModel, 6); //6 : �����°�
            yield return new WaitForSeconds(5);//�ð��� CSV�� ������ �Ǵ� �������� ���Ƿ� 5�� �س���
            StageManager.CharacterLayerChange(characterModel, 7); //7: �������·ε��ƿ�
        }

        /*--- Private Methods ---*/




    }
}