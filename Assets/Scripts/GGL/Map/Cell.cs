using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public int x;
    public int y;
    private IAbility ability = null;

    public Cell(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// 重载==运算符，用于比较两个Cell对象是否相等
    /// </summary>
    /// <param name="a">第一个Cell对象</param>
    /// <param name="b">第二个Cell对象</param>
    /// <returns>如果两个Cell对象的x和y坐标都相等，则返回true；否则返回false</returns>
    public static bool operator==(Cell a, Cell b)
    {
        // 处理null值的情况
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
        {
            return false;
        }

        // 比较x和y坐标
        return a.x == b.x && a.y == b.y;
    }

    /// <summary>
    /// 重载!=运算符，用于比较两个Cell对象是否不相等
    /// </summary>
    /// <param name="a">第一个Cell对象</param>
    /// <param name="b">第二个Cell对象</param>
    /// <returns>如果两个Cell对象的x或y坐标不相等，则返回true；否则返回false</returns>
    public static bool operator!=(Cell a, Cell b)
    {
        return !(a == b);
    }

    /// <summary>
    /// 重写Equals方法，用于比较两个对象是否相等
    /// </summary>
    /// <param name="obj">要比较的对象</param>
    /// <returns>如果对象是Cell类型且x和y坐标都相等，则返回true；否则返回false</returns>
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Cell other = (Cell)obj;
        return x == other.x && y == other.y;
    }

    /// <summary>
    /// 重写GetHashCode方法，返回对象的哈希码
    /// </summary>
    /// <returns>基于x和y坐标计算的哈希码</returns>
    public override int GetHashCode()
    {
        // 使用x和y坐标计算哈希码
        int hash = 17;
        hash = hash * 23 + x.GetHashCode();
        hash = hash * 23 + y.GetHashCode();
        return hash;
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

    public IAbility GetAbility()
    {
        return ability;
    }

    /// <summary>
    /// 清除单元格的能力
    /// </summary>
    public void ClearAbility()
    {
        ability = null;
    }
}
