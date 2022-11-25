using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class EffectInput: MonoBehaviour
	{
        [SerializeField] private GameObject[] effect;
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                EffectManager.Instance.ShowDecal(gameObject, effect[Random.Range(0, effect.Length - 1)]);
            }
        }
    }
}