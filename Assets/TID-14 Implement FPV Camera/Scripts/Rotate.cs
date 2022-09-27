using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
    public class Rotate : MonoBehaviour
    {
        #region Public Fields
        #endregion

        #region Private Fields
        #endregion

        #region MonoBehaviour Callbacks
        void Start()
        {

        }

        void Update()
        {
            if (Input.GetKey(KeyCode.A))
            {
                this.transform.Rotate(0, 10*Time.deltaTime, 0);
            }
            if(Input.GetKey(KeyCode.D))
            {

            }
        }
        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        #endregion
    }
}

