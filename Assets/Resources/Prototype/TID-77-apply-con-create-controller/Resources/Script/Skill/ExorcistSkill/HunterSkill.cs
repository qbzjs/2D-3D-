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
        public GameObject CrowPrefab;
        protected List<Crow> Crows = new List<Crow>();

        protected Sk_Default sk_Default = new Sk_Default();
        protected Sk_InstallTrap sk_InstallTrap = new Sk_InstallTrap();
        protected Sk_CollectTrap sk_CollectTrap = new Sk_CollectTrap();
            
        
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
            StartCoroutine(HunterPassiveSkill());
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
        public void Debuff(DollController dollController)
        {
            dollController.DoActionBy(DetectedDoll);
        }

        public void SettingToInstallTrap()
        {
            Controller.AllocSkill(sk_InstallTrap);
        }

        public void SettingToCollectTrap()
        {
            Controller.AllocSkill(sk_CollectTrap);
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
        protected IEnumerator HunterPassiveSkill()
        {
            yield return new WaitForSeconds(1.0f);
            RandomSpawnCrows(1);

            yield return new WaitForSeconds(10.0f);
            ClearCrows();
            RandomSpawnCrows(2);

            yield return new WaitForSeconds(15.0f);
            ClearCrows();
            RandomSpawnCrows(3);

            yield return new WaitForSeconds(20.0f);
            ClearCrows();
            RandomSpawnCrows(4);
        }
        protected void RandomSpawnCrows(int num)
        {
            Transform[] crowGenPos = StageManager.Instance.CrowGenPos;
            for (int i = 0; i < num; i++)
            {
                GameObject crowObj = Instantiate(CrowPrefab, crowGenPos[Random.Range(0, crowGenPos.Length)]);
                Crows.Add(crowObj.GetComponent<Crow>());
            }

            foreach (Crow crow in Crows)
            {
                crow.SetHunter(this);
            }
        }
        protected void ClearCrows()
        {
            foreach (Crow crow in Crows)
            {
                crow.Relocate();
            }
            Crows.Clear();
        }

        protected IEnumerator DetectedDoll(GameObject target)
        {
            DollController doll = target.transform.parent.GetComponent<DollController>();
            if (!doll)
            {
                Debug.LogError("Crow.DetectedDoll : target.transform.parent.GetComponent<DollController>() Cannot GetComponent");
            }
            doll.IsCrowDebuff = true;
            doll.CrowGauge = 0.0f;
            if (DataManager.Instance.PlayerIdx == 0)
            {
                StageManager.CharacterLayerChange(target, LayerMask.NameToLayer("RanderOnTop"));
            }
            yield return new WaitForSeconds(20.0f);
            StageManager.CharacterLayerChange(target, LayerMask.NameToLayer("Player"));
            yield return new WaitForSeconds(30.0f);
            doll.IsCrowDebuff = false;
        }
    }
}