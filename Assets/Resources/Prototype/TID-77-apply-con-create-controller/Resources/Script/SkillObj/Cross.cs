using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class Cross: MonoBehaviour
    {
		/*--- Public Fields ---*/
        // ���ڰ��� ������ ����.
        // �������� �� ���� ��Ȱ��ȭ.
        // ������ ��ȣ�ۿ��� ���� ��Ȱ��ȭ ����
        // ���� :�����ִ¹��� + ����
        // �𸶻�� ��ġ�� ��Ȱ��ȭ�� ���ڰ��� ȸ�� ����
        // ���� : �����ִ¹��� + ���� + ��Ȱ��ȭ

		/*--- Protected Fields ---*/
		protected float maxHolyGauge=60.0f;
		protected float curHolyGauge=60.0f;
		protected float reductionGauge = 1.0f;
		protected float increaseGauge = 5.0f;

        protected float timer = 1.0f;
        /*--- Private Fields ---*/

        void OnEnable()
        {
            StartCoroutine(reductGauge());   
        }

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
    }
}