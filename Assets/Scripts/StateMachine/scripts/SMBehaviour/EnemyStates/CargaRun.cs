using m17;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargaRun : MBState
{
    [SerializeField]
    private Transform m_Target;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    Collider2D[] overlap;
    [SerializeField]
    private LayerMask m_LayerMask;
    private float minDistance;

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();

    }

    public override void Init()
    {
        base.Init();
        m_Animator.Play("CargaRun");
    }


    void Update()
    {
        float minDistance = Mathf.Infinity;
        overlap = Physics2D.OverlapCircleAll(transform.position, Mathf.Infinity, m_LayerMask);
        foreach (Collider2D col in overlap)
        {
            if (Vector2.Distance(transform.position, col.transform.position) < minDistance)
            {
                m_Target = col.transform;
                minDistance = Vector2.Distance(transform.position, col.transform.position);
            }
        }
        if (m_Target != null)
        {
            Vector2 direction = m_Target.position - transform.position;
            transform.right = direction;
            m_Rigidbody.velocity = transform.right * 3f;

        }

        if (Vector2.Distance(transform.position, m_Target.transform.position) < 5f)
        {
            m_StateMachine.ChangeState<CargaCargando>();
        }

    }
}
