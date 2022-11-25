using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace GHJ_Lib
{
	public class EffectInput: MonoBehaviour
	{
        [SerializeField] private GameObject[] effect;
        [SerializeField] Image[] images;
        [SerializeField] RectTransform rectTransform;
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                EffectManager.Instance.ShowDecal(gameObject, effect[Random.Range(0, effect.Length - 1)]);
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                EffectManager.Instance.ShowBloodOnCamera(images[Random.Range(0, images.Length - 1)], rectTransform);
            }
        }
    }
}