using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Zenject;

public class TextPopup : MonoBehaviour
{
    internal bool IsDestroyed => _IsDestroyed;
    internal float TimeSinceSpawn => _timeSinceSpawn;
    [SerializeField] private TMP_Text _text;
    private bool _IsDestroyed;
    private float _timeSinceSpawn;
    private int _num;
    private Color _color;

    void Start()
    {
        this.transform.DOBlendableMoveBy(Vector3.up * 0.5f, 1f).OnComplete(OnEnd);
    }

    internal void SetText(string text, Color color)
    {
        _text.text = text;
        _text.color = color;
        _color = color;
    }

    internal void SetNum(int num, Color color)
    {
        _num = num;
        SetText("+" + AbbrevationUtility.AbbreviateNumber(_num), color);
    }

    internal void AddNum(int add)
    {
        SetNum(_num + add, _color);
    }

    private void Update()
    {
        _timeSinceSpawn += Time.deltaTime;
    }

    private void OnEnd()
    {
        _IsDestroyed = true;
        Destroy(gameObject);
    }

    public class Factory : PlaceholderFactory<TextPopup>
    {

    }
}
