using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dataDelete : MonoBehaviour
{
    public void PlayerPrefsDelete()
    {
        PlayerPrefs.DeleteAll();
        LevelBtn[] levelBtns = FindObjectsOfType<LevelBtn>();

        foreach(LevelBtn _levelBtn in levelBtns)
        {
            _levelBtn.InitGame();
        }
    }
}
