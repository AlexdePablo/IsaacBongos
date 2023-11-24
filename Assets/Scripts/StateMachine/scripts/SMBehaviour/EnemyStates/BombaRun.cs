using m17;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombaRun : MBState
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
    private JefeStateMachine jefe;

    void Awake()
    {
        jefe = GetComponent<JefeStateMachine>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();

    }

    public override void Init() { 
        base.Init();
        m_Animator.Play("bombaRun");
    }

   
    void Update()
    {
        if (jefe != null) {
            int random = Random.Range(0, 2);
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
            if (Vector2.Distance(transform.position, m_Target.transform.position) < 4f)
            {
                m_StateMachine.ChangeState<BombaApunto>();
            } else if (Vector2.Distance(transform.position, m_Target.transform.position) < 5f) {
                if (random == 1) {
                    m_StateMachine.ChangeState<JefeAgujeroNegro>();
                }
                return;
            
            }
        }
        else {
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

            if (Vector2.Distance(transform.position, m_Target.transform.position) < 2f)
            {
                m_StateMachine.ChangeState<BombaApunto>();
            }
        }


    }
}
