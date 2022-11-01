using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
using Photon.Realtime;

namespace GHJ_Lib
{
	public class BishopController: ExorcistController
	{
		/*--- Public Fields ---*/
		public GameObject CrossPrefab;

		/*--- Protected Fields ---*/
		

		protected int maxCrossCount = 5;
		protected float CollectRadius = 4.0f;

		protected Cross canCollectCross = null;
		protected List<Cross> installcrosses = new List<Cross>();
		/*--- Private Fields ---*/


		/*--- MonoBehaviour Callbacks ---*/


		/*---Skill---*/
		[PunRPC]
		public override void DoActiveSkill()
		{
			if (installcrosses.Count > 5)
			{
				return;
			}

			if (GetCanCollectCross())
			{
				StartCoroutine("CollectCross");
			}
			else
			{
				StartCoroutine("installCross");
			}

		}

		protected Cross GetCanCollectCross()
		{
			foreach (var cross in installcrosses)
			{
				if((cross.transform.position - this.transform.position).magnitude < CollectRadius)
				{
					return cross;
				}
			}
			return null;
		}

		protected IEnumerator installCross()
		{
			useActiveSkill = true;
			CurCharacterAction.PushSuccessorState(actSkill);
			yield return new WaitForSeconds(2.0f);//¼±µô
			GameObject crossObj = Instantiate(CrossPrefab, new Vector3(transform.position.x, 0, transform.position.z) + forward*10.0f, transform.rotation); ;
			Cross cross = crossObj.GetComponent<Cross>();
			installcrosses.Add(cross);
			yield return new WaitForSeconds(2.0f);
			CurCharacterAction = idle;
			yield return new WaitForSeconds(1.0f);
			useActiveSkill = false;
		}

		protected IEnumerator CollectCross()
		{
			useActiveSkill = true;
			CurCharacterAction.PushSuccessorState(actSkill);
			yield return new WaitForSeconds(2.0f);//¼±µô
			installcrosses.Remove(canCollectCross);
			Destroy(canCollectCross.gameObject);
			yield return new WaitForSeconds(2.0f);
			CurCharacterAction = idle;
			yield return new WaitForSeconds(1.0f);
			useActiveSkill = false;
		}

		/*--- Public Methods ---*/


		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
	}
}