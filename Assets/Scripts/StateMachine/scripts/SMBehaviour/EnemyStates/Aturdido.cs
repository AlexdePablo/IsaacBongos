using m17;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Aturdido : MBState
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
    }
    public override void Init()
    {
        base.Init();
        m_Animator.Play("aturdido");
        m_Rigidbody.velocity = Vector3.zero;
        StartCoroutine(Aturdir());
    }

    public override void Exit()
    {
        base.Exit();
        StopAllCoroutines();
    }
    private IEnumerator Aturdir() {
        yield return new WaitForSeconds(2f);
        m_StateMachine.ChangeState<CargaRun>();
    }
}
