using System;
using UnityEngine;

public class OnAttackSMB : StateMachineBehaviour
{
    private readonly float ANIMATION_TIME_KEY_VALUE = 0.35f;

    public Action DamageEnemies { get; set; }
    public Action OnExit { get; set; }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(stateInfo.normalizedTime >= ANIMATION_TIME_KEY_VALUE)
        {
            DamageEnemies?.Invoke();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnExit?.Invoke();
    }
}
