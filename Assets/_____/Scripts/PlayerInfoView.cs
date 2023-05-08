using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInfoView : MonoBehaviour , IPlayerCoinsDisplayable, IPlayerNameDisplayable
{
    [SerializeField] private TMP_Text _playerNameLabel;
    [SerializeField] private TMP_Text _coinsNumLabel;

    public void SetCoins(int coinsNum)
    {
        _coinsNumLabel.text = coinsNum.ToString();
    }

    public void SetName(string name)
    {
        _playerNameLabel.text = name;
    }
}

public interface IPlayerCoinsDisplayable
{
    public void SetCoins(int coinsNum);
}

public interface IPlayerNameDisplayable
{
    public void SetName(string name);
}
