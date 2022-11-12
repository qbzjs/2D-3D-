using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;

using LSH_Lib;
namespace GHJ_Lib
{
    public class HunterSkill : ExorcistSkill
    {
        public struct PosRot
        {
            public Vector3 position;
            public Quaternion rotation;

            public PosRot(Vector3 pos, Quaternion rot)
            {
                position = pos;
                rotation = rot;
            }
        }

        public GameObject TrapPrefab;
        public GameObject CrowPrefab;
        protected List<Crow> Crows = new List<Crow>();
        protected Sk_Default sk_Default = new Sk_Default();
        Transform[] crowGenPos;
        protected Sk_InstallTrap sk_InstallTrap = new Sk_InstallTrap();
        protected Sk_CollectTrap sk_CollectTrap = new Sk_CollectTrap();
        protected RandomGenerator<int> crowRandomGenerator = new RandomGenerator<int>();

        public int TrapCount { get; protected set; }
        public string TrapName { get; protected set; }
        public bool isUse { get; protected set; }

        protected override void OnEnable()
        {
            isUse = false;
            base.OnEnable();
            SettingCrowGenPosIdx();
            TrapCount = 5;
            Controller.AllocSkill(new BvHunterActSkill());
            TrapName = "Trap";
            PhotonNetwork.PrefabPool.RegisterPrefab(TrapName, TrapPrefab);
            if (photonView.IsMine)
            { 
                StartCoroutine(HunterPassiveSkill());
            }
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
            ClearCrowTo_RPC();
            RandomSpawnCrows(2);

            yield return new WaitForSeconds(15.0f);
            ClearCrowTo_RPC();
            RandomSpawnCrows(3);

            yield return new WaitForSeconds(20.0f);
            ClearCrowTo_RPC();
            RandomSpawnCrows(4);
        }
        protected void SettingCrowGenPosIdx()
        {
            crowGenPos = StageManager.Instance.CrowGenPos;
            for (int i = 0; i < crowGenPos.Length; i++)
            {
                crowRandomGenerator.Add(i);
            }
        }

        protected void RandomSpawnCrows(int num)
        {
            int[] UsedIdx = new int[num];   

            for (int i = 0; i < num; i++)
            {
                int idx = crowRandomGenerator.GetAndRemoveItem();
                UsedIdx[i] = idx;
                photonView.RPC("InstanceCrow", RpcTarget.AllViaServer, idx);
            }

            for (int i = 0; i < UsedIdx.Length; ++i)
            {
                crowRandomGenerator.Add(UsedIdx[i]);
            }
            
        }
        public void ClearCrowTo_RPC()
        {
            photonView.RPC("ClearCrows",RpcTarget.AllViaServer);
        }
        [PunRPC]
        public void InstanceCrow(int idx)
        {
            GameObject crowObj = Instantiate(CrowPrefab, crowGenPos[idx]);
            Crow crow = crowObj.GetComponent<Crow>();
            Crows.Add(crow);
            crow.SetOwner(this);
        }
        [PunRPC]
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
                StageManager.CharacterLayerChange(target, LayerMask.NameToLayer(GameManager.RendOnTopLayer));
            }
            yield return new WaitForSeconds(20.0f);
            StageManager.CharacterLayerChange(target, LayerMask.NameToLayer(GameManager.PlayerLayer));
            yield return new WaitForSeconds(30.0f);
            doll.IsCrowDebuff = false;
        }
    }
}