using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public int x;
    public int y;
    public IAbility ability = null;

    public Cell(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// 单元格是否有能力
    /// </summary>
    /// <returns></returns>
    public bool HasAbility()
    {
        return ability != null;
    }

    public void SetAbility(IAbility ability)
    {
        this.ability = ability;
    }
}
