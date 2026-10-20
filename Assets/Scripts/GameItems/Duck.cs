public class Duck : Obstacle
{
    public override void Tap()
    {
        PlayShakeTween();
    }

    public override void Break()
    {
        if (FlyToTaskPanelComponent != null)
        {
            FlyToTaskPanelComponent.Invoke();
        }
    }

    public override void AttachAction(Tile tile)
    {
        if (tile.yIndex == 0)
        {
            Break();
        }
    }
}
