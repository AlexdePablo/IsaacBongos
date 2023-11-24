using m17;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargaCargando : MBState
{
    private Transform m_Target;
    Collider2D overlap;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    [SerializeField]
    private LayerMask m_LayerMask;
    void Awake()
    {

        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();

    }

    public override void Init()
    {
        base.Init();
        m_Animator.Play("CargaCargando");
        m_Rigidbody.velocity = Vector3.zero;
        StartCoroutine(Esperar());
    }



    IEnumerator Esperar()
    {

        yield return new WaitForSeconds(1f);
        overlap = Physics2D.OverlapCircle(transform.position, 6f, m_LayerMask);
        if (overlap != null)
        {
            if (Vector2.Distance(transform.position, overlap.transform.position) < 5f)
            {
                m_StateMachine.ChangeState<CargaCarga>();
            }
            else
            {

                m_StateMachine.ChangeState<CargaRun>();
            }
        }
        else
        {
            m_StateMachine.ChangeState<CargaRun>();
        }


    }
}
