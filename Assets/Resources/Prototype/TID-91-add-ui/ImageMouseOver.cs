using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib.UI;
using UnityEngine.EventSystems;
namespace LSH_Lib
{
	public class ImageMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private void Start()
        {
            transform.localScale = Vector3.zero;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.LeanScale(Vector3.one, 0.5f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.LeanScale(Vector3.zero, 0.5f);
        }
    }
}
