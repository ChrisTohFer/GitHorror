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
    public float AttackDuration;
    public float StunDuration;
    public float AttackRange;
    public Transform playerTransform;

    //
    Vector3 m_startPosition;
    State m_state = State.IDLE;
    float m_stateTimer = 0f;

    //
    private void Start()
    {
        m_startPosition = transform.position;
    }

    private void FixedUpdate()
    {
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

    //Per-state update methods
    public void UpdateIdle()
    {
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
        //Attack state is controlled by animation
        m_stateTimer -= Time.fixedDeltaTime;
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
        Animator.SetInteger("Animation", 0);
        m_state = State.IDLE;
    }
    public void SetChasing()
    {
        Animator.SetInteger("Animation", 0);
        m_state = State.CHASING;
        NavMeshPath path = new NavMeshPath();
        if (Agent.CalculatePath(playerTransform.position, path) && path.status == NavMeshPathStatus.PathComplete)
        {
            Agent.SetPath(path);
        }
        else
            SetMoving();    //Reset to moving if we can't reach the player (this will be repeatedly called each frame but it's probably fine...)
        
    }
    public void SetAttacking()
    {
        Animator.SetInteger("Animation", 1);
        m_state = State.ATTACKING;
        m_stateTimer = AttackDuration;
        Agent.ResetPath();
    }
    public void SetMoving()
    {
        Animator.SetInteger("Animation", 0);
        m_state = State.MOVING;
        NavMeshPath path = new NavMeshPath();
        if (Agent.CalculatePath(m_startPosition, path) && path.status == NavMeshPathStatus.PathComplete)
        {
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
        Animator.SetInteger("Animation", 0);
        m_state = State.DEAD;
        Agent.ResetPath();
        Collider.enabled = false;
    }
}
