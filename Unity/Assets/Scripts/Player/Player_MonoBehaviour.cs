using InControl;
public partial class Player
{
    InputDevice _controller;
    private void Awake()
    {
        Controller.detectCollisions = true;
        InputManager.Setup();
        _controller = InputManager.ActiveDevice;
    }

    private void Update()
    {
        InputManager.Update();
        InputUpdate();
    }
}