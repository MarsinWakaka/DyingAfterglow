using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameUIUtil
{

    public static void SwitchAllGUI()
    {
        for (int i = 0; i < UIManager.UI.transform.childCount; i++)
        {
            GUISwitch(i);
        }
    }

    public static void SwitchAllGUI(bool state)
    {
        for (int i = 0; i < UIManager.UI.transform.childCount; i++)
        {
            GUISwitch(i, state);
        }
    }

    public static bool GUISwitch(int index)
    {
        UIManager.UI.transform.GetChild(index).gameObject.SetActive(!UIManager.UI.transform.GetChild(index).gameObject.activeSelf);
        return UIManager.UI.transform.GetChild(index).gameObject.activeSelf;
    }

    public static bool GUISwitch(int index, bool state)
    {
        UIManager.UI.transform.GetChild(index).gameObject.SetActive(state);
        return state;
    }

    public static bool TryPickUp(out Tilemap containMap, out Vector3Int indexPosition)                                                       //尝试从GUI中取起物品图标
    {
        RaycastHit hitInfo;
        Ray lookingAt = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3Int pointedPosition;
        containMap = null;
        indexPosition = Vector3Int.zero;
        if (Physics.Raycast(lookingAt, out hitInfo))
        {
            if (hitInfo.transform.tag == "GUI")
            {
                Dictionary<Vector3Int, Vector3Int> keyValuePositions =
                    hitInfo.transform.GetComponent<ContainerMap>().container.slots;                                                         //获取字典

                pointedPosition = (Vector3Int)(Vector2Int)hitInfo.transform.GetComponent<Grid>().
                    WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));                                                       //获取鼠标所在槽位

                if (!keyValuePositions.ContainsKey(pointedPosition))
                {
                    return false;
                }

                containMap = hitInfo.transform.GetComponent<Tilemap>();

                indexPosition = keyValuePositions[pointedPosition];                                                                         //根据鼠标所在槽位从该容器的字典中查找索引槽位
                if (containMap.GetTile(indexPosition) == null)
                {
                    return false;
                }                                                                                                                           //如果鼠标点击位置没有瓦片则返回false

                containMap.SetColor(indexPosition, new Color(1, 1, 1, 0.7f));                                                               //成功取其物品，将原槽位物品透明度降低
                return true;
            }
        }                                                                                                                                   //当取起成功时返回true
        return false;
    }

    public static bool TryPutDown(Tilemap fromcontainMap, Vector3Int fromSlot)                                                               //尝试从GUI中放下物品图标
    {
        //yield return new WaitUntil(() => UIManager.UI.isDragging);
        RaycastHit hitInfo;
        Ray lookingAt = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(lookingAt, out hitInfo))
        {
            if (hitInfo.transform.tag == "GUI")
            {
                #region  获取物品(icontoput) 和 其来源位置信息(fromcontainmap, fromslot) 以及 目标(iconreceived) 和 其位置信息(containmap, indexposition)
                Dictionary<Vector3Int, Vector3Int> keyValuePositions =
                    hitInfo.transform.GetComponent<ContainerMap>().container.slots;                                                         //获取字典

                IconTiles icontoput = (IconTiles)fromcontainMap.GetTile(fromSlot);                                                          //获取fromcontainer上fromslot网格的瓦片

                Vector3Int pointedPosition = (Vector3Int)(Vector2Int)hitInfo.transform.GetComponent<Grid>().
                    WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));                                                       //获取鼠标所在的网格坐标

                if (!keyValuePositions.ContainsKey(pointedPosition))
                {
                    return false;
                }
                Vector3Int indexposition = keyValuePositions[pointedPosition];                                                              //根据鼠标所在网格从字典中获取索引网格

                fromcontainMap.SetTile(fromSlot, null);                                                                                     //删除源网格位置上的瓦片
                Tilemap containMap = hitInfo.transform.GetComponent<Tilemap>();                                                             //获取当前的tilemap
                IconTiles iconreceived = (IconTiles)containMap.GetTile(indexposition);                                                      //获取目标网格的瓦片
                #endregion

                #region 没有目标物品且目标区域有效

                SingleGridMap.ResetSlot(fromcontainMap.gameObject, fromSlot, icontoput.iconsize);                                           //重置源区域的网格索引关系
                if (iconreceived == null)
                {
                    if (SingleGridMap.CheckArea(containMap.gameObject, pointedPosition, icontoput.iconsize))
                    {
                        #region 放置物品
                        containMap.SetTile(pointedPosition, icontoput);
                        SingleGridMap.BindSlotsTo(containMap.gameObject, pointedPosition, icontoput.iconsize);
                        return true;
                        #endregion
                    }
                    SingleGridMap.BindSlotsTo(fromcontainMap.gameObject, fromSlot, icontoput.iconsize);                                    //失败后重新绑定源区域索引
                    fromcontainMap.SetTile(fromSlot, icontoput);                                                                           //失败后放回源瓦片
                    return false;
                }
                #endregion

                #region 目标物品为自身
                #endregion

                #region 有目标物品
                else if (iconreceived != null)
                {
                    SingleGridMap.ResetSlot(containMap.gameObject, indexposition, iconreceived.iconsize);
                    #region 比较源物品与目标物品的尺寸,若源物品长宽均大于等于目标物品
                    if (icontoput.iconsize.x >= iconreceived.iconsize.x && icontoput.iconsize.y >= iconreceived.iconsize.y)
                    {
                        #region 尝试交换物品后检查目标区域有效性
                        SingleGridMap.BindSlotsTo(fromcontainMap.gameObject, fromSlot, iconreceived.iconsize);
                        if (SingleGridMap.CheckArea(containMap.gameObject, indexposition, icontoput.iconsize))
                        {
                            #region 交换源与目标
                            containMap.SetTile(indexposition, icontoput);
                            SingleGridMap.BindSlotsTo(containMap.gameObject, indexposition, icontoput.iconsize);
                            fromcontainMap.SetTile(fromSlot, iconreceived);
                            fromcontainMap.SetColor(fromSlot, Color.white);
                            return true;
                            #endregion
                        }
                        SingleGridMap.BindSlotsTo(fromcontainMap.gameObject, fromSlot, icontoput.iconsize);
                        SingleGridMap.BindSlotsTo(containMap.gameObject, indexposition, iconreceived.iconsize);
                        fromcontainMap.SetTile(fromSlot, icontoput);
                        return false;
                        #endregion
                    }
                    #endregion
                    #region 比较源物品与目标物品的尺寸,若源物品长宽均小于目标物品
                    else if (icontoput.iconsize.x <= iconreceived.iconsize.x && icontoput.iconsize.y <= iconreceived.iconsize.y)
                    {
                        #region 尝试交换物品后检查源区域有效性
                        SingleGridMap.BindSlotsTo(containMap.gameObject, indexposition, icontoput.iconsize);
                        if (SingleGridMap.CheckArea(fromcontainMap.gameObject, fromSlot, iconreceived.iconsize))
                        {
                            #region 交换源与目标
                            containMap.SetTile(indexposition, icontoput);
                            fromcontainMap.SetTile(fromSlot, iconreceived);
                            SingleGridMap.BindSlotsTo(fromcontainMap.gameObject, fromSlot, iconreceived.iconsize);
                            fromcontainMap.SetColor(fromSlot, Color.white);
                            return true;
                            #endregion
                        }
                        SingleGridMap.BindSlotsTo(fromcontainMap.gameObject, fromSlot, icontoput.iconsize);
                        SingleGridMap.BindSlotsTo(containMap.gameObject, indexposition, iconreceived.iconsize);
                        fromcontainMap.SetTile(fromSlot, icontoput);
                        return false;
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
        }
        return false;
    }

    internal static void TryPause() { }

    public static void SetGUIsize(List<GameObject> GUIList)
    {
        foreach (var item in GUIList)
        {
            item.transform.localScale *= SavesManager.configdic["UGUI"];
        }
    }

    public static GameObject SetFloatIcon(GameObject icon)
    {
        icon = (GameObject)GameObject.Instantiate(Resources.Load("FloatIcon"), UIManager.UI.transform);
        icon.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return icon;
    }

}