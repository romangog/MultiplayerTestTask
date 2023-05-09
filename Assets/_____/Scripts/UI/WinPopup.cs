using TMPro;
using UnityEngine;

public class WinPopup : MonoBehaviour, IWinPopupDisplayable
{
    [SerializeField] private GameObject _view;
    [SerializeField] private TMP_Text _winnerCoinsNumLabel;
    [SerializeField] private TMP_Text _winnerNameLabel;
    public void Show()
    {
        _view.SetActive(true);
    }

    public void Hide()
    {
        _view.SetActive(false);
    }

    public void DisplayWinnerInfo(string name, int coins, Color winnerColor)
    {
        Show();
        _winnerCoinsNumLabel.text = coins.ToString();
        _winnerNameLabel.text = name.ToString();
        _winnerNameLabel.color = winnerColor;
    }
}

public interface IWinPopupDisplayable
{
    public void DisplayWinnerInfo(string name, int coins, Color winnerColor);
}