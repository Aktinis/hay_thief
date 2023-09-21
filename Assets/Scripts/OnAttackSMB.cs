using System;
using UnityEngine;

public class OnAttackSMB : StateMachineBehaviour
{
    private const float SLOW_DOWN_TIME_VALUE = 0.3f;
    private const float DEFAULT_TIME_SCALE_TIME_VALUE = 1f;

    private readonly float[] ANIMATION_TIME_KEY_VALUES = {0.25f, 0.35f, 0.5f };

    public Action DamageEnemies { get; set; }
    public Action OnExit { get; set; }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= ANIMATION_TIME_KEY_VALUES[0])
        {
            //Time.timeScale = SLOW_DOWN_TIME_VALUE;
        }

        if(stateInfo.normalizedTime >= ANIMATION_TIME_KEY_VALUES[1])
        {
            DamageEnemies?.Invoke();
        }

        if(stateInfo.normalizedTime >= ANIMATION_TIME_KEY_VALUES[2])
        {
           // Time.timeScale = DEFAULT_TIME_SCALE_TIME_VALUE;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnExit?.Invoke();
    }
}
