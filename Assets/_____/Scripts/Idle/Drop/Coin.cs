using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Coin : Drop
{
    protected override void OnPickedUp()
    {
        if (_canSpawnPopup)
            _pickupPopup.SetNum(_stackNum, Color.yellow);
        //Money._instance.Add(_stackNum);
    }

    public class Factory : PlaceholderFactory<Drop.Settings, Coin>
    {

    }
}
