using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startBtn : MonoBehaviour
{
    public void gameStart(int diff)
    {
        Difficulty.Diificulty = diff;
        SceneManager.LoadScene("MainScene");
    }
}
