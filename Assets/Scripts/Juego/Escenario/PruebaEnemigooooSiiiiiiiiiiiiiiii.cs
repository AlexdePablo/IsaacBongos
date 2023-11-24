using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacBongos
{
    public class PruebaEnemigooooSiiiiiiiiiiiiiiii : MonoBehaviour
    {
        public Action muerteEnemigo;
        public Action desuscribirEnemigo;
        [SerializeField]
        private GameObject monedillaPrefab;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            muerteEnemigo?.Invoke();
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            GameObject monedilla = Instantiate(monedillaPrefab, transform.parent);
            monedilla.transform.position = transform.position;
            desuscribirEnemigo?.Invoke();
        }
    }
}