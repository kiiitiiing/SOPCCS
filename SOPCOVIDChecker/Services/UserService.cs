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

namespace SOPCOVIDChecker.Services
{
    public interface IUserService
    {
        Task<(bool, Sopusers)> ValidateUserCredentialsAsync(string username, string password);
        void ChangePasswordAsync(Sopusers user, string newPassword);

        Task<bool> RegisterUserAsync(AddUserModel model, string level);
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

        public Task<bool> RegisterUserAsync(AddUserModel model, string level)
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
                newUser.Fname = model.Firstname;
                newUser.Mname = model.Middlename;
                newUser.Lname = model.Lastname;
                newUser.ContactNo = model.ContactNumber;
                newUser.Email = model.Email;
                newUser.FacilityId = model.FacilityId;
                newUser.Username = model.Username;
                newUser.Password = hashedPass;
                newUser.UserLevel = level;
                newUser.Barangay = facility.Barangay;
                newUser.Muncity = facility.Muncity;
                newUser.Province = (int)facility.Province;
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
            user.Fname = model.Firstname;
            user.Mname = model.Middlename;
            user.Lname = model.Lastname;
            user.ContactNo = model.ContactNumber;
            user.Email = model.Email;
            user.FacilityId = model.FacilityId;
            user.Username = model.Username;
            user.Barangay = (int)facility.Barangay;
            user.Muncity = (int)facility.Muncity;
            user.Province = (int)facility.Province;
            user.UpdatedAt = DateTime.Now;
            _context.Add(user);
            _context.SaveChanges();
        }
    }
}