using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

delegate void RequestDelete(_3DGameProject.MyBaseObject obj);

namespace _3DGameProject
{
    interface IDeletable
    {
        void setDeleteFunction(RequestDelete rd);
        void onDelete();
    }
}
