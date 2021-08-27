//BY FHIZ
//MODIFIED BY DX4D

using OpenMMO;
using OpenMMO.Database;
using OpenMMO.Debugging;
using UnityEngine;

using System;

namespace OpenMMO.Database
{
    public abstract partial class BaseDatabaseManager
    {

        //CAN AUTO LOGIN
        public virtual bool CanPlayerAutoLogin(string playername, string username)
        {
            return (Tools.IsAllowedName(playername) //VALID CHARACTER NAME
                && Tools.IsAllowedName(username)); //VALID ACCOUNT NAME
        }

        //CAN SWITCH SERVER
        public virtual bool CanPlayerSwitchServer(string playername, string anchorname, string zonename, int token)
        {
            return (Tools.IsAllowedName(playername) //VALID CHARACTER NAME
                && !String.IsNullOrWhiteSpace(anchorname) //ANCHOR NAME NOT BLANK
                && !String.IsNullOrWhiteSpace(zonename) //ZONE NAME NOT BLANK
                && token >= 1000 && token <= 9999); //TOKEN IN RANGE
        }
    }
}
