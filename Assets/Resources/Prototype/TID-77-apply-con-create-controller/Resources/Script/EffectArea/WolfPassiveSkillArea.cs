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
        bool isEnabled = false;
        private void OnEnable()
        {
            isEnabled = true;
        }
        protected override void OnTriggerEnter(Collider other)
        {
            if (!isEnabled)
            {
                return;
            }

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
                if (targetDoll.photonView == this.photonView)
                {
                    DataManager.Instance.LocalPlayerData.roleData.InteractionSpeed = DataManager.Instance.RoleInfos[8].InteractionSpeed * rate;
                }
                else
                { 
                    int targetPlayerIndex = targetDoll.PlayerIndex;
                    int targetTypeIndex = targetDoll.TypeIndex;
                    DataManager.Instance.LocalPlayerData.roleData.InteractionSpeed = DataManager.Instance.RoleInfos[targetTypeIndex].InteractionSpeed * rate;
                }
                DataManager.Instance.ShareRoleData();
            }
        }
    }
}