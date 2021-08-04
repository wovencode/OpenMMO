//BY DX4D

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenMMO.Network
{
    /// <summary><c>NetworkTransaction</c> - A Collection of Client Requests and Server Responses</summary>
    [CreateAssetMenu(menuName = "OpenMMO/Network/Transaction Processor", order = 0)]
    public class NetworkTransactionProcessor : ScriptableObject
    {
        [SerializeField] NetworkTransaction _transactionToHandle;
        [SerializeField] ScriptableNetworkMessageHandler _clientRequestHandler;
        [SerializeField] ScriptableNetworkMessageHandler _serverResponseHandler;
    }
}
