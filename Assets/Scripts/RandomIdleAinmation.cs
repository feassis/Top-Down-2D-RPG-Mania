using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdleAinmation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Start()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        animator.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
    }
}
