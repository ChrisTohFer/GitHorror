using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    public float Damage = 40f;
    public float SpeedThreshold = 40f;
    public float DisappearTime = 2f;
    public Rigidbody Rigidbody;

    float m_timeUnderSpeed = 0;

    private void FixedUpdate()
    {
        if (Rigidbody.velocity.magnitude < SpeedThreshold)
        {
            m_timeUnderSpeed += Time.fixedDeltaTime;
            if (m_timeUnderSpeed > DisappearTime)
                Destroy(gameObject);
        }
        else
            m_timeUnderSpeed = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 8) //MAGIC NUMBERS SUCK
        {
            //Enemy
            Monster monster = collision.gameObject.GetComponent<Monster>();
            monster.Health = Mathf.Clamp(monster.Health - Damage, 0f, 10000f);
            Destroy(gameObject);
        }
        else
        {
            Rigidbody.velocity = Rigidbody.velocity / 5f;
        }
    }
}
