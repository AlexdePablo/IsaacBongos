using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacBongos
{
    public class ItemScript : MonoBehaviour
    {
        [SerializeField]
        private Item m_Item;
        public Item Item => m_Item;

        public Action objetoAgarrado;
        public Action desuscribirObjeto;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                objetoAgarrado?.Invoke();
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            desuscribirObjeto?.Invoke();
        }
    }
}
