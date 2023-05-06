using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenLog : MonoBehaviour
{
    private static ScreenLog _instance;

    [SerializeField] private TMP_Text _textPrefab;

    private Dictionary<string, TMP_Text> _lines = new Dictionary<string, TMP_Text>();

    private void Start()
    {
        _instance = this;
    }

    public static void Log(string key, string line) => _instance.Log_p(key, line);

    private void Log_p(string key, string line)
    {
        if (!_lines.ContainsKey(key))
        {
            TMP_Text newLine = GameObject.Instantiate(_textPrefab, this.transform);
            _lines.Add(key, newLine);
            newLine.transform.localPosition = new Vector3(newLine.transform.localPosition.x, -90 * _lines.Count, 0);
        }
        _lines[key].text = key + ": " + line;
    }
}
