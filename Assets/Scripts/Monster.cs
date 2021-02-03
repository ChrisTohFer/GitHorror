using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    enum State
    {
        IDLE,
        CHASING,
        ATTACKING,
        MOVING,
        HITSTUN,
        DEAD
    }
    
    //
    public DetectionArea DetectionArea;
    public NavMeshAgent Agent;
    public Collider Collider;
    public Animator Animator;
    public AudioSource AudioSource;
    public float Health = 100f;
    public float AttackDuration;
    public float AttackActivationTime;
    public float AttackRange;
    public float Damage = 10f;
    public float StunDuration;
    public float IdleSoundProbabilityPerSecond = 0.1f;

    //
    Vector3 m_startPosition;
    State m_state = State.IDLE;
    float m_stateTimer = 0f;
    Transform playerTransform;
    bool m_attackActivated = false;
    float m_timeSinceLastIdleSound = 0f;

    //
    private void Start()
    {
        m_startPosition = transform.position;
        playerTransform = PlayerManager.Singleton.transform;
    }

    private void FixedUpdate()
    {
        if (Health <= 0f && m_state != State.DEAD)
            SetDead();

        switch (m_state)
        {
            case State.IDLE:
                UpdateIdle();
                break;
            case State.CHASING:
                UpdateChasing();
                break;
            case State.ATTACKING:
                UpdateAttacking();
                break;
            case State.MOVING:
                UpdateMoving();
                break;
            case State.HITSTUN:
                UpdateHitstun();
                break;
            case State.DEAD:
                UpdateDead();
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        Health = Mathf.Clamp(Health - damage, 0f, 10000f);
        if (Health > 0f)
            SetHitstun();
    }

    //Per-state update methods
    public void UpdateIdle()
    {
        m_timeSinceLastIdleSound += Time.fixedDeltaTime;
        if (m_timeSinceLastIdleSound > 4f && Random.value < IdleSoundProbabilityPerSecond * Time.fixedDeltaTime)
        {
            AudioManager.Play(AudioManager.AudioClips.EnemyIdle, AudioSource);
        }
        if (DetectionArea.ContainsPlayer)
        {
            SetChasing();
        }
    }
    public void UpdateChasing()
    {
        if(!DetectionArea.ContainsPlayer)
        {
            //Stop chasing if player leaves
            SetMoving();
        }    
        else if (Vector3.Distance(transform.position, playerTransform.position) < AttackRange)
        {
            //Start attacking if player in range
            SetAttacking();
        }
        else
        {
            //Update path if player still needs chasing
            NavMeshPath path = new NavMeshPath();
            if (Agent.CalculatePath(playerTransform.position, path) && path.status == NavMeshPathStatus.PathComplete)
            {
                Agent.SetPath(path);
            }
            else
                SetMoving();    //Reset to moving if we can't reach the player (this will be repeatedly called each frame but it's probably fine...)
        }
    }
    public void UpdateAttacking()
    {
        transform.LookAt(playerTransform);
        //Attack state is controlled by animation
        m_stateTimer -= Time.fixedDeltaTime;
        if(m_stateTimer < AttackDuration - AttackActivationTime && !m_attackActivated && Vector3.Distance(transform.position, playerTransform.position) < AttackRange)
        {
            PlayerManager player = playerTransform.GetComponent<PlayerManager>();
            player.TakeDamage(Damage);
            m_attackActivated = true;
        }
        if (m_stateTimer < 0)
        {
            SetChasing();
        }
    }
    public void UpdateMoving()
    {
        if (Vector3.Distance(transform.position, m_startPosition) < 0.5f)
        {
            SetIdle();
        }
        else if (DetectionArea.ContainsPlayer)
            SetChasing();
    }
    public void UpdateHitstun()
    {
        //Hitstun state is controlled by animation
        m_stateTimer -= Time.fixedDeltaTime;
        if (m_stateTimer < 0)
        {
            if(DetectionArea.ContainsPlayer)
            {
                SetChasing();
            }
            else
            {
                SetMoving();
            }
        }
    }
    public void UpdateDead()
    {
        //Do nothing
    }

    //State transitions
    public void SetIdle()
    {
        m_timeSinceLastIdleSound = 0f;
        Animator.SetInteger("Animation", 0);
        m_state = State.IDLE;
    }
    public void SetChasing()
    {
        Animator.SetInteger("Animation", 1);
        NavMeshPath path = new NavMeshPath();
        if (Agent.CalculatePath(playerTransform.position, path) && path.status == NavMeshPathStatus.PathComplete)
        {
            Agent.SetPath(path);
            if(m_state != State.HITSTUN && m_state != State.ATTACKING)    //Don't play from hitstun, the sound will play repeatedly
                AudioManager.Play(AudioManager.AudioClips.EnemySpottedPlayer, AudioSource);
        }
        else
            SetMoving();    //Reset to moving if we can't reach the player (this will be repeatedly called each frame but it's probably fine...)
        
        m_state = State.CHASING;
    }
    public void SetAttacking()
    {
        Animator.SetInteger("Animation", 2);
        m_state = State.ATTACKING;
        m_stateTimer = AttackDuration;
        Agent.ResetPath();
        m_attackActivated = false;
        AudioManager.Play(AudioManager.AudioClips.EnemyAttacking, AudioSource);
    }
    public void SetMoving()
    {
        Animator.SetInteger("Animation", 1);
        m_state = State.MOVING;
        NavMeshPath path = new NavMeshPath();
        if (Agent.CalculatePath(m_startPosition, path) && path.status == NavMeshPathStatus.PathComplete)
        {
            Debug.Log(path.status == NavMeshPathStatus.PathComplete);
            Agent.SetPath(path);
        }
        else
            Agent.ResetPath();
    }
    public void SetHitstun()
    {
        Animator.SetInteger("Animation", 0);
        m_state = State.HITSTUN;
        m_stateTimer = StunDuration;
        Agent.ResetPath();
    }
    public void SetDead()
    {
        Animator.SetInteger("Animation", 3);
        m_state = State.DEAD;
        Agent.ResetPath();
        Collider.enabled = false;
        AudioManager.Play(AudioManager.AudioClips.EnemyDeath, AudioSource);
    }
}
