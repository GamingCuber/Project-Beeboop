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

        playerController.Player.MoveLeft.performed += ctx => setMovingLeft(ctx);
        playerController.Player.MoveLeft.canceled += ctx => cancelMovingLeft(ctx);

        playerController.Player.MoveRight.performed += ctx => setMovingRight(ctx);
        playerController.Player.MoveRight.canceled += ctx => cancelMovingRight(ctx);

        playerController.Player.Up1.performed += ctx => setLookingUp(ctx);
        playerController.Player.Up1.canceled += ctx => cancelLookingDown(ctx);

        playerController.Player.Down.performed += ctx => setLookingDown(ctx);
        playerController.Player.Down.canceled += ctx => cancelLookingDown(ctx);

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


    private void setMovingLeft(InputAction.CallbackContext context)
    {
        pressingLeftButton = true;
    }
    private void cancelMovingLeft(InputAction.CallbackContext context)
    {
        pressingLeftButton = false;
    }

    private void setMovingRight(InputAction.CallbackContext context)
    {
        pressingRightButton = true;

    }
    private void cancelMovingRight(InputAction.CallbackContext context)
    {
        pressingRightButton = false;

    }
    private void setLookingUp(InputAction.CallbackContext context)
    {
        pressingUpButton = true;

    }

    private void cancelLookingUp(InputAction.CallbackContext context)
    {
        pressingUpButton = false;

    }

    private void setLookingDown(InputAction.CallbackContext context)
    {
        pressingDownButton = true;

    }
    private void cancelLookingDown(InputAction.CallbackContext context)
    {
        pressingDownButton = false;

    }

}
