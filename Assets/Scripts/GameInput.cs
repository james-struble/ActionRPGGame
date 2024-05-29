using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance {get; private set; }
    public event EventHandler OnAttackAction;
    public event EventHandler OnDashAction;
    public event EventHandler OnMagicAction;
    public event EventHandler OnHealAction;
    [SerializeField] Camera mainCamera;

    private PlayerInputActions playerInputActions;

    void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();

        playerInputActions.Player.Enable();

        playerInputActions.Player.Attack.performed += Attack_performed;
        playerInputActions.Player.Dash.performed += Dash_performed;
        //playerInputActions.Player.Magic.started += Magic_performed;
        playerInputActions.Player.Magic.performed += Magic_performed;
        //playerInputActions.Player.Magic.canceled += Magic_performed;
        
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Attack.performed -= Attack_performed;
        playerInputActions.Player.Dash.performed -= Dash_performed;

        playerInputActions.Dispose();
    }

    private void Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnAttackAction?.Invoke(this, EventArgs.Empty);
    }

    private void Dash_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnDashAction?.Invoke(this,EventArgs.Empty);
    }

    private void Magic_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (obj.interaction is HoldInteraction)
        {
            OnHealAction?.Invoke(this,EventArgs.Empty);
        }
        else
        {
            OnMagicAction?.Invoke(this,EventArgs.Empty);
        }
    }

    public Vector3 GetMovementVector()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }

    public Vector3 GetAimVector()
    {
        //this doesnt work because it is attached to the gameinput object which is stationary!
        //solution could be to attach the gameinput object to the player/have it move with the player
        //or just leave the aimvector functionality on the player script even tho it input
        //OR find the player object in the awake function, then use that in the lookdirection line rather than the gameinput's transform
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector3 lookDirection = (mousePosition - transform.position).normalized;

        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;

        return new Vector3(0,0,angle);
    }


}
