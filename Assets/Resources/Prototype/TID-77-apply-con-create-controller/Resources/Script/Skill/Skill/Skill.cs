using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class Skill : MonoBehaviour
	{
		/*--- Public Fields ---*/
		public GameObject skillArea;

		/*--- Protected Fields ---*/
		protected SP_CrossObs SP_crossObs = new SP_CrossObs();
		protected SP_Detected SP_detected = new SP_Detected();


		protected List<SkillPart> skillParts = new List<SkillPart>();

		/*--- Private Fields ---*/



		/*--- Public Methods ---*/
		public void SetArea(GameObject Area)
		{
			skillArea = Area;
		}

		public virtual void Excute()
		{
			foreach (var skillpart in skillParts)
			{
				skillpart.Excute();
			}
		}

		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
	}
}