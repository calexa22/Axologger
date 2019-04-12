using System;
using System.Collections.Generic;
using System.Text;

namespace Axologger.Lib.Models
{
    public interface ICopyable<T>
    {
        T DeepCopy();
    }
}
