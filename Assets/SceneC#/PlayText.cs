using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayText : MonoBehaviour
{
    public float moveSpeed = 5f;
    public VerticalHealthBar healthBar; // 血条引用
    
    private float currentValue = 5f; // 当前数值
    
    void Update()
    {
        // 左右移动
        float moveX = 0f;
        
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
            ChangeValue(1); // 左走加1
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveX = 1f;
            ChangeValue(-1); // 右走减1
        }
        
        // 应用移动
        transform.Translate(moveX * moveSpeed * Time.deltaTime, 0, 0);
    }
    
    void ChangeValue(float change)
    {
        currentValue += change;
        currentValue = Mathf.Clamp(currentValue, 0f, 10f);
        
        if (healthBar != null)
        {
            healthBar.SetHealthValue(currentValue);
        }
        
        Debug.Log($"当前数值: {currentValue}");
    }
}