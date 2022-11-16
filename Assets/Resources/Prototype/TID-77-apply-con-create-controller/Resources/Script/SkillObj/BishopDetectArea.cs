using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class BishopDetectArea: MonoBehaviour
	{
        List<DollController> NotRendOnTopDolls = new List<DollController>();
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(GameManager.DollTag))
            {
                DollController doll = other.GetComponent<DollController>();
                if (!doll)
                {
                    Debug.LogError("Missing doll");
                    return;
                }
                if (doll.CrossStack >= 4)
                {
                    doll.DoActionBy(Detected);
                }
                else
                {
                    NotRendOnTopDolls.Add(doll);
                }
            }
        }

        private void FixedUpdate()
        {
            if (NotRendOnTopDolls.Count == 0)
            {
                return;
            }

            foreach (DollController doll in NotRendOnTopDolls)
            {
                if (doll.CrossStack>=4)
                {
                    doll.DoActionBy(Detected);
                    NotRendOnTopDolls.Remove(doll);
                    return;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(GameManager.DollTag))
            {
                DollController doll = other.GetComponent<DollController>();
                if (!doll)
                {
                    Debug.LogError("Missing doll");
                    return;
                }

                if (doll.CrossStack >= 4)
                {
                    doll.DoActionBy(UnDetected);
                }
                else
                { 
                    NotRendOnTopDolls.Remove(doll);
                }
            }
        }

        protected void Detected(GameObject model)
        {
            StageManager.CharacterLayerChange(model,LayerMask.NameToLayer(GameManager.RendOnTopLayer));
        }
        protected void UnDetected(GameObject model)
        {
            StageManager.CharacterLayerChange(model, LayerMask.NameToLayer(GameManager.PlayerLayer));
        }
    }
}