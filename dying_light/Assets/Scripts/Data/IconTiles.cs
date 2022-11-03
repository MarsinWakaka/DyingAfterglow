using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Icon", menuName = "ObjectsAddedbyZhiJ/Icon")]
public class IconTiles : Tile
{
    [HideInInspector] public Vector2Int iconsize;
    public string itemName;
    public string itemDiscription;
}