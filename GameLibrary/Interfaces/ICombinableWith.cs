using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibrary.Interfaces
{
    interface ICombinableWith<T> where T : IUsable        
    {
        string UsedWith(T item);
    }
}
