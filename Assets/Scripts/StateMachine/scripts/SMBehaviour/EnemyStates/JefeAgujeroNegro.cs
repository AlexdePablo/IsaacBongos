using m17;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JefeAgujeroNegro : MBState
{
    [SerializeField]
    private GameObject m_agujeroNegro;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    Collider2D[] overlap;
    Collider2D overlapCercano;
    [SerializeField]
    private LayerMask m_LayerMask;
    // Start is called before the first frame update
    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
    }

    public override void Init()
    {
        base.Init();
        m_Animator.Play("bombaRun");
        StartCoroutine(agujero());
        m_Rigidbody.velocity = Vector3.zero;
    }
    private IEnumerator agujero() {

        overlap = Physics2D.OverlapCircleAll(transform.position, Mathf.Infinity, m_LayerMask);
        GameObject agujeroNegro = Instantiate(m_agujeroNegro);
        Vector3 puntoMedio = (overlap[0].transform.position + overlap[1].transform.position) / 2f;
        agujeroNegro.transform.position = puntoMedio;
        yield return new WaitForSeconds(3f);
        Destroy(agujeroNegro);
        overlapCercano = Physics2D.OverlapCircle(transform.position, 5f, m_LayerMask);
        if (overlapCercano != null)
        {
            if (Vector2.Distance(transform.position, overlapCercano.transform.position) < 3f)
            {
                m_StateMachine.ChangeState<BombaKaboom>();
            }
            else
            {
                m_StateMachine.ChangeState<BombaRun>();
            }
        }
        else
        {
            m_StateMachine.ChangeState<BombaRun>();
        }
        m_StateMachine.ChangeState<BombaRun>();


        
        
    }

    public override void Exit()
    {
        base.Exit();
        StopAllCoroutines();
    }
}
