using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Infrastructure.Context_Models;

namespace UserService.Infrastructure.GenericRepository
{
    public interface IUserRepository
    {
        Task<TB_User> GetUserByUsernameAsync(string username);
        Task<TB_User> GetUserByEmailAsync(string email);
    }
    public class UserRepository : IUserRepository
    {
        private readonly IGenericRepository<TB_User> _IGenericRepository;

        public UserRepository(IGenericRepository<TB_User> IGenericRepository)
        {
            this._IGenericRepository = IGenericRepository;
        }

        public async  Task<TB_User> GetUserByEmailAsync(string email)
        {
            //simple query data with generic class
            //error always without real connection
            try
            {
                return await this._IGenericRepository.Table.FirstOrDefaultAsync(s => s.Email == email);

            }
            catch (Exception ex)
            {

                return new TB_User()
                {
                    Username = "aaa",
                    Email = email,
                    First_Name = "aaa",
                    Last_Name = "bbb",
                    Tel = "00000000",
                    Type = "admin"
                };
            }
        }

        public async Task<TB_User> GetUserByUsernameAsync(string username)
        {
            //simple query data with generic class
            //error always without real connection
            try
            {
                return await this._IGenericRepository.Table.FirstOrDefaultAsync(s => s.Username == username);
            }
            catch (Exception ex)
            {

                return new TB_User()
                {
                    Username = username,
                    Email = "aaa@bbb.com",
                    First_Name = "aaa",
                    Last_Name = "bbb",
                    Tel = "00000000",
                    Type = "admin"
                };
            }
        }
    }
}
