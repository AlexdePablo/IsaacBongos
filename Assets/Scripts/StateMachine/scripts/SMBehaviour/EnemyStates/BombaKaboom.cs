using m17;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombaKaboom : MBState
{
    private Transform m_Target;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    Collider2D overlap;
    [SerializeField]
    private LayerMask m_LayerMask;
    private Pool m_Pool;
    public Pool Pool
    {
        get { return m_Pool; }
        set { m_Pool = value; }
    }
    void Awake()
    {
      
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();

    }

    public override void Init()
    {
        base.Init();
        m_Animator.Play("bombaKaboom");
        m_Rigidbody.velocity = Vector3.zero;
        StartCoroutine(Explotar());
      
    }

    IEnumerator Explotar() {
        GameObject projectil_1 = m_Pool.GetElement();
        projectil_1.transform.position = transform.position;
        GameObject projectil_2 = m_Pool.GetElement();
        projectil_2.transform.position = transform.position;
        GameObject projectil_3 = m_Pool.GetElement();
        projectil_3.transform.position = transform.position;
        GameObject projectil_4 = m_Pool.GetElement();
        projectil_4.transform.position = transform.position;
        GameObject projectil_5 = m_Pool.GetElement();
        projectil_5.transform.position = transform.position;
        GameObject projectil_6 = m_Pool.GetElement();
        projectil_6.transform.position = transform.position;
        GameObject projectil_7 = m_Pool.GetElement();
        projectil_7.transform.position = transform.position;
        GameObject projectil_8 = m_Pool.GetElement();
        projectil_8.transform.position = transform.position;
        projectil_1.GetComponent<Rigidbody2D>().velocity = Vector3.up * 3f;
        projectil_2.GetComponent<Rigidbody2D>().velocity = new Vector3(1,1,0) * 3f;
        projectil_3.GetComponent<Rigidbody2D>().velocity = Vector3.right * 3f;
        projectil_4.GetComponent<Rigidbody2D>().velocity = new Vector3(1,-1,0) * 3f;
        projectil_5.GetComponent<Rigidbody2D>().velocity = -Vector3.up * 3f;
        projectil_6.GetComponent<Rigidbody2D>().velocity = new Vector3(-1,-1,0) * 3f;
        projectil_7.GetComponent<Rigidbody2D>().velocity = -Vector3.right * 3f;
        projectil_8.GetComponent<Rigidbody2D>().velocity = new Vector3(-1,1,0) * 3f;
        yield return new WaitForSeconds(1f);
        overlap = Physics2D.OverlapCircle(transform.position, 3f, m_LayerMask);
        if (overlap != null)
        {
            if (Vector2.Distance(transform.position, overlap.transform.position) < 2f)
            {
                m_StateMachine.ChangeState<BombaApunto>();
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
