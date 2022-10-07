using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TID22{

    public class AltarManager : MonoBehaviour
    {
        #region Public Fields
        public static AltarManager instance { get { return _instance; } }
        public int altarCount = 0;
        public GameObject finalAltar;
        #endregion

    
        #region Private Fields
        private static AltarManager _instance;
        
        #endregion

    
        #region MonoBehaviour Callbacks
        void Start()
        {
            if(_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        void Update()
        {
        
        }
        #endregion

        #region Public Methods
        public void AddCount()
        {
            altarCount++;
            if(altarCount == 4)
            {
                finalAltar.SetActive(true);
            }
        }
        #endregion

        #region Private Methods
        #endregion
    }
}
