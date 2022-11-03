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

    public static bool TryPickUp(out Tilemap containMap, out Vector3Int indexPosition)                                                       //���Դ�GUI��ȡ����Ʒͼ��
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
                    hitInfo.transform.GetComponent<ContainerMap>().container.slots;                                                         //��ȡ�ֵ�

                pointedPosition = (Vector3Int)(Vector2Int)hitInfo.transform.GetComponent<Grid>().
                    WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));                                                       //��ȡ������ڲ�λ

                if (!keyValuePositions.ContainsKey(pointedPosition))
                {
                    return false;
                }

                containMap = hitInfo.transform.GetComponent<Tilemap>();

                indexPosition = keyValuePositions[pointedPosition];                                                                         //����������ڲ�λ�Ӹ��������ֵ��в���������λ
                if (containMap.GetTile(indexPosition) == null)
                {
                    return false;
                }                                                                                                                           //��������λ��û����Ƭ�򷵻�false

                containMap.SetColor(indexPosition, new Color(1, 1, 1, 0.7f));                                                               //�ɹ�ȡ����Ʒ����ԭ��λ��Ʒ͸���Ƚ���
                return true;
            }
        }                                                                                                                                   //��ȡ��ɹ�ʱ����true
        return false;
    }

    public static bool TryPutDown(Tilemap fromcontainMap, Vector3Int fromSlot)                                                               //���Դ�GUI�з�����Ʒͼ��
    {
        //yield return new WaitUntil(() => UIManager.UI.isDragging);
        RaycastHit hitInfo;
        Ray lookingAt = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(lookingAt, out hitInfo))
        {
            if (hitInfo.transform.tag == "GUI")
            {
                #region  ��ȡ��Ʒ(icontoput) �� ����Դλ����Ϣ(fromcontainmap, fromslot) �Լ� Ŀ��(iconreceived) �� ��λ����Ϣ(containmap, indexposition)
                Dictionary<Vector3Int, Vector3Int> keyValuePositions =
                    hitInfo.transform.GetComponent<ContainerMap>().container.slots;                                                         //��ȡ�ֵ�

                IconTiles icontoput = (IconTiles)fromcontainMap.GetTile(fromSlot);                                                          //��ȡfromcontainer��fromslot�������Ƭ

                Vector3Int pointedPosition = (Vector3Int)(Vector2Int)hitInfo.transform.GetComponent<Grid>().
                    WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));                                                       //��ȡ������ڵ���������

                if (!keyValuePositions.ContainsKey(pointedPosition))
                {
                    return false;
                }
                Vector3Int indexposition = keyValuePositions[pointedPosition];                                                              //�����������������ֵ��л�ȡ��������

                fromcontainMap.SetTile(fromSlot, null);                                                                                     //ɾ��Դ����λ���ϵ���Ƭ
                Tilemap containMap = hitInfo.transform.GetComponent<Tilemap>();                                                             //��ȡ��ǰ��tilemap
                IconTiles iconreceived = (IconTiles)containMap.GetTile(indexposition);                                                      //��ȡĿ���������Ƭ
                #endregion

                #region û��Ŀ����Ʒ��Ŀ��������Ч

                SingleGridMap.ResetSlot(fromcontainMap.gameObject, fromSlot, icontoput.iconsize);                                           //����Դ���������������ϵ
                if (iconreceived == null)
                {
                    if (SingleGridMap.CheckArea(containMap.gameObject, pointedPosition, icontoput.iconsize))
                    {
                        #region ������Ʒ
                        containMap.SetTile(pointedPosition, icontoput);
                        SingleGridMap.BindSlotsTo(containMap.gameObject, pointedPosition, icontoput.iconsize);
                        return true;
                        #endregion
                    }
                    SingleGridMap.BindSlotsTo(fromcontainMap.gameObject, fromSlot, icontoput.iconsize);                                    //ʧ�ܺ����°�Դ��������
                    fromcontainMap.SetTile(fromSlot, icontoput);                                                                           //ʧ�ܺ�Ż�Դ��Ƭ
                    return false;
                }
                #endregion

                #region Ŀ����ƷΪ����
                #endregion

                #region ��Ŀ����Ʒ
                else if (iconreceived != null)
                {
                    SingleGridMap.ResetSlot(containMap.gameObject, indexposition, iconreceived.iconsize);
                    #region �Ƚ�Դ��Ʒ��Ŀ����Ʒ�ĳߴ�,��Դ��Ʒ��������ڵ���Ŀ����Ʒ
                    if (icontoput.iconsize.x >= iconreceived.iconsize.x && icontoput.iconsize.y >= iconreceived.iconsize.y)
                    {
                        #region ���Խ�����Ʒ����Ŀ��������Ч��
                        SingleGridMap.BindSlotsTo(fromcontainMap.gameObject, fromSlot, iconreceived.iconsize);
                        if (SingleGridMap.CheckArea(containMap.gameObject, indexposition, icontoput.iconsize))
                        {
                            #region ����Դ��Ŀ��
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
                    #region �Ƚ�Դ��Ʒ��Ŀ����Ʒ�ĳߴ�,��Դ��Ʒ�����С��Ŀ����Ʒ
                    else if (icontoput.iconsize.x <= iconreceived.iconsize.x && icontoput.iconsize.y <= iconreceived.iconsize.y)
                    {
                        #region ���Խ�����Ʒ����Դ������Ч��
                        SingleGridMap.BindSlotsTo(containMap.gameObject, indexposition, icontoput.iconsize);
                        if (SingleGridMap.CheckArea(fromcontainMap.gameObject, fromSlot, iconreceived.iconsize))
                        {
                            #region ����Դ��Ŀ��
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