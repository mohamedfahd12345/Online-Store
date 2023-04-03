using AutoMapper;
using online_store.Authentication_Services;
using online_store.AutoMapper;
using online_store.Helper;

namespace online_store.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly OnlineStoreContext _context;
        private readonly IMapper mapper;
        private readonly HashServices hashServices;
        public AuthRepository(OnlineStoreContext context , IMapper mapper
            , HashServices hashServices)
        {
            _context = context;
            this.mapper = mapper;
            this.hashServices = hashServices;
        }

        public async Task<VerifyOfRequest> Register(CustomerDTO customerDTO, string Role = "user")
        {
            var VerifyOfRequest = new VerifyOfRequest();

            if (!customerDTO.Email.Contains("@") || !customerDTO.Email.EndsWith(".com"))
            {
                VerifyOfRequest.Errorexisting = true;
                VerifyOfRequest.ErrorDetails = "Invaild Email";
                return VerifyOfRequest;
            }

            var EmailExistingBefore = await _context.Users.Where(x => x.Email == customerDTO.Email)
                .Select(x => x.UserId).FirstOrDefaultAsync();

            if (EmailExistingBefore != 0)
            {
                VerifyOfRequest.Errorexisting = true;
                VerifyOfRequest.ErrorDetails = "This Email Exist before";
                Console.WriteLine(EmailExistingBefore);
                return VerifyOfRequest;
            }

            var Customer = new User();
           
            var UserAddress = new Address();
           
            // <wonna be returned > (source )
            Customer = mapper.Map<User>(customerDTO);
            UserAddress = mapper.Map<Address>(customerDTO);
            

            Customer.Role = Role;

            byte[] passwordHash;
            byte[] passwordSalt;
           
            try
            {
                hashServices.CreatePasswordHash(customerDTO.Password, out passwordHash, out passwordSalt);

                Customer.PasswordHash = passwordHash;
                Customer.PasswordSalt = passwordSalt;

               
                await _context.Addresses.AddAsync(UserAddress);
                await _context.SaveChangesAsync();


                int AddressIdInDb = UserAddress.AddressId;


                Customer.AddressId = AddressIdInDb;
                

                await _context.Users.AddAsync(Customer);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                VerifyOfRequest.Errorexisting = true;
                VerifyOfRequest.ErrorDetails = ex.Message;
                return VerifyOfRequest;
            }
           
            return VerifyOfRequest;
        }

        

         

        public async Task<User> GetUser(string email)
        {
            var targetUser = await _context.Users
                .Where(x => x.Email == email)
                .FirstOrDefaultAsync();
            return targetUser;
        }
    }
}
