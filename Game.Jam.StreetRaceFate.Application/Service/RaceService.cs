namespace Game.Jam.StreetRaceFate.Application.Service;
public interface IRaceService
{
    float GetRailSpeed();
    float GetCloseCitySpeed();
    float GetFarCitySpeed();
    bool IsRaceStarted();
    void StartRace();
}
public class RaceService : IRaceService
{
    public RaceService()
    {
        Speed = 10f;
    }

    private bool RaceStarted { get; set; }
    private float Speed { get; set; }

    public bool IsRaceStarted()
    {
        return RaceStarted;
    }

    public void StartRace()
    {
        RaceStarted = true;
    }

    public float GetRailSpeed()
    {
        return Speed;
    }

    public float GetCloseCitySpeed()
    {
        return GetRailSpeed() / 2;
    }

    public float GetFarCitySpeed()
    {
        return GetCloseCitySpeed() / 2;
    }
}
