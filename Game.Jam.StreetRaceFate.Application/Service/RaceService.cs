namespace Game.Jam.StreetRaceFate.Application.Service;
public interface IRaceService
{
    float GetRailSpeed();
    float GetCloseCitySpeed();
    float GetFarCitySpeed();
    float GetTreesSpeed();
    bool IsRaceStarted();
    bool IsRaceOver();
    bool IsRacing();
    void StartRace();
    void StopRace();
}
public class RaceService : IRaceService
{
    public RaceService()
    {
        Speed = 0f;
    }

    private bool RaceStarted { get; set; }
    private bool RaceOver { get; set; }
    private float Speed { get; set; }

    public bool IsRacing()
    {
        return RaceStarted && !RaceOver;
    }

    public bool IsRaceStarted()
    {
        return RaceStarted;
    }

    public bool IsRaceOver()
    {
        return RaceOver;
    }

    public void StartRace()
    {
        Speed = 20f;
        RaceStarted = true;
    }

    public void StopRace()
    {
        Speed = 0;
        RaceOver = true;
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

    public float GetTreesSpeed()
    {
        return (GetRailSpeed() / 4) * 3;
    }
}
