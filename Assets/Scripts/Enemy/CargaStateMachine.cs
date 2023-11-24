using IsaacBongos;
using m17;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CargaRun))]
[RequireComponent(typeof(CargaCarga))]
[RequireComponent(typeof(CargaCargando))]
[RequireComponent(typeof(Aturdido))]
public class CargaStateMachine : MonoBehaviour
{
    private HealthController m_healthController;
    private FiniteStateMachine m_StateMachine;
    public Action muerteEnemigo;
    public Action desuscribirEnemigo;
    [SerializeField]
    private GameObject monedillaPrefab;
    void Awake()
    {
        m_healthController = GetComponent<HealthController>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
    }
    void Start()
    {
        GetComponentInParent<SalaEnemigos>().OnPLayerInSala += EmpezarPersecucion;

    }

    private void EmpezarPersecucion()
    {
        m_StateMachine.ChangeState<CargaRun>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ProjectilPlayer"))
        {
            print(collision.name);
            m_healthController.Damage(collision.gameObject.GetComponent<Daño>().daño);
            if (m_healthController.HP == 0)
            {
                muerteEnemigo?.Invoke();
                Destroy(gameObject);
            }
        }

    }

    private void OnDestroy()
    {
        GameObject monedilla = Instantiate(monedillaPrefab, transform.parent);
        monedilla.transform.position = transform.position;
        desuscribirEnemigo?.Invoke();
    }
    public void Aturdirse()
    {
        m_StateMachine.ChangeState<Aturdido>(); 
    }
}
