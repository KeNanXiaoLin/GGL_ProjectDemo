using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class MaskPosition
{
    public int value;
    public Vector2 position;
}

public class VerticalHealthBar : MonoBehaviour
{
    [Header("UI References")]
    public Image blueBar;
    public Image redBar;
    public Image maskSprite;
    public TextMeshProUGUI valueText;
    
    [Header("Configuration")]
    public float initialValue = 5f; // 初始值设为5
    
    [Header("Mask Positions")]
    public MaskPosition[] maskPositions = new MaskPosition[]
    {
        new MaskPosition { value = 0, position = new Vector2(0, 0) },
        new MaskPosition { value = 1, position = new Vector2(0, 10) },
        new MaskPosition { value = 2, position = new Vector2(0, 20) },
        new MaskPosition { value = 3, position = new Vector2(0, 30) },
        new MaskPosition { value = 4, position = new Vector2(0, 40) },
        new MaskPosition { value = 5, position = new Vector2(0, 50) },
        new MaskPosition { value = 6, position = new Vector2(0, 60) },
        new MaskPosition { value = 7, position = new Vector2(0, 70) },
        new MaskPosition { value = 8, position = new Vector2(0, 80) },
        new MaskPosition { value = 9, position = new Vector2(0, 90) },
        new MaskPosition { value = 10, position = new Vector2(0, 100) }
    };
    
    private int currentIntValue = 5;
    
    void Start()
    {
        // 初始化UI
        SetHealthValue(initialValue);
    }
    
    /// <summary>
    /// 外部调用接口：设置血条数值
    /// </summary>
    public void SetHealthValue(float value)
    {
        // 限制范围 0-10
        value = Mathf.Clamp(value, 0, 10);
        
        // 取整数部分
        int newIntValue = Mathf.FloorToInt(value);
        
        // 只有整数变化时才更新
        if (newIntValue != currentIntValue)
        {
            currentIntValue = newIntValue;
            
            // 更新蓝色血条（按0.1步进）
            if (blueBar != null)
            {
                blueBar.fillAmount = (float)currentIntValue / 10f;
            }
            
            // 更新面具位置
            UpdateMaskPosition(currentIntValue);
            
            // 更新显示文本
            if (valueText != null)
            {
                valueText.text = currentIntValue.ToString();
            }
        }
    }
    
    /// <summary>
    /// 更新面具位置
    /// </summary>
    private void UpdateMaskPosition(int value)
    {
        if (maskSprite == null) return;
        
        // 查找配置的位置
        foreach (var pos in maskPositions)
        {
            if (pos.value == value)
            {
                maskSprite.rectTransform.anchoredPosition = pos.position;
                return;
            }
        }
        
        // 没找到配置，使用默认计算
        maskSprite.rectTransform.anchoredPosition = new Vector2(0, value * 10f);
    }
}