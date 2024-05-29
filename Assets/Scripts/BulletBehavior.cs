using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D shooterCollider;
    private float bulletSpeed;
    private Sprite bulletSprite;


    public void Setup(Collider2D shooterCollider, float bulletSpeed/*, Sprite bulletSprite*/)
    {
        this.shooterCollider = shooterCollider;
        this.bulletSpeed = bulletSpeed;
        //this.bulletSprite = bulletSprite;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        GameObject spawner = GameObject.Find("Distracting Clown Boss");
        shooterCollider = spawner.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), shooterCollider);
    }

    void Update()
    {
        //Physics2D.IgnoreCollision(GetComponent<Collider2D>(), shooterCollider);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
