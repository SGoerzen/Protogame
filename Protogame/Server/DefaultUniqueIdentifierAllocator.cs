namespace Protogame
{
    public class DefaultUniqueIdentifierAllocator : IUniqueIdentifierAllocator
    {
        private int m_NextUniqueIdentifier;

        public DefaultUniqueIdentifierAllocator()
        {
            this.m_NextUniqueIdentifier = 1;
        }

        public int Current
        {
            get
            {
                return this.m_NextUniqueIdentifier;
            }
            set
            {
                this.m_NextUniqueIdentifier = value;
            }
        }

        public int Allocate()
        {
            return this.m_NextUniqueIdentifier++;
        }
    }
}

