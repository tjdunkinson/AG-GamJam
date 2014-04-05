using UnityEngine;
using InControl;

public class InControlManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        InputManager.Setup();
	}
	
	// Update is called once per frame
	void Update () {
        InputManager.Update();
        var InputDevice = InputManager.ActiveDevice;

        if (InputDevice.Action1.WasPressed)
            Debug.Log("hello");
	}
}
