using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class DistracingClownBossAI : MonoBehaviour
{
    private enum State
    {
        Chase = 0,
        Charge = 1,
        Shoot = 2,
        BounceWalls = 3,
        SuperJump = 4
    }

    private State behaviorState;
    private Rigidbody2D rb;

    private GameObject player;
    [SerializeField] Transform playerCompass;
    Vector3 moveDirection;
    Vector3 lastDirection;
    [SerializeField] GameObject projectile;

    private float moveSpeed = 10f;
    private float rotateSpeed = 5f;
    private float chargeSpeed = 30f;
    int shootCount = 0;

    private bool attackStarted = false;
    private bool attackReady = false;
    private bool shootReady = true;
    private bool SuperJumpFinished = false;
    private bool timerStarted = false;
    private bool SuperJumpStarted = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        behaviorState = State.Chase;
    }

    void Update()
    {
        playerCompass.up = player.transform.position - playerCompass.position;
        switch (behaviorState)
        {
            case State.Chase:
                ChaseBehavior();
                break;
            case State.Charge:
                ChargeBehavior();
                break;
            case State.Shoot:
                if (!attackStarted)
                {
                    rb.velocity = new Vector3(0, 0, 0);
                    StartCoroutine(BeginAttack(2));
                }
                if (attackReady)
                {
                    if(shootCount == 10)
                    {
                        behaviorState = State.Chase;
                    }
                    if (shootReady)
                    {
                        StartCoroutine(ShootTimer(0.5f));
                        ShootBehavior(10f);
                        shootCount++;
                    }
                }
                break;
            case State.BounceWalls:
                BounceWallsBehavior();
                break;
            case State.SuperJump:
                SuperJumpBehavior();
                break;
        }
    }

    void ChaseBehavior()
    {
        attackStarted = false;
        attackReady = false;
        SuperJumpStarted = false;
        shootCount = 0;
        if (timerStarted == false)
        {
            StartCoroutine(StateTransitionTimer());
        }
        rotateSpeed = 5f;

        lastDirection = moveDirection;
        moveDirection = Vector3.Slerp(moveDirection, playerCompass.up, Time.deltaTime * rotateSpeed);
        
        rb.velocity = moveSpeed * new Vector3(moveDirection.x, moveDirection.y,0);
    }

    void SuperJumpBehavior()
    {
        if (!attackStarted)
        {
            rb.velocity = new Vector3(0, 0, 0);
            chargeSpeed = 30f;
            StartCoroutine(BeginAttack(2));
        }
        if (attackReady)
        {
            GetComponent<Collider2D>().enabled = false;
            moveDirection = player.transform.position - transform.position;
            rb.velocity = chargeSpeed * moveDirection;
            if (!SuperJumpStarted)
            {
                StartCoroutine(SuperJumpDuration(5));
            }
            if (SuperJumpFinished)
            {
                //needs to freeze briefly before restoring its hitbox so the player has time to dodge
                //might also want to have it shoot bullets
                GetComponent<Collider2D>().enabled = true;
                behaviorState = State.Chase;
            }
        }
    }

    void ChargeBehavior()
    {
        rotateSpeed = 10f;
        float chargeSpeedDropMultiplier = 2f;
        
        if (!attackStarted)
        {
            rb.velocity = new Vector3(0, 0, 0);
            chargeSpeed = 30f;
            StartCoroutine(BeginAttack(2));
        }

        if (attackReady)
        {
            lastDirection = moveDirection;
            rb.velocity = chargeSpeed * moveDirection;
            chargeSpeed -= chargeSpeed * chargeSpeedDropMultiplier * Time.deltaTime;
        } else
        {
            moveDirection = Vector3.Slerp(moveDirection, playerCompass.up, Time.deltaTime * rotateSpeed);
        }

        if (chargeSpeed <= 5f)
        {
            behaviorState = State.Chase;
        }
    }

    void ShootBehavior(float numberOfProjectiles)
    {
        float angleStep = 360f / numberOfProjectiles;
        float angle = 0f;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float projectlileDirXPosition = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
            float projectlileDirYPosition = transform.position.y + Mathf.Cos ((angle * Mathf.PI)/ 180);

            Vector2 projectlileVector = new Vector2(projectlileDirXPosition, projectlileDirYPosition);
            Vector2 projectileMoveDirection = (projectlileVector - new Vector2(transform.position.x, transform.position.y)).normalized;

            var proj = Instantiate(projectile, transform.position, Quaternion.identity);
            proj.GetComponent<Rigidbody2D>().velocity = projectileMoveDirection * 2;
            //then we need to get a function on the bullet that tells it to move in the direction of the thing.
            //my heart really isn't in this right now and im not sure why... think I need to get better sleep
            angle += angleStep;
        }
    }

    void BounceWallsBehavior()
    {
        if (!attackStarted)
        {
            chargeSpeed = 30f;
            moveDirection = new Vector3(-1,-1,0);
            StartCoroutine(BeginAttack(2));
        }
        lastDirection = moveDirection;
        rb.velocity = chargeSpeed * moveDirection;
        if (chargeSpeed <= 0)
        {
            behaviorState = State.Chase;
        }
    }

    IEnumerator BeginAttack(int timer)
    {
        attackStarted = true;
        yield return new WaitForSeconds(timer);
        attackReady = true;
    }

    IEnumerator ShootTimer(float timer)
    {
        shootReady = false;
        yield return new WaitForSeconds(timer);
        shootReady = true;
    }

    IEnumerator StateTransitionTimer()
    {
        timerStarted = true;
        yield return new WaitForSeconds(5);
        //behaviorState = State.BounceWalls;
        //behaviorState = State.Charge;
        //behaviorState = State.Shoot;
        behaviorState = State.SuperJump;
        //behaviorState = (State)UnityEngine.Random.Range(1,4);
        timerStarted = false;
    }

    IEnumerator SuperJumpDuration(float timer)
    {
        SuperJumpStarted = true;
        SuperJumpFinished = false;
        yield return new WaitForSeconds(timer);
        SuperJumpFinished = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (behaviorState == State.BounceWalls || behaviorState == State.Charge || behaviorState == State.Chase)
        {
            moveDirection = Vector3.Reflect(lastDirection, -collision.GetContact(0).normal);
            chargeSpeed -= 5;
        }
    }
}
