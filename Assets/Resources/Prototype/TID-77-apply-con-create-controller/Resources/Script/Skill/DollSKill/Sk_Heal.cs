using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
using KSH_Lib.Data;

namespace GHJ_Lib
{
	public class Sk_Heal : Behavior<NetworkBaseController>
	{
        protected EffectArea effectArea;
        protected RabbitSkill rabbit;
        GameObject healTarget;
        DollController peer=null;
        protected override void Activate(in NetworkBaseController actor)
        {
            if (effectArea == null)
            {
                effectArea = actor.skill.actSkillArea;
            }

            if (rabbit == null)
            {
                rabbit = actor.skill as RabbitSkill;
            }

            healTarget = effectArea.GetNearestTarget();
            if (!healTarget)
            {
                Debug.LogError("Sk_Heal.DoBehavior : healTarget is null ... why actSkillArea.CanGetTarget() is true???");
                return;
            }


            peer = healTarget.GetComponent<DollController>();
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            if (!peer)
            {
                Debug.LogError("Sk_Heal.DoBehavior : peer is null ... why actSkillArea.CanGetTarget() is true???");
                return PassIfHasSuccessor();
            }


            if (peer.photonView.IsMine)
            {
                DollData peerData = (DataManager.Instance.LocalPlayerData.roleData as DollData);
                peerData.DollHP += rabbit.HealAmount * Time.deltaTime;
                DollData peerInitDate = (DataManager.Instance.RoleInfos[peer.TypeIndex] as DollData);
                
                if (peerInitDate.DollHP <= peerData.DollHP)
                {
                    peerData.DollHP = peerInitDate.DollHP;
                    rabbit.CancelHeal();
                }
                DataManager.Instance.ShareRoleData();
            }

            if (actor.photonView.IsMine)
            {
                if (!Input.GetKey(KeyCode.Mouse1) && rabbit.IsHeal)
                {
                    rabbit.CancelHeal();
                }
            }

            if (rabbit.IsHeal)
            {
                return null;
            }

            return PassIfHasSuccessor();

        }
            
    }
}