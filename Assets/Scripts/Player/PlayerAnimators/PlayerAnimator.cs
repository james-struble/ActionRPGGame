using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_DASHING = "IsDashing";
    private const string IS_WALKING = "IsWalking";
    private const string HURT_EVENT = "HurtEvent";
    [SerializeField] PlayerBehavior player;
    [SerializeField] PlayerStats playerStats;
    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        player.OnDash += PlayerBehavior_OnDash;
        playerStats.OnHurt += PlayerStats_OnHurt;
    }

    private void PlayerBehavior_OnDash(object sender, EventArgs e)
    {
        animator.SetTrigger(IS_DASHING);
    }

    private void PlayerStats_OnHurt(object sender, EventArgs e)
    {
        animator.SetTrigger(HURT_EVENT);
    }

    void Update()
    {
        animator.SetBool(IS_WALKING, player.IsWalking());
        
        //animator.SetBool(IS_DASHING, player.IsDashing());
    }
}
