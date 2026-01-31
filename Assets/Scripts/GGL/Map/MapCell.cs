using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapCell:MonoBehaviour
{
    public Vector2Int startPos;//(0,0)
    public Vector2Int endPos;//(2,2)
    public List<Cell> cells = new List<Cell>();
    public bool showDebug = false;

    void Awake()
    {
        InitCell();
    }

    void InitCell()
    {
        int cellWidth = (int)(endPos.x - startPos.x);
        int cellHeight = (int)(endPos.y - startPos.y);
        for (int i = 0; i < cellWidth; i++)
        {
            for (int j = 0; j < cellHeight; j++)
            {
                cells.Add(new Cell(i, j));
            }
        }
    }

    /// <summary>
    /// 通过世界坐标获取单元格
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public Cell WorldToCell(Vector2 worldPos)
    {
        int cellWidth = (int)(endPos.x - startPos.x);
        int cellHeight = (int)(endPos.y - startPos.y);
        int x = (int)(worldPos.x - startPos.x);
        int y = (int)(worldPos.y - startPos.y);
        if (x >= 0 && x < cellWidth && y >= 0 && y < cellHeight)
        {
            return cells[x + y * cellWidth];
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 通过单元格获取世界坐标
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    public Vector2 CellToWorld(Cell cell)
    {
        return new Vector2(startPos.x + cell.x, startPos.y + cell.y);
    }

    public int CalGridDisByWorldPos(Vector2 worldPos1, Vector2 worldPos2)
    {
        Cell cell1 = WorldToCell(worldPos1);
        Cell cell2 = WorldToCell(worldPos2);
        Debug.Log($"cell1.x:{cell1.x},cell1.y:{cell1.y},cell2.x:{cell2.x},cell2.y:{cell2.y}");
        if (cell1 != null && cell2 != null)
        {
            int x = Mathf.Abs(cell1.x - cell2.x);
            int y = Mathf.Abs(cell1.y - cell2.y);
            Debug.Log($"Distance:{x+y}");
            return x + y;
        }
        else
        {
            Debug.LogError("CalGridDisByWorldPos: cell1 or cell2 is null");
            return int.MaxValue;
        }
    }

    /// <summary>
    /// 在编辑器中绘制格子的可视化效果
    /// </summary>
    void OnDrawGizmos()
    {
        if (!showDebug)
        {
            return;
        }
        // 如果格子列表为空，尝试初始化（仅在编辑器模式下）
        if (cells.Count == 0)
        {
            InitCell();
        }

        // 设置绘制颜色
        Gizmos.color = Color.green;

        // 绘制每个格子
        foreach (Cell cell in cells)
        {
            // 获取格子的世界坐标
            Vector2 cellWorldPos = CellToWorld(cell);
            
            // 计算格子的四个角
            Vector3 bottomLeft = new Vector3(cellWorldPos.x, cellWorldPos.y, 0);
            Vector3 bottomRight = new Vector3(cellWorldPos.x + 1, cellWorldPos.y, 0);
            Vector3 topLeft = new Vector3(cellWorldPos.x, cellWorldPos.y + 1, 0);
            Vector3 topRight = new Vector3(cellWorldPos.x + 1, cellWorldPos.y + 1, 0);

            // 绘制格子边框
            Gizmos.DrawLine(bottomLeft, bottomRight);
            Gizmos.DrawLine(bottomRight, topRight);
            Gizmos.DrawLine(topRight, topLeft);
            Gizmos.DrawLine(topLeft, bottomLeft);

            // 在格子中心显示坐标
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontSize = 12;
            style.alignment = TextAnchor.MiddleCenter;
            
            Vector3 labelPos = new Vector3(cellWorldPos.x + 0.5f, cellWorldPos.y + 0.5f, -1);
            Handles.Label(labelPos, $"({cell.x},{cell.y})", style);
        }

        // 绘制整个地图的边界
        Gizmos.color = Color.red;
        Vector3 mapBottomLeft = new Vector3(startPos.x, startPos.y, 0);
        Vector3 mapBottomRight = new Vector3(endPos.x, startPos.y, 0);
        Vector3 mapTopLeft = new Vector3(startPos.x, endPos.y, 0);
        Vector3 mapTopRight = new Vector3(endPos.x, endPos.y, 0);

        Gizmos.DrawLine(mapBottomLeft, mapBottomRight);
        Gizmos.DrawLine(mapBottomRight, mapTopRight);
        Gizmos.DrawLine(mapTopRight, mapTopLeft);
        Gizmos.DrawLine(mapTopLeft, mapBottomLeft);
    }

    
}
