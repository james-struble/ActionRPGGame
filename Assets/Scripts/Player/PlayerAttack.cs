using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public event EventHandler OnAttack;
    [SerializeField] Transform weaponTransform;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] GameInput gameInput;
    [SerializeField] float attackRange;

    private float attackCooldownTimer = 0.5f;
    private bool attackReady = true;
    void Start()
    {
        gameInput.OnAttackAction += GameInput_OnAttackAction;
    }

    private void GameInput_OnAttackAction(object sender, EventArgs e)
    {
        Attack();
    }

    void Attack()
    {
        if (attackReady)
        {
            StartCoroutine(AttackCooldownCountdown());
            OnAttack?.Invoke(this, EventArgs.Empty);
            foreach(RaycastHit2D enemy in Physics2D.CircleCastAll(weaponTransform.position, attackRange, new Vector2(0,0)))
            {
                if(enemy.transform.gameObject.TryGetComponent<IDamageable>(out IDamageable entity))
                {
                    Debug.Log("Hit");
                    playerStats.PlayerMagicRegen(5);
                    entity.TakeDamage(10);
                }
            }
        }
    }

    IEnumerator AttackCooldownCountdown()
    {
        attackReady = false;
        yield return new WaitForSeconds(attackCooldownTimer);
        attackReady = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(weaponTransform.position, attackRange);
    }

}
