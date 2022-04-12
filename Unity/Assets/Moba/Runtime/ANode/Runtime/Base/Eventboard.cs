using System.Collections.Generic;

namespace Scarf.ANode.Flow.Runtime
{
    public class Eventboard
    {
        private HashSet<string> m_lstEventNames = new HashSet<string>();

        public void SetEvent(string key)
        {
            m_lstEventNames.Add(key);
        }
        
        public bool ContainsEvent(string key)
        {
            return m_lstEventNames.Contains(key);
        }

        public void Clear()
        {
            m_lstEventNames.Clear();
        }
    }
}