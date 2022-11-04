using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Data;
namespace GHJ_Lib
{
	public class WolfPassiveSkillArea: EffectArea
	{
        protected float PassiveRate = 1.05f;
        protected override void OnTriggerEnter(Collider other)
        {
            var target = FindTargets(other);
            if (target == null)
            {
                return;
            }
            if (!targets.Contains(target))
            {
                targets.Add(target);
                ChangeInteractSpeed(target.gameObject.GetComponent<DollController>(), PassiveRate);
            }
        }
        protected override void OnTriggerExit(Collider other)
        {
            if (targets.Remove(FindTargets(other)))
            {
                ChangeInteractSpeed(other.gameObject.GetComponent<DollController>(), 1.0f);
            }
        }
        protected override GameObject FindTargets(Collider other)
        {
            if (other.gameObject.CompareTag(GameManager.DollTag))
            {
                return other.gameObject;
            }
            return null; 
        }
        private void ChangeInteractSpeed(DollController targetDoll,float rate)
        {
            if (targetDoll.photonView.IsMine)
            {
                int targetPlayerIndex = targetDoll.PlayerIndex;
                int targetTypeIndex = targetDoll.TypeIndex;
                DataManager.Instance.PlayerDatas[targetPlayerIndex].roleData.InteractionSpeed = DataManager.Instance.RoleInfos[targetTypeIndex].InteractionSpeed * rate;
            }
        }
    }
}