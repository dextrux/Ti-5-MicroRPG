using System.Collections.Generic;

namespace Logic.Scripts.GameDomain.MVC.Environment.Orb
{
    public class OrbRegistry
    {
        private readonly List<OrbController> _list = new List<OrbController>();
        public void Register(OrbController c)
        {
            if (c != null && !_list.Contains(c)) _list.Add(c);
        }
        public void Unregister(OrbController c)
        {
            if (c != null) _list.Remove(c);
        }
        public List<OrbController> GetAll() { return _list; }
    }
}


