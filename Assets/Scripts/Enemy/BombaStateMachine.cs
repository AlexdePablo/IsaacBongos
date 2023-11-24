using IsaacBongos;
using m17;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BombaRun))]
[RequireComponent(typeof(BombaKaboom))]
[RequireComponent(typeof(BombaApunto))]
public class BombaStateMachine : MonoBehaviour
{
    private HealthController m_healthController;
    private FiniteStateMachine m_StateMachine;
    public Action muerteEnemigo;
    public Action desuscribirEnemigo;
    [SerializeField]
    private GameObject monedillaPrefab;
    [SerializeField]
    private Pool m_pool;
    public Pool Pool { get { return m_pool; }
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
        GetComponentInParent<SalaEnemigos>().OnPLayerInSala += EmpezarPersecucion;
       
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
            if(m_healthController.HP == 0) {
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


}
