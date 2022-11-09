using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class Sk_CollectCross: Behavior<NetworkBaseController>
	{
		protected BishopSkill bishopskill;

		protected override void Activate(in NetworkBaseController actor)
		{
			if (bishopskill == null)
			{
				bishopskill = (actor.skill as BishopSkill);
			}
			//Controller 에서 십자가를 보고있는지, 가까이 있는지 판단 후 들어올 스킬
			// 위 조건에 맞는 십자가를 제거 -> stageManager 를 불러서 해결
			//StageManager.Instance.DestroyObj(bishop.Cross)
			actor.BaseAnimator.Play("install Cross");
		}
	}
}