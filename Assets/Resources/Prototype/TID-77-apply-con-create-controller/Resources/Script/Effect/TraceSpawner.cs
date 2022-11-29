using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
    public class TraceSpawner : MonoBehaviour
    {
        GameObject TracePrefab;
        float TraceSideLength;
        Ray ray;
        List<Trace> Traces = new List<Trace>();

        private void OnEnable()
        {
            ray = new Ray(transform.position, Vector3.down);
            TraceSideLength = TracePrefab.transform.localScale.x;
        }

        public void Update()
        {
            Vector3 centerPoint = transform.position;

        }

        private void SearchTrace(Vector3 centerPoint)
        {
            centerPoint = new Vector3(centerPoint.x - centerPoint.x % TraceSideLength, centerPoint.y, centerPoint.z - centerPoint.z % TraceSideLength);

            
            foreach (Trace trace in Traces)
            {
                if (trace.gameObject.activeInHierarchy&&(centerPoint - trace.gameObject.transform.position).sqrMagnitude.Equals(0) )
                {
                    


                }
            }

            Trace newTrace = Instantiate(TracePrefab, transform).GetComponent<Trace>();
            Traces.Add(newTrace);
            //newTrace.Start()

        }

    }

}

