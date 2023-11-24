using m17;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBMuertoState : MBState
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
    }

    public override void Init()
    {
        base.Init();
        m_Animator.Play("playerMuerto");
        m_Rigidbody.velocity = Vector3.zero;
    }

}
