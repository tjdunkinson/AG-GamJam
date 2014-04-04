using UnityEngine;

public partial class Player
{
    // input

    private string MovementAxisName { get { return _slot + " Movement"; }
    }

    private Vector2 MovementAxis 
    {
        get
        {
            return new Vector2(Input.GetAxisRaw(MovementAxisName + " X"), 
                Input.GetAxisRaw(MovementAxisName + " Y"));
        }
    }

    private string AimingAxisName { get { return _slot + " Aiming"; }
    }

    private Vector2 AimingAxis
    {
        get {
            return new Vector2(Input.GetAxisRaw(AimingAxisName + " X"),
                Input.GetAxisRaw(AimingAxisName + " Y")); 
        }
    }

    private bool MeleeTrigger { get { return Input.GetButtonDown(_slot + " Melee Trigger"); }}

    private bool InteractionTrigger { get { return Input.GetButtonDown(_slot + " Interaction Trigger"); } }

    private AbilitySlot AbilityButton { get
    {
        // TODO: Additional priority handling for held button attacks?
        if (Input.GetButtonDown(_slot + " Ability 1")) return AbilitySlot.Slot1;
        if (Input.GetButtonDown(_slot + " Ability 2")) return AbilitySlot.Slot2;
        if (Input.GetButtonDown(_slot + " Ability 3")) return AbilitySlot.Slot3;
        if (Input.GetButtonDown(_slot + " Ability 4")) return AbilitySlot.Slot4;

        return AbilitySlot.None;
    }}

    private void InputUpdate()
    {
        if (_slot == 0) return;

        // TODO: Input exclusivity for specific cases
        
        MovementInputEvent(MovementAxis);
        
        AimingInputEvent(AimingAxis);

        AbilityInputEvent(AbilityButton);

        // value interaction over meleeing
        if (InteractionTrigger) InteractionInputEvent();
        else
        if (MeleeTrigger) MeleeInputEvent();

    }
}