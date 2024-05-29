using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttackBehavior : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        float moveSpeed = 100f;
        transform.position +=  moveSpeed * Time.deltaTime * transform.up;
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if(collider.transform.TryGetComponent<IDamageable>(out IDamageable entity))
        {
            entity.TakeDamage(20);
            Destroy(gameObject);
        }
    }
}
