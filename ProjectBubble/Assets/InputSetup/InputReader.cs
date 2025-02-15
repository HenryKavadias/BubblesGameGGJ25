using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInputs;

// Reads the players inputs
[CreateAssetMenu(fileName = "New Input Reader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, IFPControlsActions
{
    public event Action<Vector2> MoveEvent;
    public event Action<Vector2> LookEvent;
    public event Action<bool> JumpEvent;
    public event Action<bool> CrouchEvent;
    public event Action<bool> SprintEvent;
    public event Action<bool> SlideEvent;
    public event Action<bool> DashEvent;
    public event Action<bool> GrappleEvent;
    public event Action<bool> SwingEvent;
    public event Action<bool> AlternateEvent;
    public event Action<bool> ShootEvent;
    public event Action<bool> ReloadEvent;
    public event Action<bool> InteractEvent;
    public event Action<bool> SwapEvent;

    [Range(1f, 500f)] public float mouseSensitivityX = 100f;
    [Range(1f, 500f)] public float mouseSensitivityY = 100f;

    private PlayerInputs controls;

    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new PlayerInputs();
            controls.FPControls.SetCallbacks(this);
        }
        controls.FPControls.Enable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    { MoveEvent?.Invoke(context.ReadValue<Vector2>()); }
    public void OnLook(InputAction.CallbackContext context)
    { LookEvent?.Invoke(context.ReadValue<Vector2>()); }
    public void OnJump(InputAction.CallbackContext context)
    { JumpEvent?.Invoke(context.action.IsPressed()); }
    public void OnCrouch(InputAction.CallbackContext context)
    { CrouchEvent?.Invoke(context.action.IsPressed()); }
    public void OnSprint(InputAction.CallbackContext context)
    { SprintEvent?.Invoke(context.action.IsPressed()); }
    public void OnSlide(InputAction.CallbackContext context)
    { SlideEvent?.Invoke(context.action.IsPressed()); }
    public void OnDash(InputAction.CallbackContext context)
    { DashEvent?.Invoke(context.action.IsPressed()); }
    public void OnGrapple(InputAction.CallbackContext context)
    { GrappleEvent?.Invoke(context.action.IsPressed()); }
    public void OnSwing(InputAction.CallbackContext context)
    { SwingEvent?.Invoke(context.action.IsPressed()); }
    public void OnAlternate(InputAction.CallbackContext context)
    { AlternateEvent?.Invoke(context.action.IsPressed()); }
    public void OnShoot(InputAction.CallbackContext context)
    { ShootEvent?.Invoke(context.action.IsPressed()); ; }
    public void OnReload(InputAction.CallbackContext context)
    { ReloadEvent?.Invoke(context.action.IsPressed()); }
    public void OnInteract(InputAction.CallbackContext context)
    { InteractEvent?.Invoke(context.action.IsPressed()); }
    public void OnSwap(InputAction.CallbackContext context) 
    { SwapEvent?.Invoke(context.action.IsPressed()); }
}
