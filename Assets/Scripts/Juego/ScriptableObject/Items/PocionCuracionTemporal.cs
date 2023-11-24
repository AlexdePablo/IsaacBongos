using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacBongos
{
    [CreateAssetMenu(fileName = "Curacion Item Temporal", menuName = "Inventory/Items/PocionDeCuracionTemporal")]
    public class PocionCuracionTemporal : Item
    {
        [Header("HP Heal Item values")]
        [SerializeField]
        [Min(0f)]
        private float m_HPHeal;
        [SerializeField]
        [Min(0f)]
        private float m_TimeHeal;
        public override bool UsedBy(GameObject go)
        {
            if (!go.TryGetComponent(out IHealable healable))
                return false;

            healable.HealByTime(m_HPHeal, m_TimeHeal);
            return true;
        }
    }
}