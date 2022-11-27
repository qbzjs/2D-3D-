using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
    public class SourceOfSonar : MonoBehaviour
    {
        [SerializeField] private float Impulse = 1.0f;
        [SerializeField] GameObject SonarCircle;

        List<SonarCircle> SonarCircles = new List<SonarCircle>();
        
        public void StartCircleSonar()
        {
            foreach (SonarCircle circle in SonarCircles)
            {
                if (!circle.gameObject.activeInHierarchy)
                {
                    circle.gameObject.SetActive(true);
                    circle.ReStart();
                    SimpleSonarShader_SonarSender.Instance.StartSonarRing(transform.position, Impulse);
                    return;
                }
            }
            GameObject sCircle = Instantiate(SonarCircle, transform.position, transform.rotation);
            SonarCircles.Add(sCircle.GetComponent<SonarCircle>());
            SimpleSonarShader_SonarSender.Instance.StartSonarRing(transform.position, Impulse);
        }

    }
}

