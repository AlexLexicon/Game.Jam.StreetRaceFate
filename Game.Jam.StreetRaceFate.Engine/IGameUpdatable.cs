﻿using Microsoft.Xna.Framework;

namespace Game.Jam.StreetRaceFate.Engine;
public interface IGameUpdatable : IGameObject
{
    void Update(GameTime gameTime);
}
