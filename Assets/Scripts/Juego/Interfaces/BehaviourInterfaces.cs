using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacBongos
{
    public interface IHealable
    {
        public void Heal(float healAmount);

        public void HealByTime(float healAmount, float healTime);
    }

    public interface IDamageable
    {
        public void Damage(float damageAmount);
    }

    public interface IBuuffable
    {
        public void BuffSpeed(float buffSpeed, float buffTime);

        public void BuffDamage(float buffDamage, float buffTime);
    }
}