using System.Collections;
using System.Collections.Generic;
using KNXL;
using UnityEngine;

/// <summary>
/// 路灯类
/// </summary>
public class Streetlight : BaseAction
{
    protected override void DoSelfSpecial()
    {
        // 子类可以重写这个方法，实现自己的特殊表现
        List<BaseAction> currentActions = GameManager.Instance.GameLogic.actions;
        foreach (BaseAction action in currentActions)
        {
            // 计算距离
            float distance = Vector3.Distance(transform.position, action.transform.position);
            
            // 只影响在checkDis范围内的实体
            if (distance > data.checkDis)
                continue;
            
            // 路灯会吸引乌鸦朝自己方向移动
            if (action is Crow)
            {
                Vector3 dir = (transform.position - action.transform.position).normalized;
                action.MoveTo(transform.position - dir);
            }
            // 路灯会驱逐老鼠朝自己方向的反方向移动
            else if (action is Mouse)
            {
                Vector3 dir = (action.transform.position - transform.position).normalized;
                action.MoveTo(transform.position + dir * (data.checkDis + 1));
            }
        }
    }
}