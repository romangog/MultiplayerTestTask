using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProgressTask : TutorialTask
{
    [SerializeField] private int _max;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private string _defaultText;

    private int _currentCount;

    internal override void Progress(int count = 1)
    {
        _currentCount += count;
        _text.text = _defaultText + _currentCount.ToString() + "/" + _max.ToString();
        if(_currentCount >= _max)
        {
            Success();
        }
    }

    internal override void Show_Specific()
    {
        _text.text = _defaultText + _currentCount.ToString() + "/" + _max.ToString();
    }

    internal override void Success_Specific()
    {
        
    }
}
