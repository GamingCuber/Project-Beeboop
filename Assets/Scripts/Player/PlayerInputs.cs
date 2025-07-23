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

        playerController.Player.Jump.performed += ctx => setJump(ctx);
        playerController.Player.Jump.canceled += ctx => cancelJump(ctx);

        playerController.Player.Hook.performed += ctx => setHook(ctx);
        playerController.Player.Hook.canceled += ctx => cancelHook(ctx);

        playerController.Player.Dash.performed += ctx => setDash(ctx);
        playerController.Player.Dash.canceled += ctx => cancelDash(ctx);

        playerController.Player.MoveLeft.performed += ctx => isMovingLeft(ctx);
        playerController.Player.MoveLeft.canceled += ctx => isMovingLeft(ctx);

        playerController.Player.MoveRight.performed += ctx => isMovingRight(ctx);
        playerController.Player.MoveRight.canceled += ctx => isMovingRight(ctx);

        playerController.Player.Up1.performed += ctx => isLookingUp(ctx);
        playerController.Player.Up1.canceled += ctx => isLookingUp(ctx);

        playerController.Player.Down.performed += ctx => isLookingDown(ctx);
        playerController.Player.Down.canceled += ctx => isLookingDown(ctx);

    }

    private void OnEnable()
    {
        playerController.Player.Enable();
    }

    private void setJump(InputAction.CallbackContext context)
    {
        pressingJumpButton = true;
    }

    private void cancelJump(InputAction.CallbackContext context)
    {
        pressingJumpButton = false;
    }

    private void setHook(InputAction.CallbackContext context)
    {
        pressingHookButton = true;

    }
    private void cancelHook(InputAction.CallbackContext context)
    {
        pressingHookButton = false;

    }

    private void setDash(InputAction.CallbackContext context)
    {
        pressingDashButton = true;

    }
    private void cancelDash(InputAction.CallbackContext context)
    {
        pressingDashButton = false;

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
