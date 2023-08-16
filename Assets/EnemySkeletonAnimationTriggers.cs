using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonAnimationTriggers : MonoBehaviour
{
    private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

    // will be called upon exiting animation, setting TriggerCalled boolean to true
    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }
}
