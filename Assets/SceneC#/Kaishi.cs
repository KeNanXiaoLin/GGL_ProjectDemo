using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Kaishi : MonoBehaviour
{
     public void StartGame()
    {
        SceneManager.LoadScene(Setting.GAME_SCENE_NAME);
    }

    public void RestartGame()
    {
        GameManager.Instance.LoadGame();
        SceneManager.LoadScene(Setting.GAME_SCENE_NAME);
    }
}
