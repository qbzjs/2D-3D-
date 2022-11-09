using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSH_Lib.Util
{
    public class Utilities
    {
        public static Vector3 RotateAroundPoint(in Vector3 pivot, in Vector3 point, Quaternion angle)
        {
            return (angle * (point - pivot)) + pivot;
        }
        public static float GetAngle( Vector3 vStart, Vector3 vEnd )
        {
            Vector3 v = vEnd - vStart;

            return Mathf.Atan2( v.y, v.x ) * Mathf.Rad2Deg;
        }
        public static float ClampAngle( float angle, float min, float max )
        {
            if ( angle < -360F )
                angle += 360F;
            if ( angle > 360F )
                angle -= 360F;
            return Mathf.Clamp( angle, min, max );
        }
    }
}