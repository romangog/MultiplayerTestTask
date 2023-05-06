using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FpsMeter : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    float _clock;
    int _frames;

    private void Update()
    {
        _clock += Time.deltaTime;
        _frames++;
        if (_clock >= 1f)
        {
            _text.text = _frames.ToString() + " FPS";
            _frames = 0;
            _clock = 0f;
        }
    }
}
