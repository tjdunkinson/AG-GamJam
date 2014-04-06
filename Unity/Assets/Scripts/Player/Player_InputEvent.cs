using UnityEngine;

public partial class Player
{
    private void AimingInputEvent(Vector2 input)
    {
    }

    private void AbilityInputEvent(AbilitySlot input)
    {
        if (input == AbilitySlot.None) return;
		HandleAbilityInput(input);
    }

    private void MeleeInputEvent()
    {
    }

    private void InteractionInputEvent()
    {
    }
}