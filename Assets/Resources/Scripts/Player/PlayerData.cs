using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DEM
{
    public struct PlayerData
    {
        /*--- Public Fields ---*/
        public ulong Score { get; private set; }
        public RoleType Role { get; private set; }


        /*--- Public Methods ---*/
        public void AddScore( ulong score_in )
        {
            Score += score_in;
        }
        public void ChangeRole(RoleType roleType)
        {
            Role = roleType;
        }
    }
}
