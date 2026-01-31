using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility
{
    public int AbilityID { get; set; }
    /// <summary>
    /// 初始化能力
    /// </summary>
    public void InitAbility();
    /// <summary>
    /// 得到能力方法
    /// </summary>
    /// <returns></returns>
    public IAbility GetAbility();
}
