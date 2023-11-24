using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacBongos
{
    [CreateAssetMenu(fileName = "Buff Velocidad Item", menuName = "Inventory/Items/PocionDeVelocidad")]
    public class PocionVelocidad : Item
    {
        [Header("Speed Buff Item values")]
        [SerializeField]
        [Min(0f)]
        private float m_BuffSpeed;
        [SerializeField]
        [Min(0f)]
        private float m_TimeBuffSpeed;
        public override bool UsedBy(GameObject go)
        {
            if (!go.TryGetComponent(out IBuuffable buffable))
                return false;

            buffable.BuffSpeed(m_BuffSpeed, m_TimeBuffSpeed);
            return true;
        }
    }
}
