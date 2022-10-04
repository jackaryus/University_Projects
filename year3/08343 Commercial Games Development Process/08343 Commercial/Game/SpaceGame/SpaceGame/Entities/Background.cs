// File Author: Daniel Masterson
using SpaceGame.Components;
using SpaceGame.GameManagers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.Entities
{
    /// <summary>
    /// Handles rendering scene backgrounds
    /// </summary>
    public class Background : Entity
    {
        const float starSize = 512.0f;

        protected override void OnSpawn()
        {
            //Just make a lot of stars
            for (int y = -5; y <= 15; ++y)
            {
                for (int x = -5; x <= 15; ++x)
                {
                    RegisterComponent(new ImageComponent("stars"))
                        .SetScale(new Vector2(starSize, starSize))
                        .SetPosition(new Vector2(x * starSize, y * starSize))
                        .SetRenderSpace(ERenderSpace.WorldSpace)
                        .SetParallax(new Vector2(0.25f, 0.25f))
                        .SetZIndex(-100);
                }
            }
        }
    }
}
