using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Transform m_target;
    [SerializeField] float speed;
    GameObject m_gameObject;
    Rigidbody2D Rigidbody2Dm_Rigidbody2;
    // Start is called before the first frame update
    private void Awake()
    {
        m_target = GameObject.FindGameObjectWithTag("Player").transform;
        m_gameObject = m_target.gameObject;
        Rigidbody2Dm_Rigidbody2 = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 direction = (m_target.position - transform.position).normalized;
        Rigidbody2Dm_Rigidbody2.velocity = direction * speed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == m_gameObject)
        {
            Attack();
        }
    }

    private void Attack()
    {
        Debug.Log("Attack Player!!");
    }
}
