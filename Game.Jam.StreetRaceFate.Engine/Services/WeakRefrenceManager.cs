using System;
using System.Collections.Generic;

namespace Game.Jam.StreetRaceFate.Engine.Services;
public interface IWeakRefrenceManager
{
    void Add<TInterface>(TInterface refrence) where TInterface : notnull;
    void Add(Type interfaceType, object refrence);
    IEnumerable<TInterface> Get<TInterface>() where TInterface : notnull;
}
public class WeakRefrenceManager : IWeakRefrenceManager
{
    private readonly Dictionary<Type, List<WeakReference>> _interfaceToWeakReferences;

    public WeakRefrenceManager()
    {
        _interfaceToWeakReferences = new Dictionary<Type, List<WeakReference>>();
    }

    public void Add<TInterface>(TInterface refrence) where TInterface : notnull
    {
        Add(typeof(TInterface), refrence);
    }

    public void Add(Type interfaceType, object refrence)
    {
        var weakRefrence = new WeakReference(refrence);

        if (_interfaceToWeakReferences.TryGetValue(interfaceType, out List<WeakReference>? weakReferences))
        {
            weakReferences.Add(weakRefrence);
        }
        else
        {
            _interfaceToWeakReferences.Add(interfaceType, new List<WeakReference>
            {
                weakRefrence,
            });
        }
    }

    public IEnumerable<TInterface> Get<TInterface>() where TInterface : notnull
    {
        if (_interfaceToWeakReferences.TryGetValue(typeof(TInterface), out List<WeakReference>? weakReferences))
        {
            foreach (WeakReference weakReference in weakReferences)
            {
                if (weakReference.IsAlive && weakReference.Target is not null)
                {
                    yield return (TInterface)weakReference.Target;
                }
            }
        }
    }
}
