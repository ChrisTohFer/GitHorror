using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    public float Damage = 40f;
    public float SpeedThreshold = 40f;
    public float DisappearTime = 2f;
    public Rigidbody Rigidbody;
    public float HeadShotMultiplier = 3f;

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
            if(collision.gameObject.tag == "Head")
            {
                Monster monster = collision.gameObject.GetComponentInParent<Monster>();
                monster.TakeDamage(Damage * HeadShotMultiplier);
                AudioManager.Play(AudioManager.AudioClips.EnemyHeadshot, monster.AudioSource);
            }
            else
            {
                Monster monster = collision.gameObject.GetComponent<Monster>();
                monster.TakeDamage(Damage);
                AudioManager.Play(AudioManager.AudioClips.EnemyBodyshot, monster.AudioSource);
            }
            Destroy(gameObject);
        }
        else
        {
            Rigidbody.velocity = Rigidbody.velocity / 5f;
        }
    }
}
