using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicAttack : MonoBehaviour
{
    [SerializeField] Transform weaponTransform;
    [SerializeField] Transform aimTransform;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] GameInput gameInput;
    public event EventHandler OnMagic;
    [SerializeField] GameObject magicPrefab;

    private bool magicReady = true;
    private float magicCooldownTimer = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        gameInput.OnMagicAction += GameInput_OnMagicAction;
        gameInput.OnHealAction += GameInput_OnHealAction;
    }

    private void GameInput_OnMagicAction(object sender, EventArgs e)
    {
        if (magicReady)
        {
            StartCoroutine(MagicCooldown());
            if (playerStats.PlayerMagic >= 20)
            {
                //Instantiates a magic attack with the rotation of the Aim game object
                playerStats.PlayerUseMagic();
                GameObject magicAttack = Instantiate(magicPrefab, weaponTransform.position, aimTransform.rotation);
                OnMagic?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void GameInput_OnHealAction(object sender, EventArgs e)
    {
        if (playerStats.PlayerMagic >= 20)
        {
            playerStats.PlayerUseMagic();
            playerStats.PlayerHeal(1);
        }
    }

    IEnumerator MagicCooldown()
    {
        magicReady = false;
        yield return new WaitForSeconds(magicCooldownTimer);
        magicReady = true;
    }
}
