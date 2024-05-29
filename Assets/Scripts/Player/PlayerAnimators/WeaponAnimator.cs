using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    private const string IS_ATTACKING = "IsAttacking";

    [SerializeField] PlayerAttack player;

    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        player.OnAttack += PlayerBehavior_OnAttack;
    }

    private void PlayerBehavior_OnAttack(object sender, EventArgs e)
    {
        animator.SetTrigger(IS_ATTACKING);
    }
}
