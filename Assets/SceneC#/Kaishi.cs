using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Kaishi : MonoBehaviour
{
     public void StartGame()
    {
        SceneManager.LoadScene("GameScene 3");
    }

    public void RestartGame()
    {
        GameManager.Instance.LoadGame();
        SceneManager.LoadScene("GameScene 3");
    }
}
