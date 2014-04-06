using InControl;
public partial class Player
{
    private void Awake()
    {
        Controller.detectCollisions = true;
        InputManager.Setup();
    }

    private void Update()
    {
        InputManager.Update();
        InputUpdate();
    }
}