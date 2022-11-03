using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[CustomEditor(typeof(StorageTiles))]
public class StorageTilesInspectorGUI : Editor
{
    Vector2Int areaSize = Vector2Int.zero;
    Vector2Int originPosition = Vector2Int.zero;
    bool buttonPlus = false;
    bool buttonMinus = false;
    bool buttonReset = false;
    public override void OnInspectorGUI()
    {
        StorageTiles container = (StorageTiles)target;
        base.OnInspectorGUI();
        areaSize = EditorGUILayout.Vector2IntField("区域尺寸:   ", areaSize);
        originPosition = EditorGUILayout.Vector2IntField("左下位置:    ", originPosition);
        buttonPlus = GUILayout.Button("添加新存储区域");
        buttonMinus = GUILayout.Button("删除全部");
        buttonReset = GUILayout.Button("重置");
        for (int i = 0; i < container.slots.Count; i++)
        {
            KeyValuePair<Vector3Int, Vector3Int> groups = container.slots.ElementAt(i);
            KeyValuePair<Vector3Int, bool> flags = container.slotsFlag.ElementAt(i);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(((Vector2Int)groups.Value).ToString());
            EditorGUILayout.LabelField((flags.Value).ToString());
            EditorGUILayout.EndHorizontal();
        }
        if (buttonPlus)
        {
            for (int j = 0; j < areaSize.y; j++)
            {
                for (int i = 0; i < areaSize.x; i++)
                {
                    Vector3Int temp = container.FixPosition(i, j, originPosition);
                    container.slots.Add(temp, temp);
                    container.slotsFlag.Add(temp, false);
                }
            }
        }
        if (buttonMinus)
        {
            container.slots.Clear();
            container.slotsFlag.Clear();
        }
        if (buttonReset)
        {
            container.ResetDic();
        }
    }
}


[CustomEditor(typeof(IconTiles))]
public class IconTilesInspectorGUI : Editor
{
    public override void OnInspectorGUI()
    {
        IconTiles iconTiles = (IconTiles)target;
        base.OnInspectorGUI();
        iconTiles.iconsize = EditorGUILayout.Vector2IntField("IconSize:", iconTiles.iconsize);
        if (GUILayout.Button("应用默认设置"))
        {
            iconTiles.iconsize = new Vector2Int((int)(iconTiles.sprite.texture.width / 32), (int)(iconTiles.sprite.texture.height / 32));
            iconTiles.flags = TileFlags.None;
            iconTiles.itemName = iconTiles.sprite.name;
        }
    }
}
