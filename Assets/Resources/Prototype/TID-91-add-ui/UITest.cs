using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace LSH_Lib
{
	public class UITest : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
        Vector3 maxSize = new Vector3(1.4f, 1.4f, 1.0f);
        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.LeanScale(maxSize, 0.5f).setEaseInOutCubic();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.LeanScale(Vector3.one, 0.5f).setEaseInOutCubic();
        }

    }

}
