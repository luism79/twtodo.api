namespace backend.services.exceptions
{
    public class ServicesExceptions : Exception
    {
        public ServicesExceptions(string message) : base(message) { }        
    }

    public class ServicesPostExceptions : ServicesExceptions
    {
        public ServicesPostExceptions(string message) : base(message)
        {
        }
    }

    public class ServicesPutExceptions : ServicesExceptions
    {
        public ServicesPutExceptions(string message) : base(message)
        {
        }
    }

    public class ServicesDeleteExceptions : ServicesExceptions
    {
        public ServicesDeleteExceptions(string message) : base(message)
        {
        }
    }

    public class ServicesAuthenticatedExceptions : ServicesExceptions
    {
        public ServicesAuthenticatedExceptions(string message) : base(message)
        {
        }
    }
}
