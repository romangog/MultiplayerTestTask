using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableCoin : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnCoinCollected))] public NetworkBool IsCollected { get; set; }

    private static void OnCoinCollected(Changed<CollectableCoin> changed)
    {
        Debug.Log("COin changed set to " + changed.Behaviour.IsCollected);
        if (changed.Behaviour.IsCollected = true)
            EventBus.CoinCollectedEvent?.Invoke(changed.Behaviour);
    }
}
