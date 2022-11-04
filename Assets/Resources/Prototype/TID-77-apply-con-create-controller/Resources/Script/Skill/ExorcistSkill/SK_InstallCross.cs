using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class Sk_InstallCross: Behavior<NetworkBaseController>
	{
		protected BishopController bishop;
		
		protected override void Activate(in NetworkBaseController actor)
        {
			if (bishop == null)
			{
				bishop = (actor as BishopController);
			}
			if (bishop.InstallCross >= bishop.CrossMaxCount ) // �ֺ��� ���ڰ��� ���� ��( �ֺ������� 4) ��ġ �Ұ� -> interact������ ���ٰ�.
			{
				return;
			}
			else
			{
				actor.BaseAnimator.Play("install Cross");
				bishop.InstallCross++;
			}
		}

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            return base.DoBehavior(actor);
        }
		private Cross SetCross(BishopController bishop)
		{
			GameObject CrossObj = GameObject.Instantiate(bishop.CrossPrefab, bishop.transform);
			
			return CrossObj.GetComponent<Cross>();
		}
		
	}
}