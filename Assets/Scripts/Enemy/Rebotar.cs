using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rebotar : MonoBehaviour
{
    private Rigidbody2D m_RigidBody;
    // Start is called before the first frame update
    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_RigidBody.velocity = transform.up * 3f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Pared") {
            RaycastHit2D hit;
            Vector2 direction =  transform.position - collision.transform.position;
            hit = Physics2D.Raycast(transform.position, direction,1f);
            if (hit.collider != null) {
                Vector2 newDirection = Vector2.Reflect(transform.up, hit.normal);
                Debug.Log(hit.normal);
                transform.up = newDirection;
                m_RigidBody.velocity = transform.up * 3f;
            }
        }
    }
}
