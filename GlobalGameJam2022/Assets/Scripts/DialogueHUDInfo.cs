using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueHUDInfo : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI _characterText;
    
    [SerializeField]
    private float _timePerCharacter = 0.02f;

    private string _textToWrite;
    private int _characterIndex;
    private float _timer;

    private bool _canWrite = false;

    private void Start()
    {
        //_characterText.enabled = false;
        //Shown(false);
    }

    public void Shown(bool show)
    {
        this.gameObject.SetActive(show);
    }

    public void PresentText(string entry)
    {
        _characterText.enabled = true;
        if (_characterText == null || entry == null)
        {
            return;
        }
        
        _textToWrite = entry;
        _characterIndex = 0;
        _timer = 0.0f;
        _characterText.text = "";

        _canWrite = true;
    }

    private void Update()
    {
        if (_characterText != null && _canWrite)
        {
            _timer -= Time.deltaTime;
            while (_timer <= 0.0f)
            {
                _timer += _timePerCharacter;

                _characterIndex++;

                if (_characterIndex < _textToWrite.Length && _textToWrite[_characterIndex] == '\\')
                {
                    // skip past line breaks
                    _characterIndex += 2;
                }

                _characterText.text = _textToWrite.Substring(0, _characterIndex).Replace("\\n", "\n");
                _characterText.parseCtrlCharacters = true;
                
                if (_characterIndex >= _textToWrite.Length)
                {
                    // entire string displayed
                    _canWrite = false;
                }
            }
        }
    }
}
