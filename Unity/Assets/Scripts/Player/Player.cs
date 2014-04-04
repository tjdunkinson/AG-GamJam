using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public partial class Player : MonoBehaviour
{
    private CharacterController Controller
    {
        get { return GetComponent<CharacterController>();  }
    }


    public enum PlayerSlot
    {
        None = 0,
        P1,
        P2,
    }

    public enum AbilitySlot
    {
        None = 0,
        Slot1,
        Slot2,
        Slot3,
        Slot4,
    }

    [SerializeField]
    private PlayerSlot _slot = PlayerSlot.P1;

}