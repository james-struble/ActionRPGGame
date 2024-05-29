using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBarUI : MonoBehaviour
{
    [SerializeField] Image bossBarImage;
    private GameObject boss;
    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.Find("Distracting Clown Boss");
    }

    // Update is called once per frame
    void Update()
    {
        bossBarImage.fillAmount = boss.GetComponent<DistractingClownBossStats>().GetBossHealthNormalized();
    }
}
