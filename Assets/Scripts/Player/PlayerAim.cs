using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] GameInput gameInput;
    [SerializeField] Transform aimTransform;
    Vector3 lookDirection;
    void Update()
    {
        //gets direction the player is aiming based on the difference between the mouse and player poistions
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        lookDirection = (mousePosition - transform.position).normalized;

        //uses the direction to rotate the Aim object attached to the player in the direction of the mouse.
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        aimTransform.eulerAngles = new Vector3(0,0,angle);
    }
}
