using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected Collider col;
    [SerializeField] protected CharacterData data;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Animator animator;


    protected const string ATTACK_BOOL_NAME = "Attack1";
    protected const string RUNSPEED_FLOAT_NAME = "Run";
    protected const string DIE_BOOL_NAME = "Die";
    protected readonly Quaternion ROTATION_RIGHT = new Quaternion(0, 0.7071f, 0, 0.7071f);
    protected readonly Quaternion ROTATION_DOWN = new Quaternion(0, 1f, 0, 0);
    protected readonly Quaternion ROTATION_LEFT = new Quaternion(0, 0.7071f, 0, -0.7071f);
    protected readonly Quaternion ROTATION_UP = new Quaternion(0, 0, 0, -1f);
    protected const float MOVING_ROTATION_SPEED = 10f;
    protected const float IDLE_ROTATION_SPEED = 20f;

    protected Transform target = null;
    protected Action onDeath  = null;
    protected IGlobalDatabaseService globalDatabaseService = null;
    private bool shouldRotate = false;
    private float rotationSpeed = 0f;
    private Quaternion targetRotation = Quaternion.identity;
    private int health = 1;

    public virtual void Setup(Vector3 position, Quaternion rotation, Action onDeath)
    {
        this.onDeath = onDeath;
        target = null;
        col.enabled = true;
        agent.Warp(position);
        transform.rotation = rotation;
        health = data.Health;
        agent.updateRotation = false;
        agent.updatePosition = false;
    }

    public virtual void Die()
    {
        col.enabled = false;
        onDeath?.Invoke();
        animator.SetTrigger(DIE_BOOL_NAME);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public bool IsMoving()
    {
        return agent.velocity.sqrMagnitude > 0.0f;
    }

    public bool HasPath()
    {
        return agent.hasPath;
    }

    public void Stop(bool state)
    {
        agent.isStopped = state;
    }

    public void TakeDamage()
    {
        health--;
        if (!IsAlive)
        {
            Die();
        }
    }

    public virtual void UpdateAnimation()
    {
        animator.SetFloat(RUNSPEED_FLOAT_NAME, agent.velocity.sqrMagnitude);
    }

    public void SetDestination(Vector3 position)
    {
        agent.SetDestination(position);
    }

    public Vector3[] GetPath() => agent.path.corners;

    protected void HandleDirection()
    {
        var dirForward = transform.forward;
        RaycastHit hitForward;
        var rayForward = new Ray(transform.GetTopDownPosition(), dirForward);
        if (Physics.Raycast(rayForward, out hitForward, .6f) && !shouldRotate)
        {
            if (hitForward.transform.GetLayerTranslatedToLayerMaskValue() == globalDatabaseService.GetObstacleLaterMaskValue)
            {
                rotationSpeed = IDLE_ROTATION_SPEED;
                shouldRotate = true;
                float val = 0f;

                float valX = dirForward.x > 0 ? dirForward.x : dirForward.x * -1;
                float valZ = dirForward.z > 0 ? dirForward.z : dirForward.z * -1;

                if (dirForward.x >= 0f)
                {
                    if (val < valX)
                    {
                        val = valX;
                        targetRotation = ROTATION_LEFT;
                    }
                }
                if (dirForward.x <= 0f)
                {
                    if (val < valX)
                    {
                        val = valX;
                        targetRotation = ROTATION_RIGHT;
                    }
                }
                if (dirForward.z >= 0f)
                {
                    if (val < valZ)
                    {
                        val = valZ;
                        targetRotation = ROTATION_DOWN;
                    }
                }
                if (dirForward.z <= 0f)
                {
                    if (val < valZ)
                    {
                        targetRotation = ROTATION_UP;
                    }
                }
            }
        }

        if (IsMoving())
        {
            targetRotation = Quaternion.LookRotation(agent.velocity.normalized);
            shouldRotate = false;
            rotationSpeed = MOVING_ROTATION_SPEED;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        transform.position = agent.nextPosition;

    }
    protected bool IsAlive => health > 0;

    private void Awake()
    {
        globalDatabaseService = GlobalContainer.Get<IGlobalDatabaseService>();
    }
}