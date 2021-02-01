using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [HideInInspector]
    public float m_damage, m_duration;

    private void FixedUpdate()
    {
        m_duration -= Time.fixedDeltaTime;
        if (m_duration < 0f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerManager player = other.gameObject.GetComponent<PlayerManager>();
        if(player != null)
        {
            //Player is in area
            player.TakeDamage(m_damage);
            Destroy(gameObject);
        }
    }
}
