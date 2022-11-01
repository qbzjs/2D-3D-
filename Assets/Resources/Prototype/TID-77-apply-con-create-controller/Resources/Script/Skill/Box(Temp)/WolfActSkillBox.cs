using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class WolfActSkillBox: MonoBehaviour
	{
		/*--- Public Fields ---*/
        

		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/


		/*--- MonoBehaviour Callbacks ---*/

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Exorcist"))
            {
                ExorcistController exorcist = other.GetComponent<ExorcistController>();

                exorcist.HitSkillBy(WolfSkill);

            }
        }

        /*--- Public Methods ---*/

        void WolfSkill(GameObject characterModel)
        {
            StartCoroutine("WolfActSkill", characterModel);
        }

        IEnumerator WolfActSkill(GameObject characterModel)
        {
            NetworkBaseController.CharacterLayerChange(characterModel, 6); //6 : �����°�
            yield return new WaitForSeconds(5);//�ð��� CSV�� ������ �Ǵ� �������� ���Ƿ� 15�� �س���
            NetworkBaseController.CharacterLayerChange(characterModel, 7); //7: �������·ε��ƿ�
            this.gameObject.SetActive(false);
        }
        /*--- Protected Methods ---*/


        /*--- Private Methods ---*/
    }
}