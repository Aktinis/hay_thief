using System;
using UnityEngine;

public delegate Vector3 GetPosition();

public sealed class AICharacter : Character
{
    private const float NAVIGATION_COOLDOWN = 1.5f;
    private const float PURSUIT_TIMER = 1.0f;
    private const float WALKING_SPEED = 2.3f;
    private const float RUNNING_SPEED = 5.0f;
    private const float TARGET_DISTANCE = 20.0f;

    private AIState activeState = AIState.Idle;
    private float currentNavigationCooldown = 0.0f;
    private float timeToPursuit = 0.0f;
    private GetPosition requestPosition = null;


    public override void Setup(Vector3 position, Quaternion rotation, Action onDeath)
    {
        base.Setup(position, rotation, onDeath);
        UpdateState(AIState.Idle);
        currentNavigationCooldown = 0.0f;
        timeToPursuit = 0.0f;
        target = null;
    }

    public void Setup(Vector3 position, Quaternion rotation, Action onDeath, GetPosition getPositionDelegate)
    {
        Setup(position, rotation, onDeath);
        this.requestPosition = getPositionDelegate;
    }

    public void UpdateLogic()
    {
        if (IsAlive)
        {
            if (CanPursuit())
            {
                UpdateState(AIState.Pursuit);
            }

            if (CanNavigate() && activeState == AIState.Idle && !HasPath())
            {
                UpdateState(AIState.Patrol);
                SetDestination(requestPosition.Invoke());
                currentNavigationCooldown = NAVIGATION_COOLDOWN;
            }

            if (activeState == AIState.Patrol && !agent.pathPending && !HasPath())
            {
                UpdateState(AIState.Idle);
            }

            if (target != null)
            {
                SetDestination(target.transform.position);
                if (!CanPursuit())
                {
                    timeToPursuit += Time.deltaTime;
                }
                else
                {
                    timeToPursuit = 0.0f;
                }
                if (timeToPursuit > PURSUIT_TIMER)
                {
                    target = null;
                    agent.ResetPath();
                    currentNavigationCooldown = NAVIGATION_COOLDOWN;
                    UpdateState(AIState.Idle);
                }
            }

            if (activeState == AIState.Idle)
            {
                currentNavigationCooldown -= Time.deltaTime;
            }

            HandleDirection();
        }
    }

    private void UpdateState(AIState state)
    {
        if (state == activeState)
        {
            return;
        }
        activeState = state;
        UpdateSpeed();
    }

    private void UpdateSpeed()
    {
        if (activeState == AIState.Patrol)
        {
            agent.speed = WALKING_SPEED;

        }
        else if (activeState == AIState.Pursuit)
        {
            agent.speed = RUNNING_SPEED;
        }
    }

    private bool CanNavigate() => currentNavigationCooldown <= 0.0f;

    private bool CanPursuit()
    {
        if (Physics.Raycast(transform.GetTopDownPosition(), transform.forward, out var hit, TARGET_DISTANCE))
        {
            if (hit.transform.GetLayerTranslatedToLayerMaskValue() == globalDatabaseService.GetPlayerLayerMaskValue)
            {
                SetTarget(hit.transform);
                return true;
            }
        }
        return false;
    }
}