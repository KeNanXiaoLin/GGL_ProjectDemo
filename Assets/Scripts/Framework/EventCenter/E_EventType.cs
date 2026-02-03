using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KNXL
{
    /// <summary>
    /// 事件类型 枚举
    /// </summary>
    public enum E_EventType
    {
        /// <summary>
        /// 场景切换时进度变化获取
        /// </summary>
        E_SceneLoadChange,

        /// <summary>
        /// 水平热键 -1~1的事件监听
        /// </summary>
        E_Input_Horizontal,

        /// <summary>
        /// 竖直热键 -1~1的事件监听
        /// </summary>
        E_Input_Vertical,
        /// <summary>
        /// 更新游戏UI
        /// </summary>
        E_UpdateGameUI,
        /// <summary>
        /// 世界发生变化触发的事件
        /// </summary>
        E_WorldChange,
    }
}
