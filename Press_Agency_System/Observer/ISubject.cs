using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Press_Agency_System.Observer
{
    public interface ISubject
    {
        void RegisterObserver(Observers observer);
        void RemoveObserver(Observers observer);
        void NotifyObservers();
    }
}
