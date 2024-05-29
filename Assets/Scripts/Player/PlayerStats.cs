using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public event EventHandler OnHurt;
    public event EventHandler OnHeal;
    public event EventHandler OnDie;
    private float invincibiltyTimer = 2.5f;
    private bool isInvincible = false;
    public int PlayerMaxHealth {get; private set;} = 6;
    public int PlayerHealth {get; private set;} = 6;
    public int PlayerMaxMagic {get; private set;} = 60;
    public int PlayerMagic {get; private set;} = 60;
    public float PlayerSpeed {get; private set;} = 10;
    public float PlayerDashSpeed {get; private set;} = 10;
    [SerializeField] Collider2D hitbox;

    void Start()
    {
        PlayerHealth = PlayerMaxHealth;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.gameObject.TryGetComponent<IDamageable>(out IDamageable entity) && !isInvincible)
        {
            PlayerTakeDamage();
        }
    }

    public void PlayerUseMagic()
    {
        PlayerMagic -= 20;
    }

    //in a rougelike setting, we're gonna wanna have a function that can take in like a value that we want to change a stat by and some sort of tell for what that stat is
    //could maybe use a string but that seems messy. Enum with a state machine perhaps? IDK
    //but we'd want to call it everytime we get a new item so it can appropriately take in all the stats
    //maybe the items can be scriptable objects we feed into the function
    //
    //that or we're gonna want to have a function that observes all the items and stat changing things obtaiend then calculate the stats from there
    //this function would be triggered if an event is fired like "OnStatsChanged"

    //for this setting tho, since we'll likely just be working with magic and health, it'd make sense to just have these quick and dirty functions
    private void PlayerTakeDamage()
    {
        PlayerHealth -= 1;
        OnHurt?.Invoke(this, EventArgs.Empty);
        Debug.Log(PlayerHealth);
        StartCoroutine(PlayerInvincibiltyTimer());
    }

    public void PlayerMagicRegen(int regenAmount)
    {
        PlayerMagic += regenAmount;
    }

    public void PlayerHeal(int healAmount)
    {
        if (PlayerHealth < PlayerMaxHealth)
        {
            PlayerHealth += healAmount;
        }
        OnHeal?.Invoke(this, EventArgs.Empty);
    }

    IEnumerator PlayerInvincibiltyTimer()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibiltyTimer);
        isInvincible = false;
    }

    /*
    The issue with implementing this as an interface is that enemies would be able to hurt eachother unless they also had a check to make sure it was only the player they were hurting
    that might make sense if enemies could be charmed to hurt eachother, but on its own it just serves to add an extra check enemies would need to do
    for now we'll keep this commented but it could be revisited

    public void TakeDamage(float damage)
    {
        PlayerHealth -= (int)damage;
        if (PlayerHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnDie?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }

    public void Di

    */
}
