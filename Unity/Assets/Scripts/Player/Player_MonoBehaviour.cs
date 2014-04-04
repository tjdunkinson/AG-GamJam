public partial class Player
{
    private void Awake()
    {
        Controller.detectCollisions = true;
    }

    private void Update()
    {
        InputUpdate();
    }
}