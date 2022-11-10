using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
namespace GHJ_Lib
{
	public abstract class BaseSkill : MonoBehaviourPun
	{
		public EffectArea actSkillArea;
		public EffectArea psvSkillArea;
		public bool IsCoolTime { get; protected set; }
		[SerializeField]protected NetworkBaseController Controller;

		public Behavior<NetworkBaseController> ActiveSkill = new Behavior<NetworkBaseController>(); //��ų ������ ��ģ�ൿ(��ų ������Ʈ�� �Ű��� ����)
		protected Sk_Default skDefault = new Sk_Default();

		public abstract bool CanActiveSkill();
		protected virtual void OnEnable()
		{
			IsCoolTime = false;
		}
		protected abstract IEnumerator ExcuteActiveSkill();
	}
}