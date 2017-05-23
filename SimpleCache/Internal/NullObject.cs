namespace XperiCode.SimpleCache.Internal
{
    internal class NullObject
    {
        private static NullObject _instance;

        private NullObject()
        {
        }

        public static NullObject Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NullObject();
                }
                return _instance;
            }
        }
    }
}
