using Fusion;

public class CollectableCoin : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnCoinCollected))] public NetworkBool IsCollected { get; set; }

    private static void OnCoinCollected(Changed<CollectableCoin> changed)
    {
        if (changed.Behaviour.IsCollected = true)
            EventBus.CoinCollectedEvent?.Invoke(changed.Behaviour);
    }
}
