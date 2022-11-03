using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Container", menuName = "ObjectsAddedbyZhiJ/Container")]
public class StorageTiles : Tile
{
    public Texture containerGUI;
    public PivotPosition pivotPosition;
    [HideInInspector]
    [SerializeField]
    public GridPositionDictionary slots;
    [HideInInspector]
    [SerializeField]
    public PositionBooleanDictionary slotsFlag;

    public void ResetDic()                                                                              //重置
    {
        for (int i = 0; i < slots.Count; i++)
        {
            KeyValuePair<Vector3Int, Vector3Int> kv = slots.ElementAt(i);
            slots[kv.Key] = kv.Key;
        }

        for (int i = 0; i < slotsFlag.Count; i++)
        {
            KeyValuePair<Vector3Int, Vector3Int> kv = slots.ElementAt(i);
            slotsFlag[kv.Key] = false;
        }
    }

    public Vector3Int FixPosition(int i, int j, Vector2Int originPosition)                              //根据锚点修正网格坐标系原点
    {
        switch (pivotPosition)
        {
            case PivotPosition.LeftTop:
                return new Vector3Int(i + originPosition.x - 1, -j - originPosition.y);
            case PivotPosition.LeftBottom:
                return new Vector3Int(i + originPosition.x - 1, j + originPosition.y - 1);
            case PivotPosition.RightBottom:
                return new Vector3Int(-i - originPosition.x, j + originPosition.y - 1);
            default:
                return new Vector3Int(-i - originPosition.x, -j - originPosition.y);
        }
    }
}