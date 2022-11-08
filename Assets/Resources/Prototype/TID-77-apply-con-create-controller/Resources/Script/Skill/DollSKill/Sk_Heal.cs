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
        protected RabbitController rabbit;
        protected override void Activate(in NetworkBaseController actor)
        {
            if (effectArea == null)
            {
                effectArea = actor.actSkillArea;
            }

            if (rabbit == null)
            {
                rabbit = actor as RabbitController;
            }
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            GameObject target = effectArea.GetNearestTarget();
            
            if (target)
            {
                Debug.LogError("Sk_Heal.DoBehavior : target is null ... why actSkillArea.CanGetTarget() is true???");
                return new Sk_Default();
            }

            DollController peer = target.GetComponent<DollController>();
            if(peer.photonView.IsMine)
            {
                DollData peerData = (DataManager.Instance.LocalPlayerData.roleData as DollData);
                peerData.DollHP += rabbit.HealAmount * Time.deltaTime;
                DollData peerInitDate = (DataManager.Instance.RoleInfos[peer.TypeIndex] as DollData);
                if (peerInitDate.DollHP <= peerData.DollHP)
                {
                    peerData.DollHP = peerInitDate.DollHP;
                    rabbit.CancelHeal();
                    return PassIfHasSuccessor();
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