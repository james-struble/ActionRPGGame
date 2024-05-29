using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSingleUI : MonoBehaviour
{
    [SerializeField] Sprite fullHeartSprite, halfHeartSprite, emptyHeartSprite;
    Image heartImage;

    public enum HeartStatus
    {
        Empty = 0,
        Half = 1,
        Full = 2
    }
    // Start is called before the first frame update

    void Awake()
    {
        heartImage = GetComponent<Image>();
    }

    public void SetHeartImage(HeartStatus status)
    {
        switch(status)
        {
            case HeartStatus.Empty:
                heartImage.sprite = emptyHeartSprite;
                break;
            case HeartStatus.Half:
                heartImage.sprite = halfHeartSprite;
                break;
                case HeartStatus.Full:
                heartImage.sprite = fullHeartSprite;
                break;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
