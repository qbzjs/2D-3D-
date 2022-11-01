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
            NetworkBaseController.CharacterLayerChange(characterModel, 6); //6 : 빛나는거
            yield return new WaitForSeconds(5);//시간은 CSV로 받을것 또는 문서참조 임의로 15로 해놓음
            NetworkBaseController.CharacterLayerChange(characterModel, 7); //7: 원래상태로돌아옴
            this.gameObject.SetActive(false);
        }
        /*--- Protected Methods ---*/


        /*--- Private Methods ---*/
    }
}