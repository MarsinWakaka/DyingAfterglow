using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Subject : MonoBehaviour
{
    #region 可受buff影响的变量

    #region 暂未使用
    [HideInInspector] public float health;                          //生命值
    [HideInInspector] public float protection;                      //防护值,多数情况先掉防护值再掉生命值,防护值由护甲提供
    [HideInInspector] public float damageAffector;                  //伤害影响因子,实际伤害=(基础伤害+武器面板伤害)*伤害影响因子
    [HideInInspector] public float speedAffector;                   //速度影响因子,实际速度=基础速度*速度影响因子
    #endregion 

    [HideInInspector] public bool isIdle = true;                    
    [HideInInspector] public FSM thisFSM;                           //状态机,可通过状态机的currentState变量实现类似沉默甚至操控其他对象的效果
    #endregion

    [HideInInspector] public float speed;                           //基础速度
    [HideInInspector] public float baseDamage;                      //基础伤害,即无武器状态下的未加成伤害  *暂未使用*
    [HideInInspector] public Rigidbody2D rg;
    [HideInInspector] public Animator anim;
}
