﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicEditor
{
    public interface IDrawable
    {
        void Draw<T>(T obj);
    }
}
