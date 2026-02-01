using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // 场景管理命名空间
public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;  // 暂停菜单UI

    // 恢复游戏
    public void Restore()
    {
        pauseMenu.SetActive(false);
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        player.RestoreGame();
    }

    // 暂停游戏
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        player.PauseGame();
    }

    // 返回主菜单
    public void BackToMenu()
    {
        SceneManager.LoadScene("Start Scenes");
    }

    // 重新开始当前关卡
    public void ReStart()
    {
        GameManager.Instance.LoadGame();
        SceneManager.LoadScene("GameScene 3");
    }
}

