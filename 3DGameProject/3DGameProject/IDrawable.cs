using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _3DGameProject
{
    interface IDrawable
    {
        void onDraw(Matrix view);
    }
}
