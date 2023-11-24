using IsaacBongos;
using m17;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SMBShootingState : MBState
{
    [SerializeField]
    private GameObject m_Projectil;
    private PJSMB m_PJ;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private StatsController m_StatsController;
    Vector3 m_Pointer;
    private bool m_Shooting;

    private Pool m_Pool;
    public Pool Pool { get { return m_Pool; } set { m_Pool = value; } }
    private Vector2 m_Movement;

    [SerializeField]
    private float m_Speed = 3;
    private void Awake()
    {  
        m_Shooting = true;
        m_PJ = GetComponent<PJSMB>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_StatsController = GetComponent<StatsController>();
    }

    public override void Init()
    {
        base.Init();
        m_Animator.Play("shootingPlayer");
        StartCoroutine(ShootCorutine());
    }

    private IEnumerator ShootCorutine() {
        m_Shooting=true;
        yield return new WaitForSeconds(0.1f);
        m_Shooting = false;
    }
    public void Shoot()
    {
        if (m_Pool.ThereAreElements())
        {
            if (m_PJ.Jugador1)
            {
                GameObject projectil = m_Pool.GetElement();
                projectil.transform.position = transform.position;
                m_Pointer = m_PJ.PointerAction.ReadValue<Vector2>();
                Vector3 position = Camera.main.ScreenToWorldPoint(m_Pointer);
                Vector2 direccion = (position - transform.position).normalized;
                projectil.transform.up = direccion;
                projectil.GetComponent<Daño>().daño = m_StatsController.DAMAGE;
                projectil.GetComponent<Rigidbody2D>().velocity = projectil.transform.up * 10f;
            }
            else {
                GameObject projectil = m_Pool.GetElement();
                projectil.transform.position = transform.position;
                m_Pointer = m_PJ.PointerAction.ReadValue<Vector2>();
                float angulo = Mathf.Atan2(m_Pointer.y, m_Pointer.x);
                Vector2 direccionProyectil = new Vector2(Mathf.Cos(angulo), Mathf.Sin(angulo));
                projectil.GetComponent<Daño>().daño = m_StatsController.DAMAGE;
                projectil.GetComponent<Rigidbody2D>().velocity = direccionProyectil * 10f;
            }
       
        }
        else
            return;

    }

    private void Update()
    {
        m_Movement = m_PJ.MovementAction.ReadValue<Vector2>();

        if (m_Movement == Vector2.zero && !m_Shooting)
            m_StateMachine.ChangeState<SMBIdleState>();
    }

    private void FixedUpdate()
    {
        m_Rigidbody.velocity = Vector2.right * m_Movement * m_Speed;
        if (!m_Shooting) { 
            m_StateMachine.ChangeState<SMBWalkState>();           
        }
    }

}
