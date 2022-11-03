using System;
using UnityEngine;

public class IdleState : IStatement
{
    Player player;

    public IdleState(Player player)
    {
        this.player = player;
    }

    public void OnEnter()
    {
        player.anim.Play("idle");
        player.rg.velocity = Vector2.zero;
    }

    public void OnUpdate() 
    {
        if (Input.GetKeyDown(MoveMent.dodge))
        {
            player.thisFSM.TransitionState(PlayerState.dodge);
        }
        else if (Input.GetKeyDown(LeftClick.hotkey))
        {
            player.thisFSM.TransitionState(PlayerState.charge);
        }
        else if (MoveMent.GetAxisRaw("Horizontal") != 0 || MoveMent.GetAxisRaw("Vertical") != 0)
        {
            player.thisFSM.TransitionState(PlayerState.runState);
        }
    }

    public void OnExit()
    { 
        
    }
}

public class RunState : IStatement
{
    Player player;
    PlayerRun run;

    public RunState(Player _player)
    {
        run = new PlayerRun(_player);
        player = _player;
    }

    public void OnEnter()
    {
        player.anim.Play("run");
    }

    public void OnUpdate()
    {
        run.DoSomethingContinually();

        if (Input.GetKeyDown(HotKeys.gamingkeys["Dodge"]))
        {
            player.thisFSM.TransitionState(PlayerState.dodge);
        }
        else if (Input.GetKeyDown(LeftClick.hotkey))
        {
            player.thisFSM.TransitionState(PlayerState.charge);
        }
        else if (player.isIdle)
        {
            player.thisFSM.TransitionState(PlayerState.idleState);
        }
    }

    public void OnExit()
    {

    }
}

public class ChargeState : IStatement
{
    Player player;
    Attack attack;
    PlayerRun run;
    float timer;
    //参数
    //float Damage_Multipler = 1.45f;

    public ChargeState(Player _player)
    {
        player = _player;
        attack = new Attack();
        run = new PlayerRun(player);
    }

    public void OnEnter()
    {
        player.anim.Play("attack_charged");
        //
        // 发射 射线，判断攻击(若是持续判定，写在OnUpdate里)
        //
    }

    public void OnUpdate()
    {
        run.DoSomethingContinually();
        MoveMent.CheckFlip(player);

        player.rg.velocity *= 0.87f;
        timer += Time.deltaTime;
        if (Input.GetKeyDown(HotKeys.gamingkeys["Dodge"]))
        {
            player.thisFSM.TransitionState(PlayerState.dodge);
        }
        else if (Input.GetKey(LeftClick.hotkey))
        {
            timer += Time.deltaTime;
        }
        else if (Input.GetKeyUp(LeftClick.hotkey))
        {
            if (timer < 1.2f)
            {
                player.anim.Play("attack01");
                attack.DoSomething();
                player.thisFSM.TransitionState(PlayerState.idleState);
            }
            else
            {
                player.thisFSM.TransitionState(PlayerState.thump);
            }
        }
    }

    public   void OnExit() 
    {
        timer = 0f;
    }
}

public class ThumpState : IStatement
{
    Player player;
    Thump thump;
    PlayerRun run;

    float timer;
    bool isDodge;

    public ThumpState(Player _player)
    {
        player = _player;
        thump = new Thump();
        run = new PlayerRun(_player);
    }

    public void OnEnter()
    {
        player.anim.Play("thump");
        player.speed *= 0.5f;
        thump.DoSomething();
        //
        // 发射 射线，判断攻击(若是持续判定，写在OnUpdate里)
        //
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;
        run.DoSomethingContinually();
        MoveMent.CheckFlip(player);

        //闪避
        if (Input.GetKeyDown(MoveMent.dodge))
        {
            isDodge = true;
        }
        //重击时间结束
        if (2.2f < timer)
        {
            player.thisFSM.TransitionState(PlayerState.idleState);
        }
        //必须等攻击动作结束后，也就是从按下攻击键的0.84s后，才能切换状态
        else if (0.84f < timer)
        {
            if (isDodge)
            {
                player.thisFSM.TransitionState(PlayerState.dodge);
            }
        }
    }
    public void OnExit()
    {
        timer = 0f;
        isDodge = false;
        player.speed *= 2f;
    }
}

//翻滚,闪避
public class DodgeState : IStatement
{

    Player player;
    Dodge dodge;
    float timer;

    public DodgeState(Player player)
    {
        this.player = player;
        dodge = new Dodge(player);
    }

    public void OnEnter()
    {
        timer = 0f;
        player.anim.Play("dodge");
        dodge.DoSomething();
        //
        // 
        //
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;
        //俗称打断后摇
        if (0.08f < timer && timer < 0.1f)
        {
            if (MoveMent.GetAxisRaw("Horizontal") > 0f || MoveMent.GetAxisRaw("Vertical") > 0f)
            {
                player.thisFSM.TransitionState(PlayerState.runState);
            }
        }
        else if (0.1f < timer)
        {
            player.thisFSM.TransitionState(PlayerState.idleState);
        }
    }
    public void OnExit()
    {

    }
}

public class DeathState : IStatement
{
    Player player;
    float timer;

    public DeathState(Player player)
    {
        this.player = player;
    }

    public   void OnEnter()
    {
        player.anim.Play("death");
    }

    public   void OnUpdate()
    {
        //复活控制
        timer += Time.deltaTime;

        if (timer > 1.5f)
        {
            player.anim.enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            player.anim.enabled = true;
            player.thisFSM.TransitionState(PlayerState.idleState);
        }
    }

    public   void OnExit()
    {
        timer = 0f;
    }
}

public class TakeHit : IStatement
{
    Player player;
    float timer;

    public TakeHit(Player player)
    {
        this.player = player;
    }

    public   void OnEnter()
    {
        player.anim.Play("takeHit");
        player.rg.velocity = Vector2.zero;
    }

    public   void OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer > 0.66f)
        {
            player.thisFSM.TransitionState(PlayerState.idleState);
        }
    }

    public   void OnExit()
    {
        timer = 0f;
    }
}