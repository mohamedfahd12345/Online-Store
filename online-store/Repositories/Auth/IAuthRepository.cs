using online_store.Helper;

namespace online_store.Repositories.Auth
{
    public interface IAuthRepository
    {
       public  Task<VerifyOfRequest> Register(CustomerDTO customerDTO, string Role = "user");

        public  Task<User> GetUser(string email);
       
    }
}
