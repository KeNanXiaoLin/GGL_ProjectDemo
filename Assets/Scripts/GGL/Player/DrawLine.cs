using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public LineRenderer lineRenderer;
    [Tooltip("线的开始宽度")]
    [SerializeField] private float startWidth = 0.05f;
    [Tooltip("线的结束宽度")]
    [SerializeField] private float endWidth = 0.1f;
    [Tooltip("左边界")]
    [SerializeField] private float leftX = -10f;
    [Tooltip("右边界")]
    [SerializeField] private float rightX = 10f;
    [Tooltip("下边界")]
    [SerializeField] private float bottomY = -5f;
    [Tooltip("上边界")]
    [SerializeField] private float topY = 5f;
    private bool enableDraw = false;
    private Vector2 lastMousePos;

    void Update()
    {

    }

    private void DrawInfoLine()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = HandleMousePos(mousePos);
        lastMousePos = mousePos;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, mousePos);
        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;
        // 指向了自己
        if (GameManager.Instance.MapCell.CalGridDisByWorldPos(transform.position, mousePos) <= 10)
        {
            lineRenderer.startColor = Color.green;
            lineRenderer.endColor = Color.green;
            Cell targetCell = GameManager.Instance.MapCell.WorldToCell(mousePos);
            if (targetCell.Action != null)
            {
                lineRenderer.endColor = Color.yellow;
            }
        }
        else
        {
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
        }
    }

    private Vector2 HandleMousePos(Vector2 mousePos)
    {
        if (mousePos.x < leftX)
        {
            mousePos.x = leftX;
        }
        else if (mousePos.x > rightX)
        {
            mousePos.x = rightX;
        }
        if (mousePos.y < bottomY)
        {
            mousePos.y = bottomY;
        }
        else if (mousePos.y > topY)
        {
            mousePos.y = topY;
        }
        return mousePos;
    }

    public void EnableDraw()
    {
        enableDraw = true;
    }

    public void DisableDraw()
    {
        enableDraw = false;
    }

    private void ClearInfoLine()
    {
        lineRenderer.startWidth = 0f;
        lineRenderer.endWidth = 0f;
    }

}
