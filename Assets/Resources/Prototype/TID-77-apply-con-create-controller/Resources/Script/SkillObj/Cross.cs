using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Object;
using Photon.Pun;

namespace GHJ_Lib
{
	public class Cross: GaugedObj
    {
		/*--- Public Fields ---*/
        // 십자가의 게이지 존재.
        // 게이지가 다 차면 비활성화.
        // 인형은 상호작용을 통해 비활성화 가능
        // 조건 :보고있는방향 + 범위
        // 퇴마사는 설치와 비활성화된 십자가를 회수 가능
        // 조건 : 보고있는방향 + 범위 + 비활성화

		/*--- Protected Fields ---*/
		[SerializeField] protected float reductionGauge = 1.0f;
        [SerializeField] protected float increaseGauge = 5.0f;
        [SerializeField] protected float OnStackDistance = 5.0f;

        [SerializeField] protected GameObject EffectSphere;
        public bool IsEnable = false;
        
        private float inverseMaxGauge;
        /*--- Private Fields ---*/
        protected override void OnEnable()
        {
            base.OnEnable();
            MaxGauge = 60.0f;
        }
        protected void Update()
        {
            EffectSphere.SetActive(IsEnable);
            if (!photonView.IsMine)
            {
                if (IsEnable&& RateOfGauge <= 0.0f)
                {
                    RateOfGauge = 0.0f;
                    IsEnable = false;
                }
                return;
            }
            if (IsEnable)
            {
                float curGage = (RateOfGauge - reductionGauge * inverseMaxGauge * Time.deltaTime);
                if (curGage <= 0.0f)
                {
                    curGage = 0.0f;
                    IsEnable = false;
                }
                SyncGauge(curGage);
            }
            
        }
        public void SetGauge(float gauge)
        {
            SyncGauge(gauge / MaxGauge);
            IsEnable = true;
            inverseMaxGauge = 1 / MaxGauge;
        }

        /*
        IEnumerator reductGauge()
        {
            while (true)
            {
                curHolyGauge -= reductionGauge;
                yield return new WaitForSeconds(timer);
                if (curHolyGauge <= 0.0f)
                {
                    DisableCross();
                    break;
                }
            }
        }
        */
        /*
        private void DisableCross()
        {
            // 외형변화
            curHolyGauge = 0.0f;
        }
        */


        protected override bool CheckAdditionalCondition(in InteractionPromptUI promptUI)
        {
            if (targetController is ExorcistController)
            {
                return false;
            }
            else if (targetController is DollController)
            { 
                return IsEnable;
            }
            return false;
        }

        bool DollRunningCondition()
        {
            return targetController.IsInteractionKeyHold()&&RateOfGauge>0.0f;
        }
        void DollPauseAction()
        {
            targetController.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Idle);
        }
        void DollFinishAction()
        {
            targetController.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Idle);
            IsEnable = false;
        }
        void DollRunningAction()
        {
            
        }
        public override bool Interact(Interactor interactor)
        {
            //targetController.InteractType = GaugedObjType.Cross;
            targetController.SetInteractType(GaugedObjType.Cross);
            targetController.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Interact);
            if (targetController.gameObject.CompareTag(GameManager.DollTag))
            {
                castingSystem.ForceSetRatioTo(RateOfGauge);
                castingSystem.StartReverseCasting(CastingSystem.Cast.CreateByRatio(deltaRatio: -DataManager.Instance.LocalPlayerData.roleData.InteractionSpeed/MaxGauge, destRatio: 0.0f, coolTime: CoolTime),
                    new CastingSystem.CastFuncSet(SyncDataWith: SyncGauge,RunningCondition: DollRunningCondition, RunningAction : DollRunningAction, PauseAction: DollPauseAction,FinishAction: DollFinishAction)
                    );
                return true;
            }
            else if (targetController.gameObject.CompareTag(GameManager.ExorcistTag))
            {
            }
            else
            {
                Debug.LogError("NormalAltar.Interact: No Interact Target Tags, Please check target's interactor tag");
            }
            return false;
        }
    }
}