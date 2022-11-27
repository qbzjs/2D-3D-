using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
    public class TestWolfEffect : MonoBehaviour
    {
        [SerializeField] private float Impulse = 1.0f;
        [SerializeField] GameObject SonarCircle;
        SourceOfSonar sourceOfSonar;
        void Start()
        {
            sourceOfSonar = GetComponent<SourceOfSonar>();
        }

    
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                sourceOfSonar.StartCircleSonar();
                //Instantiate(SonarCircle, transform.position,transform.rotation);
                //SimpleSonarShader_SonarSender.Instance.StartSonarRing(transform.position, Impulse);
            }
        }


    }

}

