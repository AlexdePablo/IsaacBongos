using IsaacBongos;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacBongos
{
    public class HealthController : MonoBehaviour, IHealable, IDamageable
    {
        private const float MAXHP = 100f;

        [SerializeField]
        private float m_HP = MAXHP;
        public float HP => m_HP;

        public Action onCambioDeVida;

        public void Damage(float damageAmount)
        {
            m_HP -= damageAmount;
            if (m_HP < 0)
                m_HP = 0;

            onCambioDeVida?.Invoke();

        }

        public void SetVida(float _HP)
        {
            m_HP = _HP;
            if (m_HP < 0)
                m_HP = 0;

            if (m_HP > 100)
                m_HP = 100;

            onCambioDeVida?.Invoke();
        }

        public void Heal(float healAmount)
        {
            m_HP += healAmount;
            if (m_HP > 100)
                m_HP = 100;

            onCambioDeVida?.Invoke();

        }

        public void HealByTime(float healAmount, float healTime)
        {
            StartCoroutine(CHealByTime(healAmount, healTime));
        }
        private IEnumerator CHealByTime(float healAmount, float healTime)
        {
            float segundos = 0;
            int cont = 0;
            while (segundos < healTime)
            {
                cont++;
                segundos += 0.2f;

                if (cont % 5 == 0)
                    Heal(healAmount);
                yield return new WaitForSeconds(0.2f);
            }
           
        }
    }
}