using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Resources;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using SOPCOVIDChecker.Data;
using SOPCOVIDChecker.Models;
using SOPCOVIDChecker.Models.AccountViewModels;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace DPADIS.Services
{
    public interface IUserService
    {
        Task<(bool, Sopusers)> ValidateUserCredentialsAsync(string username, string password);
        void ChangePasswordAsync(Sopusers user, string newPassword);

        Task<bool> RegisterUserAsync(AddUserModel model);
        Task UpdateUserAsync(UpdateUserModel model);

    }
    public class UserService : IUserService
    {
        private readonly SOPCCContext _context;

        public PasswordHasher<Sopusers> _hashPassword = new PasswordHasher<Sopusers>();

        

        public UserService(SOPCCContext context)
        {
            _context = context;
        }

        public async Task<(bool, Sopusers)> ValidateUserCredentialsAsync(string username, string password)
        {
            Sopusers user = null;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return (false, user);

            user = await _context.Sopusers
                .Include(x=>x.Facility)
                .SingleOrDefaultAsync(x => x.Username.Equals(username));

            if (user == null)
                return (false, user);

            else
            {
                try
                {
                    var result = _hashPassword.VerifyHashedPassword(user, user.Password, password);
                    if (result.Equals(PasswordVerificationResult.Success))
                    {
                        user.UpdatedAt = DateTime.Now;
                        _context.Update(user);
                        return (true, user);
                    }
                    else
                        return (false, user);
                }
                catch
                {
                    return (false, user);
                }
            }
            
        }

        public void ChangePasswordAsync(Sopusers user, string newPassword)
        {
            var hashedPassword = _hashPassword.HashPassword(user, newPassword);

            user.Password = hashedPassword;
            user.UpdatedAt = DateTime.Now;

            _context.Update(user);
            _context.SaveChanges();
        }

        public Task<bool> RegisterUserAsync(AddUserModel model)
        {
            if (_context.Sopusers.Any(x => x.Username.Equals(model.Username)))
            {
                return Task.FromResult(false);
            }
            else
            {
                var facility = _context.Facility.First(x => x.Id.Equals(model.FacilityId));
                Sopusers newUser = new Sopusers();
                string hashedPass = _hashPassword.HashPassword(newUser, model.Password);
                newUser.Firstname = model.Firstname;
                newUser.Middlename = model.Middlename;
                newUser.Lastname = model.Lastname;
                newUser.Contact = model.ContactNumber;
                newUser.Email = model.Email;
                newUser.FacilityId = model.FacilityId;
                newUser.Username = model.Username;
                newUser.Password = hashedPass;
                newUser.UserLevel = "user";
                newUser.BarangayId = (int)facility.BarangayId;
                newUser.MuncityId = (int)facility.MuncityId;
                newUser.ProvinceId = (int)facility.ProvinceId;
                newUser.CreatedAt = DateTime.Now;
                newUser.UpdatedAt = DateTime.Now;
                _context.Add(newUser);
                _context.SaveChanges();
                return Task.FromResult(true);
            }
        }

        public async Task UpdateUserAsync(UpdateUserModel model)
        {
            var user = await _context.Sopusers.FindAsync(model.Id);
            var facility = _context.Facility.First(x => x.Id.Equals(model.FacilityId));
            if(!string.IsNullOrEmpty(model.ConfirmPassword) && !string.IsNullOrEmpty(model.Password))
            {
                string hashedPass = _hashPassword.HashPassword(user, model.Password);
                user.Password = hashedPass;
            }
            user.Firstname = model.Firstname;
            user.Middlename = model.Middlename;
            user.Lastname = model.Lastname;
            user.Contact = model.ContactNumber;
            user.Email = model.Email;
            user.FacilityId = model.FacilityId;
            user.Username = model.Username;
            user.BarangayId = (int)facility.BarangayId;
            user.MuncityId = (int)facility.MuncityId;
            user.ProvinceId = (int)facility.ProvinceId;
            user.UpdatedAt = DateTime.Now;
            _context.Add(user);
            _context.SaveChanges();
        }
    }
}