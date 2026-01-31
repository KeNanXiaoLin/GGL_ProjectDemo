using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jieshu : MonoBehaviour
{
   public void QuitGameButton()
    {
        // 在编辑器中停止播放模式
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // 在打包后的程序里直接退出
            Application.Quit();
        #endif
    }
}
