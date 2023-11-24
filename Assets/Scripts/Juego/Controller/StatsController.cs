using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacBongos
{
    public class StatsController : MonoBehaviour, IBuuffable
    {
        private const float MAXSPEED = 10f;

        [SerializeField]
        private float m_SPEED = MAXSPEED;
        public float SPEED => m_SPEED;

        private const float MAXDAMAGE = 10f;

        [SerializeField]
        private float m_DAMAGE = MAXDAMAGE;
        public float DAMAGE => m_DAMAGE;

        public void BuffDamage(float buffDamage, float buffTime)
        {
           StartCoroutine(CBuffDamage(buffDamage, buffTime));
        }

        public void BuffSpeed(float buffSpeed, float buffTime)
        {
            StartCoroutine(CBuffSpeed(buffSpeed, buffTime));
        }
        private IEnumerator CBuffSpeed(float buffSpeed, float buffTime)
        {
            float segundos = 0;
            m_SPEED += buffSpeed;
            if (m_SPEED > 20)
                m_SPEED = 20;

            Debug.Log(string.Format("Received {0} buff speed. Remaining SPEED: {1}", buffSpeed, m_SPEED));
            while (segundos < buffTime)
            {
                segundos += 0.2f;
                yield return new WaitForSeconds(0.2f);
            }
            m_SPEED -= buffSpeed;
            Debug.Log(string.Format("Received {0} buff speed. Remaining SPEED: {1}", buffSpeed, m_SPEED));
        }
        private IEnumerator CBuffDamage(float buffDamage, float buffTime)
        {
            float segundos = 0;
            m_DAMAGE += buffDamage;
            if (m_DAMAGE > 20)
                m_DAMAGE = 20;

            Debug.Log(string.Format("Received {0} buff damage. Remaining DAMAGE: {1}", buffDamage, m_DAMAGE));
            while (segundos < buffTime)
            {
                segundos += 0.2f;
                yield return new WaitForSeconds(0.2f);
            }
            m_DAMAGE -= buffDamage;
            Debug.Log(string.Format("Received {0} buff damage. Remaining DAMAGE: {1}", buffDamage, m_DAMAGE));
        }
    }
}