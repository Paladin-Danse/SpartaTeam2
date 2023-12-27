using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBtn : MonoBehaviour
{
    [SerializeField] Diff diff;
    // Start is called before the first frame update
    void Start()
    {
        InitGame();
    }
    public void InitGame()
    {
        switch (diff)
        {
            case Diff.Normal:
                if (PlayerPrefs.HasKey("easyClear") == true)
                {
                    gameObject.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);
                }
                break;

            case Diff.Hard:
                if (PlayerPrefs.HasKey("normalClear") == true)
                    gameObject.SetActive(true);
                else
                {
                    gameObject.SetActive(false);
                }
                break;
        }
    }
}
