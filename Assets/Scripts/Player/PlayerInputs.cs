using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class PlayerInputs : MonoBehaviour
{
    public static PlayerInputs Instance;

    private PlayerController playerController;
    public bool pressingDashButton;
    public bool pressingJumpButton;
    public bool pressingHookButton;
    public bool pressingLeftButton;
    public bool pressingRightButton;
    public bool pressingUpButton;
    public bool pressingDownButton;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Awake()
    {
        Instance = this;

        playerController = new PlayerController();

        playerController.Player.Jump.performed += ctx => isJumping(ctx);
        playerController.Player.Hook.performed += ctx => isHooking(ctx);
        playerController.Player.Dash.performed += ctx => isDashing(ctx);

        playerController.Player.MoveLeft.performed += ctx => isMovingLeft(ctx);
        playerController.Player.MoveRight.performed += ctx => isMovingRight(ctx);
        playerController.Player.Up1.performed += ctx => isLookingUp(ctx);
        playerController.Player.Down.performed += ctx => isLookingDown(ctx);
    }

    private void OnEnable()
    {
        playerController.Player.Enable();
    }

    private void isJumping(InputAction.CallbackContext context)
    {
        if (Input.GetKeyDown(KeyCode.Space) || context.performed)
        {
            pressingJumpButton = true;
        }
        if (context.canceled)
        {
            pressingJumpButton = false;
        }

    }

    private void isHooking(InputAction.CallbackContext context)
    {
        pressingHookButton = Input.GetKeyDown(KeyCode.F) || context.performed;

    }

    private void isDashing(InputAction.CallbackContext context)
    {
        pressingDashButton = Input.GetKeyDown(KeyCode.E) || context.performed;

    }
    private void isMovingLeft(InputAction.CallbackContext context)
    {
        pressingLeftButton = Input.GetKeyDown(KeyCode.A) || context.performed;
    }

    private void isMovingRight(InputAction.CallbackContext context)
    {
        pressingRightButton = Input.GetKeyDown(KeyCode.D) || context.performed;

    }

    private void isLookingUp(InputAction.CallbackContext context)
    {
        pressingUpButton = Input.GetKeyDown(KeyCode.W) || context.performed;

    }

    private void isLookingDown(InputAction.CallbackContext context)
    {
        pressingDownButton = Input.GetKeyDown(KeyCode.S) || context.performed;

    }

}
