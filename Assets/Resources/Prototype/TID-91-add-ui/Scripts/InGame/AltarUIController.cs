using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KSH_Lib;
using GHJ_Lib;
namespace LSH_Lib
{
    public class AltarUIController : MonoBehaviour
    {
        [Header("Normal Altar")]
        [SerializeField]
        Sprite normalAltarSprite;
        [SerializeField]
        Image[] normalAltarImages;

        private void Update()
        {
            AltarIcon();
        }
        void AltarIcon()
        {
            int count = StageManager.Instance.AltarCount;
            switch(count)
            {
                case 3:
                    normalAltarImages[0].sprite = normalAltarSprite;
                    break;
                case 2:
                    normalAltarImages[0].sprite = normalAltarSprite;
                    normalAltarImages[1].sprite = normalAltarSprite;
                    break;
                case 1:
                    normalAltarImages[0].sprite = normalAltarSprite; 
                    normalAltarImages[1].sprite = normalAltarSprite; 
                    normalAltarImages[2].sprite = normalAltarSprite;
                    break;
                case 0:
                    normalAltarImages[0].sprite = normalAltarSprite;
                    normalAltarImages[1].sprite = normalAltarSprite;
                    normalAltarImages[2].sprite = normalAltarSprite;
                    normalAltarImages[3].sprite = normalAltarSprite;
                    break;
            }
        }
    }
}

