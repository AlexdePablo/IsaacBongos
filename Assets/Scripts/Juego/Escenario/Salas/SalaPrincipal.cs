using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacBongos
{
    public class SalaPrincipal : TipoSala
    {
        protected override void SpawnerSala()
        {
            throw new System.NotImplementedException();
        }

        // Start is called before the first frame update
        void Start()
        {
            m_CanOpenDoor = true;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}