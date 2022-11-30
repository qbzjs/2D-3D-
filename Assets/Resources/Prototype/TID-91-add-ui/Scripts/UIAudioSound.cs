using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace LSH_Lib
{
    public class UIAudioSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            UIAudioManager.instance.Play("ButtonOver");
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            
        }
    }
}
