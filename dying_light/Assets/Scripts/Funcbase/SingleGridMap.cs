using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SingleGridMap
{
    public static void Delete(Tilemap containMap, Vector3Int indexPosition, Vector2Int groupSize)                                           //删除网格
    {
        ResetSlot(containMap.gameObject, indexPosition, groupSize);
        containMap.SetTile(indexPosition, null);
    }

    public static bool CheckArea(GameObject container, Vector3Int indexPosition, Vector2Int groupSize)                                       //检查区域的占用和越界情况
    {
        if (!container.GetComponent<ContainerMap>().container.slots.ContainsKey(indexPosition + new Vector3Int(groupSize.x - 1, -groupSize.y + 1)))
        {
            return false;
        }
        for (int j = 0; j < groupSize.y; j++)
        {
            for (int i = 0; i < groupSize.x; i++)
            {
                Vector3Int temp = indexPosition + new Vector3Int(i, -j);
                if (container.GetComponent<ContainerMap>().container.slotsFlag[temp])
                {
                    return false;
                }
            }
        }
        return true;
    }

    public static void BindSlotsTo(GameObject container, Vector3Int indexPosition, Vector2Int groupSize)                                     //绑定一个网格组
    {
        for (int j = 0; j < groupSize.y; j++)
        {
            for (int i = 0; i < groupSize.x; i++)
            {
                container.GetComponent<ContainerMap>().container.slots[indexPosition + new Vector3Int(i, -j)] = indexPosition;
                container.GetComponent<ContainerMap>().container.slotsFlag[indexPosition + new Vector3Int(i, -j)] = true;
            }
        }
    }

    public static void ResetSlot(GameObject container, Vector3Int indexPosition, Vector2Int groupSize)                                       //重置一个网格组
    {
        for (int j = 0; j < groupSize.y; j++)
        {
            for (int i = 0; i < groupSize.x; i++)
            {
                container.GetComponent<ContainerMap>().container.slots[indexPosition + new Vector3Int(i, -j)] = indexPosition + new Vector3Int(i, -j);
                container.GetComponent<ContainerMap>().container.slotsFlag[indexPosition + new Vector3Int(i, -j)] = false;
            }
        }
    }
}
