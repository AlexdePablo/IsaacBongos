using IsaacBongos;
using m17;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BombaRun))]
[RequireComponent(typeof(BombaKaboom))]
[RequireComponent(typeof(BombaApunto))]
[RequireComponent (typeof(JefeAgujeroNegro))]
public class JefeStateMachine : MonoBehaviour
{
    private bool m_jefe = true;
    public bool Jefe => m_jefe;
    private HealthController m_healthController;
    private FiniteStateMachine m_StateMachine;
    [SerializeField]
    private GameObject monedillaPrefab;
    [SerializeField]
    private Pool m_pool;
    public Pool Pool
    {
        get { return m_pool; }
        set { m_pool = value; }
    }

    private void Awake()
    {
       
        m_healthController = GetComponent<HealthController>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInParent<SalaBoss>().OnPLayerInSala += EmpezarPersecucion;

    }

    private void EmpezarPersecucion()
    {
        m_StateMachine.ChangeState<BombaRun>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ProjectilPlayer"))
        {
            m_healthController.Damage(collision.gameObject.GetComponent<Daño>().daño);
            if (m_healthController.HP == 0)
            {
                Destroy(gameObject);
            }
        }

    }
    private void OnDestroy()
    {
        GameManager.Instance.AcabarPartida();
        GameObject monedilla = Instantiate(monedillaPrefab, transform.parent);
        monedilla.transform.position = transform.position;
    }
}
