using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Object;

namespace GHJ_Lib
{
	public class Cross: GaugedObj
    {
		/*--- Public Fields ---*/
        // ���ڰ��� ������ ����.
        // �������� �� ���� ��Ȱ��ȭ.
        // ������ ��ȣ�ۿ��� ���� ��Ȱ��ȭ ����
        // ���� :�����ִ¹��� + ����
        // �𸶻�� ��ġ�� ��Ȱ��ȭ�� ���ڰ��� ȸ�� ����
        // ���� : �����ִ¹��� + ���� + ��Ȱ��ȭ

		/*--- Protected Fields ---*/
		protected float reductionGauge = 1.0f;
		protected float increaseGauge = 5.0f;

        protected float timer = 1.0f;
        /*--- Private Fields ---*/
        protected override void OnEnable()
        {
            base.OnEnable();
            MaxGauge = 60.0f;
        }
        public void SetGauge(float gauge)
        {
            SyncGauge(gauge / MaxGauge);
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
        private void OnTriggerEnter(Collider other)
        {
            if (curHolyGauge < 0.0f)
            {
                return;
            }

            if (Vector3.Distance(transform.position, other.transform.position) > 5)
            {
                //���ڰ��� ��ȣ�ۿ밡��
                return;
            }
        

            if (other.CompareTag("Doll"))
            {
                DisableCross();
                other.GetComponent<DollController>().AprrochCrossArea();
            }
        }

        private void DisableCross()
        {
            // ������ȭ
            curHolyGauge = 0.0f;
        }
        */


        public override bool Interact(Interactor interactor)
        {
            throw new System.NotImplementedException();
        }
    }
}