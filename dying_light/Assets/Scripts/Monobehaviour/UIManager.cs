using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager UI;                                                                                         //µ¥Àý·ÃÎÊÇþµÀ


    InventoryAccess inventoryAccess;
    public List<GameObject> GUIList;
    public bool isDragging = false;

    void Awake()
    {
        Application.targetFrameRate = 50;

        UI = this;
        SavesManager.GetGameConfig();
        GameUIUtil.SetGUIsize(GUIList);

        inventoryAccess = new InventoryAccess();
    }

    void FixedUpdate()
    {

        if (Input.GetKeyDown(HotKeys.GUIkeys["Inventory"]))
        {
            if (!GameUIUtil.GUISwitch(0))
            {
                StopCoroutine(inventoryAccess.DoSomething());
                inventoryAccess.BreakMovement();
            }
        }
        else if (Input.GetKeyDown(HotKeys.GUIkeys["PlayerInfo"]))
        {
            GameUIUtil.GUISwitch(1);
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameUIUtil.SwitchAllGUI(false);
            GameUIUtil.TryPause();
        }


        if (Input.GetKeyDown(LeftClick.hotkey))
        {
            if (isDragging == false)
            {
                StartCoroutine(inventoryAccess.DoSomething());
            }
            else
            {
                isDragging = false;
            }
        }
        if (isDragging)
        {
            inventoryAccess.DoSomethingContinually();
        }
    }
}