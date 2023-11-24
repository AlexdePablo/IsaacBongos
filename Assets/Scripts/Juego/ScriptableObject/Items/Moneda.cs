using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacBongos
{

    public class Moneda : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                Destroy(gameObject);
        }
    }
}