using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace m17
{
    public class DummyBehaviour : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log(string.Format("I've been hit by {0} and it did {1} damage to me",
                collision.name,
                collision.gameObject.GetComponent<HitboxInfo>().Damage));
        }
    }
}

