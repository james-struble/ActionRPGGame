using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] GameObject heartPrefab;
    [SerializeField] private PlayerStats playerStats;
    List<HealthSingleUI> hearts = new List<HealthSingleUI>();
    [SerializeField] private Transform container;
    //[SerializeField] private Transform heartTemplate;
    // Start is called before the first frame update
    // void Awake()
    // {
    //     heartTemplate.gameObject.SetActive(false);
    // }

    void Start()
    {
        playerStats.OnHurt += PlayerStats_OnHurt;
        playerStats.OnHeal += PlayerStats_OnHeal;
        DrawHearts();
    }

    public void DrawHearts()
    {
        ClearHearts();

        //determine how many hearts to make total

        float maxHealthRemainder = playerStats.PlayerMaxHealth % 2;
        int heartsToMake = (int)((playerStats.PlayerMaxHealth / 2) + maxHealthRemainder);
        for(int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }

        for(int i = 0; i < hearts.Count; i++)
        {
            int heartStatusRemainder = (int)Mathf.Clamp(playerStats.PlayerHealth - (i*2), 0, 2);
            hearts[i].SetHeartImage((HealthSingleUI.HeartStatus)heartStatusRemainder);
        }
    }

    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(container.transform);

        HealthSingleUI heartComponent = newHeart.GetComponent<HealthSingleUI>();
        heartComponent.SetHeartImage(HealthSingleUI.HeartStatus.Empty);
        hearts.Add(heartComponent);
    }

    private void ClearHearts()
    {
        foreach(Transform t in container.transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthSingleUI>();
    }

    void PlayerStats_OnHurt(object sender, EventArgs e)
    {
        DrawHearts();
        //UpdateVisual();
    }

    void PlayerStats_OnHeal(object sender, EventArgs e)
    {
        DrawHearts();
        //UpdateVisual();
    }

    // private void UpdateVisual()
    // {
    //     foreach (Transform child in container)
    //     {
    //         Destroy(child.gameObject);
    //     }

    //     for(int i = 0; i <= playerStats.playerHealth; i++)
    //     {
    //         Transform heartTransform = Instantiate(heartTemplate, container);
    //         heartTransform.gameObject.SetActive(true);
    //     }
    // }
}
