namespace Game.Jam.StreetRaceFate.Engine.Services;
public interface IDrawablePriorityProvider
{
    int Priority { get; set; }
}
public interface IDrawablePriorityProvider<TGameObject> : IDrawablePriorityProvider where TGameObject : IGameDrawable
{
}
public class DrawablePriorityProvider<TGameObject> : IDrawablePriorityProvider<TGameObject> where TGameObject : IGameDrawable
{
    public int Priority { get; set; }
}
