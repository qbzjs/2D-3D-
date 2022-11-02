using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSH_Lib
{
    public class GaugeTriggerArea : TriggerArea<GaugedObject>
    {
        protected override void OnTriggerEnter( Collider other )
        {
            base.OnTriggerEnter( other );
            target.SendMessage( "HandleTriggerEnter", other );
        }
        protected override void OnTriggerStay( Collider other )
        {
            base.OnTriggerStay( other );
            target.SendMessage( "HandleTriggerStay", other );
        }
        protected override void OnTriggerExit( Collider other )
        {
            base.OnTriggerExit( other );
            target.SendMessage( "HandleTriggerExit", other );
        }
    }
}