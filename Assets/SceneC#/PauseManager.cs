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
    }

    // 暂停游戏
    public void Pause()
    {
        pauseMenu.SetActive(true);
    }

    // 返回主菜单
    public void BackToMenu()
    {
        SceneManager.LoadScene("Start Scenes");
    }

    // 重新开始当前关卡
    public void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

