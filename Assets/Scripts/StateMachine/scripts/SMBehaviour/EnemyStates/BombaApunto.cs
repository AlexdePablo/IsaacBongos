using m17;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombaApunto : MBState
{
    private Transform m_Target;
    Collider2D overlap;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    [SerializeField]
    private LayerMask m_LayerMask;
    void Awake() { 

         m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();

    }

    public override void Init()
    {
      base.Init();
      m_Animator.Play("bombaApunto");
      m_Rigidbody.velocity = Vector3.zero;  
      StartCoroutine(Esperar());  
    }



    IEnumerator Esperar() { 

        yield return new WaitForSeconds(0.5f);
        overlap = Physics2D.OverlapCircle(transform.position, 3f, m_LayerMask);
        if (overlap != null)
        {
            if (Vector2.Distance(transform.position, overlap.transform.position) < 2f)
            {
                m_StateMachine.ChangeState<BombaKaboom>();
            }
            else
            {
                m_StateMachine.ChangeState<BombaRun>();
            }
        }
        else {
            m_StateMachine.ChangeState<BombaRun>();
        }

      

    }

    public override void Exit()
    {
        base.Exit();
        StopAllCoroutines();
    }
}
