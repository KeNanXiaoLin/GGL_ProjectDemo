using UnityEngine;

[System.Serializable]
public class MyVector3
{
    // 向量的三个分量
    public float x;
    public float y;
    public float z;

    /// <summary>
    /// 默认构造函数，初始化所有分量为0
    /// </summary>
    public MyVector3()
    {
        x = 0;
        y = 0;
        z = 0;
    }

    /// <summary>
    /// 带参数的构造函数
    /// </summary>
    /// <param name="x">x分量</param>
    /// <param name="y">y分量</param>
    /// <param name="z">z分量</param>
    public MyVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    /// <summary>
    /// 从Unity的Vector3构造MyVector3
    /// </summary>
    /// <param name="vector">Unity的Vector3对象</param>
    public MyVector3(UnityEngine.Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    /// <summary>
    /// 将MyVector3转换为Unity的Vector3
    /// </summary>
    /// <returns>Unity的Vector3对象</returns>
    public UnityEngine.Vector3 ToVector3()
    {
        return new UnityEngine.Vector3(x, y, z);
    }

    /// <summary>
    /// 隐式转换：从MyVector3到Vector3
    /// </summary>
    /// <param name="myVector">MyVector3对象</param>
    public static implicit operator UnityEngine.Vector3(MyVector3 myVector)
    {
        return myVector.ToVector3();
    }

    /// <summary>
    /// 隐式转换：从Vector3到MyVector3
    /// </summary>
    /// <param name="vector">Vector3对象</param>
    public static implicit operator MyVector3(UnityEngine.Vector3 vector)
    {
        return new MyVector3(vector);
    }

    /// <summary>
    /// 重载ToString方法，方便调试
    /// </summary>
    /// <returns>向量的字符串表示</returns>
    public override string ToString()
    {
        return $"({x}, {y}, {z})";
    }

    /// <summary>
    /// 计算向量的长度（模）
    /// </summary>
    /// <returns>向量的长度</returns>
    public float Magnitude()
    {
        return Mathf.Sqrt(x * x + y * y + z * z);
    }

    /// <summary>
    /// 计算向量的平方长度（避免开方运算，提高性能）
    /// </summary>
    /// <returns>向量的平方长度</returns>
    public float SqrMagnitude()
    {
        return x * x + y * y + z * z;
    }

    /// <summary>
    /// 归一化向量（将向量长度变为1）
    /// </summary>
    public void Normalize()
    {
        float mag = Magnitude();
        if (mag > 0)
        {
            x /= mag;
            y /= mag;
            z /= mag;
        }
    }

    /// <summary>
    /// 返回归一化后的向量（不修改原向量）
    /// </summary>
    /// <returns>归一化后的向量</returns>
    public MyVector3 Normalized()
    {
        float mag = Magnitude();
        if (mag > 0)
        {
            return new MyVector3(x / mag, y / mag, z / mag);
        }
        return new MyVector3();
    }

    // 以下是一些常用的静态方法和运算符重载，使MyVector3的使用更方便

    /// <summary>
    /// 两个向量相加
    /// </summary>
    /// <param name="a">第一个向量</param>
    /// <param name="b">第二个向量</param>
    /// <returns>相加后的向量</returns>
    public static MyVector3 operator +(MyVector3 a, MyVector3 b)
    {
        return new MyVector3(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    /// <summary>
    /// 两个向量相减
    /// </summary>
    /// <param name="a">第一个向量</param>
    /// <param name="b">第二个向量</param>
    /// <returns>相减后的向量</returns>
    public static MyVector3 operator -(MyVector3 a, MyVector3 b)
    {
        return new MyVector3(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    /// <summary>
    /// 向量乘以标量
    /// </summary>
    /// <param name="a">向量</param>
    /// <param name="scalar">标量</param>
    /// <returns>乘以标量后的向量</returns>
    public static MyVector3 operator *(MyVector3 a, float scalar)
    {
        return new MyVector3(a.x * scalar, a.y * scalar, a.z * scalar);
    }

    /// <summary>
    /// 标量乘以向量
    /// </summary>
    /// <param name="scalar">标量</param>
    /// <param name="a">向量</param>
    /// <returns>乘以标量后的向量</returns>
    public static MyVector3 operator *(float scalar, MyVector3 a)
    {
        return new MyVector3(a.x * scalar, a.y * scalar, a.z * scalar);
    }

    /// <summary>
    /// 向量除以标量
    /// </summary>
    /// <param name="a">向量</param>
    /// <param name="scalar">标量</param>
    /// <returns>除以标量后的向量</returns>
    public static MyVector3 operator /(MyVector3 a, float scalar)
    {
        if (scalar != 0)
        {
            return new MyVector3(a.x / scalar, a.y / scalar, a.z / scalar);
        }
        throw new System.DivideByZeroException("Cannot divide by zero");
    }

    /// <summary>
    /// 计算两个向量的点积
    /// </summary>
    /// <param name="a">第一个向量</param>
    /// <param name="b">第二个向量</param>
    /// <returns>点积结果</returns>
    public static float Dot(MyVector3 a, MyVector3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    /// <summary>
    /// 计算两个向量的叉积
    /// </summary>
    /// <param name="a">第一个向量</param>
    /// <param name="b">第二个向量</param>
    /// <returns>叉积结果</returns>
    public static MyVector3 Cross(MyVector3 a, MyVector3 b)
    {
        return new MyVector3(
            a.y * b.z - a.z * b.y,
            a.z * b.x - a.x * b.z,
            a.x * b.y - a.y * b.x
        );
    }

    /// <summary>
    /// 计算两个向量之间的距离
    /// </summary>
    /// <param name="a">第一个向量</param>
    /// <param name="b">第二个向量</param>
    /// <returns>两个向量之间的距离</returns>
    public static float Distance(MyVector3 a, MyVector3 b)
    {
        return (a - b).Magnitude();
    }

    // 常用的向量常量
    public static readonly MyVector3 zero = new MyVector3(0, 0, 0);
    public static readonly MyVector3 one = new MyVector3(1, 1, 1);
    public static readonly MyVector3 up = new MyVector3(0, 1, 0);
    public static readonly MyVector3 down = new MyVector3(0, -1, 0);
    public static readonly MyVector3 left = new MyVector3(-1, 0, 0);
    public static readonly MyVector3 right = new MyVector3(1, 0, 0);
    public static readonly MyVector3 forward = new MyVector3(0, 0, 1);
    public static readonly MyVector3 back = new MyVector3(0, 0, -1);
}