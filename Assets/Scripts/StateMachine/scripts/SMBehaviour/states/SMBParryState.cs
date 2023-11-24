using m17;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBParryState : MBState
{
    private PJSMB m_PJ;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private Vector2 m_Movement;
    [SerializeField]
    private float m_Speed = 3;
    private Collider2D m_Collider;
    private AudioSource m_AudioSource;
    [SerializeField]
    private AudioClip m_AudioClip;

    void Awake()
    {
        m_Collider = GetComponent<Collider2D>();
        m_PJ = GetComponent<PJSMB>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSource.clip = m_AudioClip;
    }

    public override void Init()
    {
        base.Init();
        m_Animator.Play("parryPlayer");
        m_Rigidbody.velocity = Vector3.zero;
        StartCoroutine(Parry());
    }

    public override void Exit()
    {
        base.Exit();
        StopAllCoroutines();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!enabled) return;
        if (collision.CompareTag("ProjectilEnemigo")) {
            StopAllCoroutines();
            StartCoroutine(ParryProjectil(collision));
        }
        else if (collision.CompareTag("EnemyHitbox")) {
            StopAllCoroutines();
            StartCoroutine(ParryHitbox(collision.gameObject));
        }
    }

    private void ParryFinished()
    {
        m_Movement = m_PJ.MovementAction.ReadValue<Vector2>();

        if (m_Movement == Vector2.zero)
            m_StateMachine.ChangeState<SMBIdleState>();

        m_StateMachine.ChangeState<SMBWalkState>();
    }

    private IEnumerator Parry() { 
        yield return new WaitForSeconds(0.3f);
        ParryFinished();
    }

    private IEnumerator ParryProjectil(Collider2D collision) {
        m_Animator.Play("succesfulparryPlayer");
        m_AudioSource.Play();
        collision.GetComponent<Rigidbody2D>().velocity *= -1;
        yield return new WaitForSeconds(0.1f);
        ParryFinished();
    }

    private IEnumerator ParryHitbox(GameObject enemy) {
        m_Animator.Play("succesfulparryPlayer");
        m_AudioSource.Play();
        enemy.transform.parent.GetComponent<CargaStateMachine>().Aturdirse();
        yield return new WaitForSeconds(0.1f);
        ParryFinished();
    }
}
