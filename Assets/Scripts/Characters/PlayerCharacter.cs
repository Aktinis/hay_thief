using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerCharacter : Character
{
    private const float MOVE_THRESHOLD = 0.4f;

    [SerializeField] private ColliderBroadcastWrapper attackCollider = null;
    private List<GameObject> hitEnemies = new List<GameObject>();

    private bool canTriggerAttack = true;
    public Action DoDamage { get; set; }

    public void Move()
    {
        HandleDirection();
        if (target != null)
        {
            SetDestination(target.transform.position);
        }
    }

    public void OnMove(Vector3 pos)
    {
        if (Vector3.Distance(pos, transform.GetTopDownPosition()) >= MOVE_THRESHOLD)
        {
            SetDestination(pos);
        }
    }

    private void HandleOnTriggerEnter(Collider obj)
    {
        if (obj.transform.GetLayerTranslatedToLayerMaskValue() == globalDatabaseService.GetEnemyLayerMaskValue)
        {
            if (!hitEnemies.Contains(obj.gameObject))
            {
                if(target && obj.gameObject == target.gameObject)
                {
                    target = null;
                    agent.ResetPath();
                }
                Stop(true);
                hitEnemies.Add(obj.gameObject);
                if(canTriggerAttack)
                {
                    animator.SetTrigger(ATTACK_BOOL_NAME);
                    canTriggerAttack = false;
                }

            }
        }
    }

    private void DamageEnemies()
    {
        foreach (var enemy in hitEnemies)
        {
            enemy.GetComponent<AICharacter>().TakeDamage();
        }
        hitEnemies.Clear();
    }

    private void Start()
    {
        var onAttackSMB = animator.GetBehaviour<OnAttackSMB>();
        onAttackSMB.DamageEnemies = DamageEnemies;
        onAttackSMB.OnExit = () => 
        {
            Stop(false);
            canTriggerAttack = true;
        };
        attackCollider.OnTriggerEnterEvent += HandleOnTriggerEnter;
    }

    private void OnDestroy()
    {
        attackCollider.OnTriggerEnterEvent -= HandleOnTriggerEnter;
    }
}
