using UnityEngine;
using InControl;

public partial class Player
{
    // input
    private Vector2 MovementAxis
    {
        get
        {
            return new Vector2(_controller.LeftStickX,
                _controller.LeftStickY);
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
            return new Vector2(_controller.RightStickY,
                _controller.RightStickY);
        }
    }

    private bool MeleeTrigger
    {
        get { return _controller.LeftTrigger; }
    }

    private bool InteractionTrigger
    {
        get { return _controller.RightTrigger; }
    }

    private AbilitySlot AbilityButton
    {
        get
        {
            // TODO: Additional priority handling for held button attacks?
            if (_controller.Action1) return AbilitySlot.Slot1;
            if (_controller.Action2) return AbilitySlot.Slot2;
            if (_controller.Action3) return AbilitySlot.Slot3;
            if (_controller.Action4) return AbilitySlot.Slot4;

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