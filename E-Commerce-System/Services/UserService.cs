using E_Commerce_System.Common;
using E_Commerce_System.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_System.Services
{
    public interface IUserService
    {
        UserInfo Authenticate(string email, string password);
        JwtSecurityToken GenerateJSONWebToken(UserInfo userInfo);
        IEnumerable<UserInfo> GetAll();
        UserInfo GetById(int id);
        UserInfo Create(UserInfo user);
        bool CheckUser(UserInfo user);
        //void Update(UserInfo user, string password = null);
        //void Delete(int id);
    }
    public class UserService : IUserService
    {
        public ADBMSContext _context;
        public UserService(ADBMSContext context)
        {
            _context = context;
        }

        public UserInfo Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.UserInfo.FirstOrDefault(x => x.Email == email);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        public JwtSecurityToken GenerateJSONWebToken(UserInfo userInfo)
        {
            if (userInfo != null)
            {
                //create claims details based on the user information
                var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, JWTSetting.Subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", userInfo.UserId.ToString()),
                    new Claim("FirstName", userInfo.FirstName),
                    new Claim("LastName", userInfo.LastName),
                    new Claim("UserName", userInfo.UserName),
                    new Claim("Email", userInfo.Email)
                   };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSetting.Key));

                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(JWTSetting.Issuer, JWTSetting.Audience, claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                return token;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<UserInfo> GetAll()
        {
            return _context.UserInfo;
        }

        public UserInfo GetById(int id)
        {
            return _context.UserInfo.Find(id);
        }

        public UserInfo Create(UserInfo user)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.UserInfo.Add(user);
            _context.SaveChanges();

            return user;
        }
        public bool CheckUser(UserInfo user)
        {
            if(_context.UserInfo.Any(x=>x.UserName==user.UserName || x.Email == user.Email))
            {
                return false;
            }
            return true;
        }

        //public void Update(UserInfo userParam, string password = null)
        //{
        //    var user = _context.UserInfo.Find(userParam.UserId);

        //    if (user == null)
        //        throw new AppException("User not found");

        //    // update username if it has changed
        //    if (!string.IsNullOrWhiteSpace(userParam.UserName) && userParam.UserName != user.UserName)
        //    {
        //        // throw error if the new username is already taken
        //        if (_context.UserInfo.Any(x => x.Username == userParam.Username))
        //            throw new AppException("Username " + userParam.Username + " is already taken");

        //        user.UserName = userParam.UserName;
        //    }

        //    // update user properties if provided
        //    if (!string.IsNullOrWhiteSpace(userParam.FirstName))
        //        user.FirstName = userParam.FirstName;

        //    if (!string.IsNullOrWhiteSpace(userParam.LastName))
        //        user.LastName = userParam.LastName;

        //    // update password if provided
        //    if (!string.IsNullOrWhiteSpace(password))
        //    {
        //        byte[] passwordHash, passwordSalt;
        //        CreatePasswordHash(password, out passwordHash, out passwordSalt);

        //        user.PasswordHash = passwordHash;
        //        user.PasswordSalt = passwordSalt;
        //    }

        //    _context.UserInfo.Update(user);
        //    _context.SaveChanges();
        //}

        //public void Delete(int id)
        //{
        //    var user = _context.Users.Find(id);
        //    if (user != null)
        //    {
        //        _context.Users.Remove(user);
        //        _context.SaveChanges();
        //    }
        //}

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}

