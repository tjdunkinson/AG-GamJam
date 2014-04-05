using UnityEngine;
using InControl;

public partial class Player
{
    // input
    private Vector2 MovementAxis
    {
        get
        {
            return new Vector2(InputManager.ActiveDevice.LeftStickX,
                InputManager.ActiveDevice.LeftStickY);
            // NOTE: We invert the Y-axis input because up should be +1, not -1
        }
    }

    private string AimingAxisName
    {
        get { return _slot + " Aiming"; }
    }

    private Vector2 AimingAxis
    {
        get
        {
            return new Vector2(InputManager.ActiveDevice.RightStickY,
                InputManager.ActiveDevice.RightStickY);
        }
    }

    private bool MeleeTrigger
    {
        get { return InputManager.ActiveDevice.LeftTrigger; }
    }

    private bool InteractionTrigger
    {
        get { return InputManager.ActiveDevice.RightTrigger; }
    }

    private AbilitySlot AbilityButton
    {
        get
        {
            // TODO: Additional priority handling for held button attacks?
            if (InputManager.ActiveDevice.Action1) return AbilitySlot.Slot1;
            if (InputManager.ActiveDevice.Action2) return AbilitySlot.Slot2;
            if (InputManager.ActiveDevice.Action3) return AbilitySlot.Slot3;
            if (InputManager.ActiveDevice.Action4) return AbilitySlot.Slot4;

            return AbilitySlot.None;
        }
    }

    private void InputUpdate()
    {
        if (_slot == 0) return;

        // TODO: Input exclusivity for specific cases

        MovementInputEvent(MovementAxis);

        AimingInputEvent(AimingAxis);

        AbilityInputEvent(AbilityButton);

        // value interaction over meleeing
        if (InteractionTrigger) InteractionInputEvent();
        else if (MeleeTrigger) MeleeInputEvent();
    }
}