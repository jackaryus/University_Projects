using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Components
{
    enum ComponentTypes
    {
        COMPONENT_NONE = 0,
        COMPONENT_POSITION = 1 << 0,
        COMPONENT_GEOMETRY = 1 << 1,
        COMPONENT_TEXTURE = 1 << 2,
        COMPONENT_VELOCITY = 1 << 3,
        COMPONENT_SHADER = 1 << 4,
        COMPONENT_AI = 1 << 5,
        COMPONENT_MODEL = 1 << 6,
        COMPONENT_PLAYER = 1 << 7,
        COMPONENT_SKYBOX = 1 << 8,
        COMPONENT_RECTANGLE = 1 << 9,
        COMPONENT_RENDERTARGET = 1 <<10,
        COMPONENT_CAMERA = 1 << 11,
        COMPONENT_UIVELOCITY = 1 << 12
    }

    interface IComponent
    {
        ComponentTypes ComponentType
        {
            get;
        }
    }
}
