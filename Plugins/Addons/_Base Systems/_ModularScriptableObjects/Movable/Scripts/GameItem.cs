//BY DX4D
using UnityEngine;
using Mirror;
using System;

namespace OpenMMO
{
    [Serializable] public class SyncListGameItem : SyncList<GameItem> { } //SYNCLIST

    public abstract class GameItem : ScriptableObject { }
}