//BY DX4D
using System;

namespace OpenMMO.Network
{
    /// <summary>
    /// NetworkActions represent a set of actions that can be passed between Client and Server.
    /// By combining the Flags attribute and Bitwise declaration we can pipe '|' multiple actions together in one message.
    /// <see cref="https://www.alanzucconi.com/2015/07/26/enum-flags-and-bitwise-operators/"/>
    /// </summary>
    [Flags] public enum NetworkAction //: ulong //NOTE: You can have a max of 32 entries by default - Uncomment this to get 64
    {
        //NO ACTION
        None                = 0,

        //ERROR STATE
        Error               = 1 << 0,

        //AUTHENTICATION
        Connect        = 1 << 1,

        //PLAYER CHARACTER PREVIEW
        UserPlayerPreviews  = 1 << 2,

        //USER ACCOUNT
        UserLogin           = 1 << 3,
        UserLogout          = 1 << 4,
        UserRegister        = 1 << 5,
        UserDelete          = 1 << 6,
        UserChangePassword  = 1 << 7,
        UserConfirm         = 1 << 8,

        //PLAYER CHARACTER
        PlayerLogin         = 1 << 9,
        PlayerRegister      = 1 << 10,
        PlayerDelete        = 1 << 11,

        //ZONES
        ZoneSwitchServer    = 1 << 12,
        ZoneAutoConnect     = 1 << 13,
        ZoneAutoLogin       = 1 << 14
    }
}