using m17;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilEnemigoController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!typeof(SMBParryState).IsInstanceOfType(collision.gameObject.GetComponent<PJSMB>().StateMachine.CurrentState))
            {
                GetComponent<Poolable>().ReturnToPool();
            }

        }
        else {
            GetComponent<Poolable>().ReturnToPool();
        }

    }
}
