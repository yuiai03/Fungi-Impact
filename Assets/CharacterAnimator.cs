using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void SetMoveDirection(Vector2 direction)
    {
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
    }

    public void SetLastMoveDirection(Vector2 direction)
    {
        animator.SetFloat("LastMoveX", direction.x);
        animator.SetFloat("LastMoveY", direction.y);
    }

    public void SetAttackDirection(Vector2 direction)
    {
        animator.SetFloat("AttackX", direction.x);
        animator.SetFloat("AttackY", direction.y);
    }

    public void SetMoveState(bool state)
    {
        animator.SetBool("Move", state);
    }
    public void SetTriggerDie()
    {
        animator.SetTrigger("Die");
    }

    public void SetTriggerAttack()
    {
        animator.SetTrigger("Attack");
    }

    //comming soon
    public void SetTriggerHurt()
    {
        animator.SetTrigger("Hurt");

    }
}
