using System.Collections;
using System.Collections.Generic;
using KNXL;
using UnityEngine;

/// <summary>
/// 磁带类
/// </summary>
public class Cassette : BaseAction
{
    protected override void DoSelfSpecial()
    {
        // 子类可以重写这个方法，实现自己的特殊表现
        SaveGameData();
        ResetPlayerCrazyValue();
    }
    
    /// <summary>
    /// 保存游戏数据
    /// </summary>
    private void SaveGameData()
    {
        // 获取玩家当前位置
        Vector3 playerPos = GameManager.Instance.Player.transform.position;
        
        // 转换为MyVector3
        MyVector3 savePos = new MyVector3(playerPos);
        
        // 保存数据
        JsonMgr.Instance.SaveData(savePos, "playerPos");
        
        Debug.Log($"游戏数据已保存，玩家位置: {savePos}");
    }
    
    /// <summary>
    /// 重置玩家疯狂值
    /// </summary>
    private void ResetPlayerCrazyValue()
    {
        // 重置疯狂值为5
        int resetValue = 5 - GameManager.Instance.Player.NowCrazyValue;
        GameManager.Instance.Player.ChangeCrazyValue(resetValue);
        
        Debug.Log("玩家疯狂值已重置为5");
    }
}