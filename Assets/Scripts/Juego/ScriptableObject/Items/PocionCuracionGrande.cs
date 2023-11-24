using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacBongos
{
    [CreateAssetMenu(fileName = "Curacion Item", menuName = "Inventory/Items/PocionDeCuracionGrande")]
    public class PocionCuracionGrande : Item
    {
        [Header("Healing Item values")]
        [SerializeField]
        [Min(0f)]
        private float m_Healing;

        public override bool UsedBy(GameObject go)
        {
            if (!go.TryGetComponent(out IHealable healable))
                return false;

            healable.Heal(m_Healing);
            return true;
        }
    }
}