using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotKeys
{
    public static Dictionary<string, KeyCode> universalkeys = new Dictionary<string, KeyCode>()
    {
        { "LeftClick", KeyCode.Mouse0},
        { "RightClick", KeyCode.Mouse1},
    };
    public static Dictionary<string, KeyCode> GUIkeys = new Dictionary<string, KeyCode>
    {
        { "Inventory", KeyCode.E},
        { "PlayerInfo", KeyCode.I},
    };

    public static Dictionary<string, KeyCode> gamingkeys = new Dictionary<string, KeyCode>()
    {
        { "Dodge", KeyCode.Space },
        { "MoveUp", KeyCode.W },
        { "MoveLeft", KeyCode.A },
        { "MoveDown", KeyCode.S },
        { "MoveRight", KeyCode.D },
        { "Space", KeyCode.Space },
    };
}
