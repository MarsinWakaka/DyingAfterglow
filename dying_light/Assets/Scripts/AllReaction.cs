using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InventoryAccess : LeftClick
{
    private Tilemap fromcontainer = null;
    private Vector3Int fromslot = Vector3Int.zero;
    private GameObject icon;
    private Vector2 prevPosition = Vector2.zero;
    public void BreakMovement()
    {
        if (fromcontainer != null)
        {
            fromcontainer.SetColor(fromslot, Color.white);
            fromcontainer = null;
        }
        fromslot = Vector3Int.zero;
        UIManager.UI.isDragging = false;
    }

    public new IEnumerator DoSomething()
    {
        fromcontainer = null;
        GameUIUtil.TryPickUp(out fromcontainer, out fromslot);
        if (fromcontainer != null && fromcontainer.GetTile(fromslot) != null)
        {
            icon = GameUIUtil.SetFloatIcon(icon);
            icon.GetComponent<SpriteRenderer>().sprite = fromcontainer.GetSprite(fromslot);
            UIManager.UI.isDragging = true;
            yield return new WaitUntil(() => !UIManager.UI.isDragging);
            GameUIUtil.TryPutDown(fromcontainer, fromslot);
            GameObject.Destroy(icon);

        }
        yield break;
    }

    public override void DoSomethingContinually()
    {
        if (prevPosition != (Vector2)Input.mousePosition)
        {
            icon.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        prevPosition = Input.mousePosition;
    }

    public override void InteractWith(){ }

}

public class OpenContainer : RightClick
{
    public override void DoSomething()
    {
    }

    public override void DoSomethingContinually()
    {
    }

    public override void InteractWith()
    {
    }
}

public class ShowItemInfo : RightClick
{
    public override void DoSomething()
    {
    }
    
    public override void DoSomethingContinually()
    {
    }

    public override void InteractWith()
    {
    }
}

public class PlayerRun : MoveMent
{
    public PlayerRun(Subject _character)
    {
        subject = _character;
    }

    public override void DoSomething()
    {
    }

    public override void DoSomethingContinually()
    {
        subject.isIdle = false;
        if (GetAxisRaw("Horizontal") == 0 && GetAxisRaw("Vertical") == 0)
        {
            subject.rg.velocity = Vector2.zero;
            subject.isIdle = true;
        }
        else
        {
            subject.rg.velocity = new Vector2(GetAxisRaw("Horizontal"), GetAxisRaw("Vertical")).normalized * subject.speed;
            CheckFlip(subject);
        }
    }

    public override void InteractWith()
    {
    }
}

public class Attack : LeftClick
{

}

public class Thump : LeftClick
{

}

public class Dodge : MoveMent
{
    public Dodge(Subject character)
    {
        subject = character;
    }
    public override void DoSomething()
    {
        if (subject.isIdle)
        {
            subject.rg.velocity = new Vector2(subject.rg.transform.localScale.x, 0).normalized * subject.speed * 2;
        }
        else
        {
            subject.rg.velocity = new Vector2(GetAxisRaw("Horizontal"), GetAxisRaw("Vertical")).normalized * subject.speed * 2;
        }
    }

    public override void DoSomethingContinually()
    {
    }

    public override void InteractWith()
    {
    }

}