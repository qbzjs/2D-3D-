using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
namespace GHJ_Lib
{
	public abstract class EffectArea: MonoBehaviourPun
	{
		public List<GameObject> Targets { get { return targets; } }
		protected List<GameObject> targets = new List<GameObject>();
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
			else
			{
				return true;
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