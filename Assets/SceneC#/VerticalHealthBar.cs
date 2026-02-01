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
    
    [Header("死亡设置")]
    public AudioClip deathSound; // 死亡音效
    public string dieSceneName = "Die"; // 死亡场景名称
    public float sceneLoadDelay = 1.5f; // 跳转延迟时间
    
    [Header("Mask Positions")]
    public MaskPosition[] maskPositions = new MaskPosition[]
    {
        new MaskPosition { value = 0, position = new Vector2(-880, 217) },
        new MaskPosition { value = 1, position = new Vector2(-880, 41) },
        new MaskPosition { value = 2, position = new Vector2(-880, -39) },
        new MaskPosition { value = 3, position = new Vector2(-880, -112) },
        new MaskPosition { value = 4, position = new Vector2(-880, -185) },
        new MaskPosition { value = 5, position = new Vector2(-880, -202) },
        new MaskPosition { value = 6, position = new Vector2(-880, -245) },
        new MaskPosition { value = 7, position = new Vector2(-880, -319) },
        new MaskPosition { value = 8, position = new Vector2(-880, -396) },
        new MaskPosition { value = 9, position = new Vector2(-880, -461) },
        new MaskPosition { value = 10, position = new Vector2(-880, -587) }
    };
    
    private int currentIntValue = 5;
    private AudioSource audioSource;
    private bool isDead = false;
    
    void Start()
    {
        // 获取或创建AudioSource组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // 初始化UI
        SetHealthValue(initialValue);
    }
    
    /// <summary>
    /// 外部调用接口：设置血条数值
    /// </summary>
    public void SetHealthValue(float value)
    {
        // 如果已经死亡，不再处理
        if (isDead) return;
        
        // 限制范围 0-10
        value = Mathf.Clamp(value, 0, 10);
        
        // 取整数部分
        int newIntValue = Mathf.FloorToInt(value);
        
        // 只有整数变化时才更新
        if (newIntValue != currentIntValue)
        {
            // 检查是否到达死亡条件
            CheckDeathCondition(newIntValue);
            
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
    /// 检查死亡条件
    /// </summary>
    private void CheckDeathCondition(int newValue)
    {
        // 到达0或10时触发死亡
        if (newValue == 0 || newValue == 10)
        {
            if (!isDead)
            {
                isDead = true;
                TriggerDeath();
            }
        }
    }
    
    /// <summary>
    /// 触发死亡效果
    /// </summary>
    private void TriggerDeath()
    {
        Debug.Log($"死亡触发！数值到达: {currentIntValue}");
        
        // 播放死亡音效
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
            Debug.Log("播放死亡音效");
        }
        else
        {
            Debug.LogWarning("死亡音效未设置或AudioSource缺失");
        }
        
        // 延迟跳转到死亡场景
        if (!string.IsNullOrEmpty(dieSceneName))
        {
            Invoke("LoadDieScene", sceneLoadDelay);
        }
    }
    
    /// <summary>
    /// 加载死亡场景
    /// </summary>
    private void LoadDieScene()
    {
        Debug.Log($"跳转到场景: {dieSceneName}");
        
        // 使用UnityEngine.SceneManagement加载场景
        #if UNITY_6000_0_OR_NEWER
        UnityEngine.SceneManagement.SceneManager.LoadScene(dieSceneName);
        #else
        // 对于旧版本Unity
        UnityEngine.SceneManagement.SceneManager.LoadScene(dieSceneName);
        #endif
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
        maskSprite.rectTransform.anchoredPosition = new Vector2(-880, value * 10f);
    }
    
    /// <summary>
    /// 重置血条状态（可选功能）
    /// </summary>
    public void ResetHealth()
    {
        isDead = false;
        SetHealthValue(initialValue);
    }
}