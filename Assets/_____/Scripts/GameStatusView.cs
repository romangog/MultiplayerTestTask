using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStatusView : MonoBehaviour, IGameStatusDisplayable
{
    [SerializeField] private TMP_Text _gameStatusLabel;

    public void DisplayGameStatus(string status)
    {
        _gameStatusLabel.text = status;
    }
}

public interface IGameStatusDisplayable
{
    void DisplayGameStatus(string status);
}
