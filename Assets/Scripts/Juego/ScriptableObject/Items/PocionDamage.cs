using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacBongos
{
    [CreateAssetMenu(fileName = "Buff Damage Item", menuName = "Inventory/Items/Pocion De Damage")]
    public class PocionDamage : Item
    {
        [Header("Damage Buff Item values")]
        [SerializeField]
        [Min(0f)]
        private float m_BuffDamage;
        [SerializeField]
        [Min(0f)]
        private float m_TimeBuffDamage;
        public override bool UsedBy(GameObject go)
        {
            if (!go.TryGetComponent(out IBuuffable buffable))
                return false;
            
            buffable.BuffDamage(m_BuffDamage, m_TimeBuffDamage);
            return true;
        }
    }
}