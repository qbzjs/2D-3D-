using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
namespace GHJ_Lib
{
	public class HunterSkill: ExorcistSkill
	{
		public GameObject TrapPrefab;
		public int TrapCount { get; protected set; }
        public string TrapName { get; protected set; }
        public bool isUse { get; protected set; }

        protected override void OnEnable()
        {
            isUse = false;
            base.OnEnable();
            TrapCount = 5;
            Controller.AllocSkill(new BvHunterActSkill());
            TrapName = "Trap";
            PhotonNetwork.PrefabPool.RegisterPrefab(TrapName, TrapPrefab);
        }
        public void InstallTrap()
        {
            TrapCount--;
            isUse = false;
        }
        public void Installfail()
        {
            Debug.LogWarning("InstallFail");
            isUse = false;
            Debug.LogWarning("isUse :" + isUse);
        }
        public void CollectTrap(GameObject gameObject)
        {
            TrapCount++;
            isUse = false;
            PhotonNetwork.Destroy(gameObject);
        }
        public override bool CanActiveSkill() // NetworkBaseController 를 통해 Curbehvior를 바꿀지 말지.. 또는 스킬을 사용하기위한조건이 있다면 여기서 작성
        {
            return false;
        }

        public bool IsSkillKeyHold()
        {
            return Input.GetKey(KeyCode.Mouse1);
        }
        protected override IEnumerator ExcuteActiveSkill()
        {
            if (isUse)
            {
                yield break;
            }
            isUse = true;
            Debug.LogWarning("isUse :" + isUse);
            while (true)
            {
                Debug.Log("ExcuteActiveSkill");
                Debug.LogWarning("isUse :" + isUse);
                yield return new WaitForEndOfFrame();
                if (!isUse)
                {
                    Controller.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Idle);
                    break;
                }
            }
        }
    }
}