using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endTxt : MonoBehaviour
{
    public void ReGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");
    }
}
