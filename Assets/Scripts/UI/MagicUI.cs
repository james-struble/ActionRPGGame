using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicUI : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats;
    [SerializeField] TextMeshProUGUI magicText;
    void Update()
    {
        magicText.text = playerStats.PlayerMagic.ToString();
    }
}
