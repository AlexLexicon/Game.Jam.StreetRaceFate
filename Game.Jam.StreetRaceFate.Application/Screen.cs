using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;

namespace Game.Jam.StreetRaceFate.Application;
public class Screen : IGameDrawable
{
    private readonly IGraphicsService _graphicsService;

    public Screen(IGraphicsService graphicsService)
    {
        _graphicsService = graphicsService;
    }

    public void Draw(GameTime gameTime)
    {
        _graphicsService.Clear(Color.Blue);
    }
}
