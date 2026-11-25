namespace DevFlowAPI.Exceptions.Auth
{
    public class EmailAlreadyRegisteredException : Exception
    {
        public EmailAlreadyRegisteredException()
            : base("Email address is already registered.")
        {
        }
    }
}
