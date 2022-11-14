using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
namespace GHJ_Lib
{
	public abstract class EffectArea: MonoBehaviourPun
	{
		public List<GameObject> Targets { get { return targets; } }
		[SerializeField]protected List<GameObject> targets = new List<GameObject>();
		public GameObject GetNearestTarget()
		{
			GameObject nearestTarget = null;
			foreach (GameObject target in targets)
			{
				if (nearestTarget == null)
				{
					nearestTarget = target;
				}
				else
				{
					if ((this.transform.position - nearestTarget.transform.position).sqrMagnitude >
						(this.transform.position - target.transform.position).sqrMagnitude)
					{
						nearestTarget = target;
					}
				}
			}
			return nearestTarget;
		}

        public bool CanGetTarget()
		{
			if (targets.Count == 0)
			{
				return false;
			}

			foreach (GameObject target in targets)
			{
				if (target.activeInHierarchy)
				{
					return true;
				}
			}
			return false;
			
		}
		public void RemoveInList(GameObject gameObject)
		{
			if (Targets.Contains(gameObject))
			{
				if (!targets.Remove(gameObject))
				{
					Debug.LogError("EffectArea.RemoveInList : Targets Can't Remove gameObject");
				}
			}
			else
			{
				Debug.LogError("EffectArea.RemoveInList : Targets not Contain gameObject ");
			}
		}
		protected virtual void OnTriggerEnter(Collider other)
        {
			var target = FindTargets(other);
			if (target == null)
			{
				return;
			}
			if(!targets.Contains(target))
            {
				targets.Add(target);
			}
		}
		protected virtual void OnTriggerExit(Collider other)
		{
			targets.Remove(FindTargets(other));
		}
		protected abstract GameObject FindTargets(Collider other);
	}
}