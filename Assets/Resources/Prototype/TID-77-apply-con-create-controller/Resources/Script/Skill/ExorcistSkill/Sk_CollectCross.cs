using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class Sk_CollectCross: Behavior<NetworkBaseController>
	{
		protected BishopController bishop;

		protected override void Activate(in NetworkBaseController actor)
		{
			if (bishop == null)
			{
				bishop = (actor as BishopController);
			}
			//Controller ���� ���ڰ��� �����ִ���, ������ �ִ��� �Ǵ� �� ���� ��ų
			// �� ���ǿ� �´� ���ڰ��� ���� -> stageManager �� �ҷ��� �ذ�
			//StageManager.Instance.DestroyObj(bishop.Cross)
			actor.BaseAnimator.Play("install Cross");
		}
	}
}