using m17;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargaCarga : MBState
{
    private Transform m_Target;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    Collider2D overlap;
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
        m_Animator.Play("CargaCarga");
        StartCoroutine(Carga());

    }

    IEnumerator Carga()
    {

        overlap = Physics2D.OverlapCircle(transform.position, 6f, m_LayerMask);
        if (overlap != null)
        {
            Vector2 direction = overlap.transform.position - transform.position;
            transform.up = direction;
            m_Rigidbody.velocity = transform.up * 10f;
        }
        
        yield return new WaitForSeconds(0.5f);
        m_Rigidbody.velocity = Vector3.zero;
        overlap = Physics2D.OverlapCircle(transform.position, 6f, m_LayerMask);
        if (overlap != null)
        {
            if (Vector2.Distance(transform.position, overlap.transform.position) < 2f)
            {
                m_StateMachine.ChangeState<CargaCargando>();
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
