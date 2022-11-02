using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KSH_Lib.UI;
using UnityEngine.EventSystems;
namespace LSH_Lib
{
	public class ImageMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.LeanScale(new Vector3(1.4f,1.4f,1), 1.0f);
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.LeanScale(Vector3.one, 0.5f);
        }
    }
}
