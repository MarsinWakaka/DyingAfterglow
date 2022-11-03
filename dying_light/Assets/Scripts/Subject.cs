using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Subject : MonoBehaviour
{
    #region ����buffӰ��ı���

    #region ��δʹ��
    [HideInInspector] public float health;                          //����ֵ
    [HideInInspector] public float protection;                      //����ֵ,��������ȵ�����ֵ�ٵ�����ֵ,����ֵ�ɻ����ṩ
    [HideInInspector] public float damageAffector;                  //�˺�Ӱ������,ʵ���˺�=(�����˺�+��������˺�)*�˺�Ӱ������
    [HideInInspector] public float speedAffector;                   //�ٶ�Ӱ������,ʵ���ٶ�=�����ٶ�*�ٶ�Ӱ������
    #endregion 

    [HideInInspector] public bool isIdle = true;                    
    [HideInInspector] public FSM thisFSM;                           //״̬��,��ͨ��״̬����currentState����ʵ�����Ƴ�Ĭ�����ٿ����������Ч��
    #endregion

    [HideInInspector] public float speed;                           //�����ٶ�
    [HideInInspector] public float baseDamage;                      //�����˺�,��������״̬�µ�δ�ӳ��˺�  *��δʹ��*
    [HideInInspector] public Rigidbody2D rg;
    [HideInInspector] public Animator anim;
}
