using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilController : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
            GetComponent<Poolable>().ReturnToPool();
    }

}
