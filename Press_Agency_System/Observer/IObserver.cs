using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Press_Agency_System.Observer
{
    public interface IObserver
    {
        void update(string availability);
    }
}
