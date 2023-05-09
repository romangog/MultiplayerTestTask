using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoView : MonoBehaviour , IPlayerCoinsDisplayable, IPlayerNameDisplayable, IPlayerHealthDisplayable
{
    [SerializeField] private TMP_Text _playerNameLabel;
    [SerializeField] private TMP_Text _coinsNumLabel;
    [SerializeField] private Image _playerHealthBar;

    public void SetCoins(int coinsNum)
    {
        _coinsNumLabel.text = coinsNum.ToString();
    }

    public void SetName(string name)
    {
        _playerNameLabel.text = name;
    }

    public void SetHealth(float health)
    {
        _playerHealthBar.fillAmount = health;
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

public interface IPlayerHealthDisplayable
{
    public void SetHealth(float health);
}

