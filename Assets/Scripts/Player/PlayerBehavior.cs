using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private enum State
    {
        Normal,
        Dashing
    }
    private State state;

    public event EventHandler OnDash;

    [SerializeField] PlayerStats playerStats;
    [SerializeField] GameInput gameInput;
    
    float dashSpeed;
    private bool isWalking;

    Rigidbody2D rb;

    Vector3 moveDirection;
    Vector3 dashDirection;

    private bool dashReady = true;
    private float dashCooldownTimer = 0.25f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        state = State.Normal;
    }
    void Start()
    {
        gameInput.OnDashAction += GameInput_OnDashAction;
    }

    private void GameInput_OnDashAction(object sender, EventArgs e)
    {
        if (dashReady)
        {
            StartCoroutine(DashCooldown());
            StartCoroutine(DashPadding());
        }
    }

    void Update()
    {
        switch (state)
        {
        case State.Normal:
            //Movement Input
            moveDirection = gameInput.GetMovementVector();
            isWalking = moveDirection != Vector3.zero;
            break;
        case State.Dashing:
            DecelerateDash();
            break;
        }
    }

    void FixedUpdate()
    {
        switch (state)
        {
        case State.Normal:
            HandleMovmement();
            break;
        case State.Dashing:
            Dash();
            break;
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    void HandleMovmement()
    {
        rb.velocity = moveDirection * playerStats.PlayerSpeed;
    }

    void DecelerateDash()
    {
        float dashSpeedDropMultiplier = 2f;
        dashSpeed -= dashSpeed * dashSpeedDropMultiplier * Time.deltaTime;

        float dashSpeedMinimum = 20f;
        if (dashSpeed < dashSpeedMinimum)
        {
            state = State.Normal;
        }
    }

    void Dash()
    {
        rb.velocity = dashDirection * dashSpeed;
    }

    IEnumerator DashPadding()
    {
        dashDirection = moveDirection;
        yield return new WaitForEndOfFrame();
        if (dashDirection != new Vector3(0,0,0))
        {
            dashDirection = moveDirection;
        }
        dashSpeed = 40f;
        if (dashDirection != new Vector3(0,0,0))
        {
            OnDash?.Invoke(this, EventArgs.Empty);
            state = State.Dashing;
        }
    }

    IEnumerator DashCooldown()
    {
        dashReady = false;
        yield return new WaitForSeconds(dashCooldownTimer);
        dashReady = true;
    }
}
