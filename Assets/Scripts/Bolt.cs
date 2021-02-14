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
    public ParticleSystem particleHit;
    private float particleLifeTime;

    float m_timeUnderSpeed = 0;

    private void Awake()
    {
        // particleLifeTime = particleHit.main.duration;
        particleLifeTime = 0.1f;
    }

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
        if (collision.gameObject.layer == 8) //MAGIC NUMBERS SUCK
        {
            if (collision.gameObject.tag == "Head")
            {
                Monster monster = collision.gameObject.GetComponentInParent<Monster>();
                monster.TakeDamage(Damage * HeadShotMultiplier);
                AudioManager.Play(AudioManager.AudioClips.EnemyHeadshot, monster.AudioSource);
                particleHit.Play();
                Rigidbody.isKinematic = true;

            }
            else
            {
                Monster monster = collision.gameObject.GetComponent<Monster>();
                monster.TakeDamage(Damage);
                AudioManager.Play(AudioManager.AudioClips.EnemyBodyshot, monster.AudioSource);
                particleHit.Play();
                Rigidbody.isKinematic = true;
            }

            StartCoroutine(DestroyBolt());
        }
        else
        {
            Rigidbody.velocity = Rigidbody.velocity / 5f;
        }
    }

    IEnumerator DestroyBolt()
    {
        yield return new WaitForSeconds(particleLifeTime);
        Destroy(gameObject);
    }
}
