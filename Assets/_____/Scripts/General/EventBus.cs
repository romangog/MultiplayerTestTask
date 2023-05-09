using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
{
    public static Action<Player> PlayerSpawnedEvent;
    public static Action<Player> PlayerNameChangedEvent;
    public static Action<Player> PlayerCollectedCoinsChangedEvent;
    public static Action<CollectableCoin> CoinCollectedEvent;
}
