using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftClick : React
{
    public static KeyCode hotkey = HotKeys.universalkeys["LeftClick"];

    public virtual void DoSomething() { }

    public virtual void DoSomethingContinually() { }

    public virtual void InteractWith() { }
}

public abstract class RightClick : React
{
    public static KeyCode hotkey = HotKeys.universalkeys["RightClick"];

    public virtual void DoSomething() { }

    public virtual void DoSomethingContinually() { }

    public virtual void InteractWith() { }
}

public abstract class MoveMent : React
{
    public static KeyCode moveup = HotKeys.gamingkeys["MoveUp"];
    public static KeyCode moveright = HotKeys.gamingkeys["MoveRight"];
    public static KeyCode moveleft = HotKeys.gamingkeys["MoveLeft"];
    public static KeyCode movedown = HotKeys.gamingkeys["MoveDown"];
    public static KeyCode dodge = HotKeys.gamingkeys["Space"];
    public Subject subject = null;

    public virtual void DoSomething() { }

    public virtual void DoSomethingContinually() { }

    public virtual void InteractWith() { }

    public static int GetAxisRaw(string axis)
    {
        if (axis == "Horizontal")
        {
            return Convert.ToInt32(Input.GetKey(moveright)) - Convert.ToInt32(Input.GetKey(moveleft));
        }
        else if (axis == "Vertical")
        {
            return Convert.ToInt32(Input.GetKey(moveup)) - Convert.ToInt32(Input.GetKey(movedown));
        }
        else
        {
            return 0;
        }
    }

    public static void CheckFlip(MonoBehaviour subject)
    {
        if (Input.GetKey(moveright))
        {
            subject.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (Input.GetKey(moveleft))
        {
            subject.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public static void CheckFlip(MonoBehaviour subject, bool flag)
    {
        if (flag)
        {
            subject.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            subject.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
