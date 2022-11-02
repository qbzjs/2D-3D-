using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LSH_Lib
{
	public class BoxButtonTest : MonoBehaviour
    {
        [SerializeField]
        GameObject introduction;
        [SerializeField]
        GameObject common;
        [SerializeField]
        GameObject active;
        [SerializeField]
        GameObject passive;

        private void DisableAllText()
        {
            introduction.SetActive(false);
            common.SetActive(false);
            active.SetActive(false);
            passive.SetActive(false);
        }
        private void EnableAllTest()
        {
            introduction.SetActive(true);
            common.SetActive(true);
            active.SetActive(true);
            passive.SetActive(true);
        }

        void EnableIntroductionText()
        {
            DisableAllText();
            introduction.SetActive(true);
        }
        void EnableCommonText()
        {
            DisableAllText();
            common.SetActive(true);
        }
        void EnableActiveText()
        {
            DisableAllText();
            active.SetActive(true);
        }
        void EnablePassiveText()
        {
            DisableAllText();
            passive.SetActive(true);
        }

    }
}

