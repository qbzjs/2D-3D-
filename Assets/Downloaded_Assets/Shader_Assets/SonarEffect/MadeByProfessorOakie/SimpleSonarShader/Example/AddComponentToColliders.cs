// SimpleSonarShader scripts and shaders were written by Drew Okenfuss.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddComponentToColliders : MonoBehaviour {

    [SerializeField]
    private string newComponentToAdd = "";

    private void Start()
    {
        foreach (Component comp in GetComponentsInChildren<Collider>())
        {
            comp.gameObject.AddComponent(Type.GetType(newComponentToAdd));
        }
    }

}
