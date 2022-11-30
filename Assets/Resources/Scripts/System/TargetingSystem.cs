using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

namespace KSH_Lib.Util
{
    public class TargetingSystem
    {
        public static List<GameObject> FindTargetInRange(in Vector3 origin, float distance, in List<GameObject> targets )
        {
            List<GameObject> results = new List<GameObject>();
            foreach ( var obj in targets )
            {
                if ( Vector3.Distance( origin, obj.transform.position ) < distance )
                {
                    results.Add( obj );
                }
            }
            return results;
        }
        public static List<GameObject> FindTargetInRange(in Vector3 origin, float distance, string tag)
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag( tag );
            return FindTargetInRange( origin, distance, targets.ToList() );
        }
    }
}