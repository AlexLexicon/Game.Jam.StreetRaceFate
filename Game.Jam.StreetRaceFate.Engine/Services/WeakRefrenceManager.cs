using System;
using System.Collections.Generic;

namespace Game.Jam.StreetRaceFate.Engine.Services;
public interface IWeakRefrenceManager
{
    void Add<TEntityInterface>(TEntityInterface refrence);
    IEnumerable<TEntityInterface> Get<TEntityInterface>();
}
public class WeakRefrenceManager : IWeakRefrenceManager
{
    private readonly Dictionary<Type, List<WeakReference>> _interfaceToWeakReferences;

    public WeakRefrenceManager()
    {
        _interfaceToWeakReferences = new Dictionary<Type, List<WeakReference>>();
    }

    public void Add<TEntityInterface>(TEntityInterface refrence)
    {
        var weakRefrence = new WeakReference(refrence);

        Type key = typeof(TEntityInterface);
        if (_interfaceToWeakReferences.TryGetValue(key, out List<WeakReference>? weakReferences))
        {
            weakReferences.Add(weakRefrence);
        }
        else
        {
            _interfaceToWeakReferences.Add(key, new List<WeakReference>
            {
                weakRefrence,
            });
        }
    }

    public IEnumerable<TEntityInterface> Get<TEntityInterface>()
    {
        if (_interfaceToWeakReferences.TryGetValue(typeof(TEntityInterface), out List<WeakReference>? weakReferences))
        {
            foreach (WeakReference weakReference in weakReferences)
            {
                if (weakReference.IsAlive && weakReference.Target is not null)
                {
                    yield return (TEntityInterface)weakReference.Target;
                }
            }
        }
    }
}
