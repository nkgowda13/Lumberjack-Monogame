using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumberjack
{
    public class Event
    {
        private event Action _action = delegate { };
        public void AddListener(Action listener)
        {
            _action += listener;
        }
        public void RemoveListener(Action listener)
        {
            _action -= listener;
        }
        public virtual void Execute()
        {
            _action.Invoke();
        }
        public virtual void Execute(int val)
        {

        }
    }
}
