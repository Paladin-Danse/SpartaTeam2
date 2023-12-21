using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFlipNotice : MonoBehaviour
{
    [SerializeField] Text _noticeText;
    [SerializeField] string[] _names;
    
    private void Start()
    {
        _noticeText.text = "";        
    }

    public void MatchCheck(bool isMatched, WhosCard whos)
    {
        if (isMatched)
            _noticeText.text = $"{_names[(int)whos]} ¸¦(À») Ã£¾Ò´Ù!";
        else
            _noticeText.text = "Æ²·ÈÁö·Õ~ :p";
    }

    public void ResetText() => _noticeText.text = "";
}
