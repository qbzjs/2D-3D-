using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DEM
{
    public struct PlayerData
    {
        #region Public Fields
        public ulong Score { get { return score; } }
        public RoleType Role { get { return roleType; } }
        #endregion


        #region Private Fields
        RoleType roleType;
        ulong score;
        #endregion


        #region Public Fields
        public void AddScore( ulong score_in )
        {
            score += score_in;
        }
        public void ChangeRole(RoleType roleType_in)
        {
            roleType = roleType_in;
        }
        #endregion


        #region Private Fields

        #endregion

    }
}
