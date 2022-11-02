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
		protected Cross canCollectCross = null;
		protected List<Cross> installcrosses = new List<Cross>();
		protected int maxCrossCount = 5;
		protected float CollectRadius = 4.0f;


		protected Sk_Default skDefault = new Sk_Default();
		protected SK_InstallCross skInstallCross = new SK_InstallCross();

		protected List<Behavior<NetworkBaseController>> BishopSkill = new List<Behavior<NetworkBaseController>>();





		/*--- Private Fields ---*/


		/*--- MonoBehaviour Callbacks ---*/
		public override void OnEnable()
		{
			base.OnEnable();
			SkillSetting();
		}

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

		protected override IEnumerator ActiveSkillBox()
		{
			useActiveSkill = true;
			//½ºÅ³Áß
			yield return new WaitForSeconds(0.2f);//¼±µô

			while (ActiveSkill.Count != 0)
			{
				ActiveSkill.Update(this, ref ActiveSkill);
			}
			SkillSetting();
			yield return new WaitForSeconds(0.2f);//ÈÄµô
												  //½ºÅ³³¡
			yield return new WaitForSeconds(14.6f);
			useActiveSkill = false;
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
			CurBehavior.PushSuccessorState(actSkill);
			yield return new WaitForSeconds(2.0f);//¼±µô
			GameObject crossObj = Instantiate(CrossPrefab, new Vector3(transform.position.x, 0, transform.position.z) + forward*10.0f, transform.rotation); ;
			Cross cross = crossObj.GetComponent<Cross>();
			installcrosses.Add(cross);
			yield return new WaitForSeconds(2.0f);
			CurBehavior = idle;
			yield return new WaitForSeconds(1.0f);
			useActiveSkill = false;
		}

		protected IEnumerator CollectCross()
		{
			useActiveSkill = true;
			CurBehavior.PushSuccessorState(actSkill);
			yield return new WaitForSeconds(2.0f);//¼±µô
			installcrosses.Remove(canCollectCross);
			Destroy(canCollectCross.gameObject);
			yield return new WaitForSeconds(2.0f);
			CurBehavior = idle;
			yield return new WaitForSeconds(1.0f);
			useActiveSkill = false;
		}

		/*--- Public Methods ---*/


		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
		private void SkillSetting()
		{
			BishopSkill.Add(skInstallCross);
			BishopSkill.Add(skDefault);
			ActiveSkill.PushSuccessorStates(BishopSkill);
		}
	}
}