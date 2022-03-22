using System;
using System.Collections.Generic;

using System.Text;

namespace UnityFramework.Utils
{
    public interface IPoolable// : IDisposable
    {
        void ResetAndClear();
    }
}
