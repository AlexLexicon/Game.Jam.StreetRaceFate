namespace Game.Jam.StreetRaceFate.Engine.Services;
public interface ISpriteBatchDrawablePriorityProvider
{
    int Priority { get; set; }
}
public interface ISpriteBatchDrawablePriorityProvider<TGameObject> : ISpriteBatchDrawablePriorityProvider where TGameObject : ISpriteBatchDrawable
{
}
public class SpriteBatchDrawablePriorityProvider<TGameObject> : ISpriteBatchDrawablePriorityProvider<TGameObject> where TGameObject : ISpriteBatchDrawable
{
    public int Priority { get; set; }
}
