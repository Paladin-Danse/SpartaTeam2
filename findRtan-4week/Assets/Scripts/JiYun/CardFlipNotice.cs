using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFlipNotice : MonoBehaviour
{
    [SerializeField] Text _noticeText;
    [SerializeField] Text _tryCountText;
    [SerializeField] string[] _names;

    public int _tryCount;

    private void Start()
    {
        _noticeText.text = "";        
    }

    public void MatchCheck(bool isMatched, WhosCard whos)
    {
        if (isMatched)
            _noticeText.text = $"{_names[(int)whos]} ��(��) ã�Ҵ�!";
        else
            _noticeText.text = "Ʋ������~ :p";
    }

    public void ResetText() => _noticeText.text = "";

    public void TryCountNotice()
    {
        _tryCountText.text = $"�õ� Ƚ�� : {_tryCount} ȸ";
    }
}
