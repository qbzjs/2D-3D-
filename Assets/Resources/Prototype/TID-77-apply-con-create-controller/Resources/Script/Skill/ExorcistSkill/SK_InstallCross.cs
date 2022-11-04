using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class SK_InstallCross: Behavior<NetworkBaseController>
	{
		protected GameObject crossPrefab;
		protected List<GameObject> installcrosses = new List<GameObject>();
		protected Cross searchedCollectCross = null;
		protected int maxCrossCount = 5;
		protected float CollectRadius = 4.0f;
		/*
		protected override void Activate(in NetworkBaseController actor)
        {
			if (crossPrefab == null)
			{
				crossPrefab = (actor as BishopController).CrossPrefab;
			}

			actor.BaseAnimator.Play("install Cross");
			if (actor.photonView.IsMine)
			{ 
				if (SearchCross(actor))
				{
					installcrosses.Remove(searchedCollectCross);
				}
				else
				{
					if (installcrosses.Count > 5)
					{
						return;
					}
					
					//¼³Ä¡

				}
			}

		}

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            return base.DoBehavior(actor);
        }


		private GameObject SetCross(Transform transform)
		{
			GameObject CrossObj = GameObject.Instantiate(crossPrefab, transform);
			return CrossObj;
		}


		
		private bool SearchCross(in NetworkBaseController actor)
		{
			List<Cross> searchedCrosses = new List<Cross>();
			foreach (var cross in installcrosses)
			{
				RaycastHit[] hits;
				Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width, Screen.height) / 2);

				float maxDist = 10.0f;

				hits = Physics.RaycastAll(ray, maxDist);
				foreach (var hit in hits)
				{
					if (installcrosses.Contains(hit.collider.gameObject))
					{
						return true;
					}
				}
				return false;







				
				if (Physics.RaycastAll(ray, out hits))
				{
					Transform objectHit = hit.transform;

				if ((cross.transform.position - actor.transform.position).magnitude < CollectRadius)
				{
					searchedCrosses.Add(cross);
				}
				
			}

			if (searchedCrosses.Count == 0)
			{
				searchedCollectCross = null;
				return false;
			}

			
			
		}
	
		private void FindNearestCross(List<Cross> crosses)
		{
			
		}

		protected IEnumerator installCross()
		{
			

			yield return new WaitForSeconds(2.0f);//¼±µô
			GameObject crossObj = Instantiate(CrossPrefab, new Vector3(transform.position.x, 0, transform.position.z) + forward * 10.0f, transform.rotation); ;
			Cross cross = crossObj.GetComponent<Cross>();
			installcrosses.Add(cross);
			yield return new WaitForSeconds(2.0f);
			CurBehavior = idle;
			yield return new WaitForSeconds(1.0f);
			
		}

		protected IEnumerator CollectCross()
		{
			
			CurBehavior.PushSuccessorState(actSkill);
			yield return new WaitForSeconds(2.0f);//¼±µô
			installcrosses.Remove(canCollectCross);
			Destroy(canCollectCross.gameObject);
			yield return new WaitForSeconds(2.0f);
			CurBehavior = idle;
			yield return new WaitForSeconds(1.0f);
			
		}
			*/
	}
}