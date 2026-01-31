using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility
{
    /// <summary>
    /// 初始化能力
    /// </summary>
    public void InitAbility();
    /// <summary>
    /// 得到能力方法
    /// </summary>
    /// <returns></returns>
    public IAbility GetAbility();
    /// <summary>
    /// 得到能力ID
    /// </summary>
    /// <returns></returns>
    public int GetAbilityID();
}
