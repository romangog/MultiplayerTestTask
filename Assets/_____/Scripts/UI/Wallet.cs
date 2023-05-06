using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class Wallet : MonoBehaviour
{
    [SerializeField] private bool _IsOnLevel;
    [SerializeField] private TMP_Text _textLabel;
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _redGlowParticles;

    private Money _money;

    [Inject]
    private void Construct(Money money)
    {
        
        _money = money;
        if (_IsOnLevel)
        {
            _money.MoneyOnLevelUpdatedEvent.AddListener(SetMoney);
            SetMoney(money.MoneyOnLevel);
        }
        else
        {
            _money.MoneyNotEnoughEvent.AddListener(BlinkRed);
            _money.MoneyUpdatedEvent.AddListener(SetMoney);
            SetMoney(money.Count);
        }

    }

    public void SetMoney(int count)
    {
        _textLabel.text = AbbrevationUtility.AbbreviateNumber(count);
        if(_animator.gameObject.activeInHierarchy) _animator.Play("Update");
    }

    internal void BlinkRed()
    {
        if (_animator.gameObject.activeInHierarchy) _animator.Play("UpdateEmpty");
        _redGlowParticles.Play();
    }
}
