using Game.Jam.StreetRaceFate.Application.Scenes;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;

namespace Game.Jam.StreetRaceFate.Application.Service;
public interface IViewService
{
    Matrix GetViewMatrix();
}
public class ViewService : IViewService
{
    public const int CAR_TEXTURE_HEIGHT = 42;
    public const int MAX_NUM_OF_PLAYERS = 17;
    public const int CAR_PADDING = 8;

    private readonly IViewportService _viewportService;

    public ViewService(IViewportService viewportService)
    {
        _viewportService = viewportService;
    }

    public Matrix GetViewMatrix()
    {
        int viewportWidth = _viewportService.GetViewportWidth();
        int viewportHeight = _viewportService.GetViewportHeight();

        int viewHeight = MAX_NUM_OF_PLAYERS * CAR_TEXTURE_HEIGHT;
        viewHeight += CAR_TEXTURE_HEIGHT * 2;//add 1 extra padding of cars to top and bottom
        viewHeight += (MAX_NUM_OF_PLAYERS - 1) * CAR_PADDING; //add CAR_PADDING between all cars

        float scaleX = (float)viewportWidth / (viewHeight * 3);
        float scaleY = (float)viewportHeight / viewHeight;
        Matrix matrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);

        return matrix;
    }
}
