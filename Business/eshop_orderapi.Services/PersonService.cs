using AutoMapper;
using Microsoft.AspNetCore.Identity;
using eshop_orderapi.Business.ViewModels;
using eshop_orderapi.Domain;
using eshop_orderapi.Domain.Models;
using eshop_orderapi.Interfaces.Services;
using eshop_orderapi.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_orderapi.Services
{
    public class PersonService : ServiceBase, IPersonService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PersonService(IUnitOfWork unitOfWork, IMapper _mapper, UserManager<ApplicationUser> userManager) : base(unitOfWork, _mapper)
        {
            _userManager = userManager;
        }

        public async Task<List<PersonModel>> GetAllAsync()
        {
            var result = mapper.Map<List<PersonModel>>(await unitOfWork.PersonRepository.GetAllAsync());
            return result.ToList();
        }

        public async Task<PersonModel> GetAsync(int id)
        {
            return mapper.Map<PersonModel>(await unitOfWork.PersonRepository.GetAsync(id));
        }

        public async Task<bool> AddAsync(Person model)
        {
            var user = new ApplicationUser { UserName = model.Name, Email = "Test@gmail.com" };
            var result = await _userManager.CreateAsync(user, "iFour@1234");

            if (result.Succeeded)
            {
                try
                {
                    var person = new Person { Name = model.Name, IsActive = model.IsActive, RoleId = model.RoleId, AspNetUserId = user.Id };
                    await unitOfWork.PersonRepository.AddAsync(person); 
                }
                catch (Exception ex)
                {
                    user = await _userManager.FindByNameAsync(model.Name);
                    await _userManager.DeleteAsync(user);
                    throw ex;
                }
                
            }
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateAsync(Person model)
        {
            var person = await unitOfWork.PersonRepository.GetAsync(model.Id);
            if (person != null)
            {
                person.Id = model.Id;
                //MAP other fields
                await unitOfWork.PersonRepository.UpdateAsync(person);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var person = unitOfWork.PersonRepository.GetAsync(id).Result;
            if (person != null)
            {
                await unitOfWork.PersonRepository.DeleteAsync(person);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> ForgotPassword(string aspNetUserId, Guid resetId)
        {
            var updateAspNetUser = await _userManager.FindByIdAsync(aspNetUserId);

            if (updateAspNetUser != null)
            {
                updateAspNetUser.PasswordResetDate = DateTime.UtcNow;
                updateAspNetUser.PasswordReset = resetId;
                await _userManager.UpdateAsync(updateAspNetUser);
                return true;
            }
            return true;
        }

        public async Task<string> ResetPassword(ChangePassword model)
        {
            var applicationUser = await _userManager.FindByEmailAsync(model.Email);
            if (applicationUser != null)
            {
                string hashedNewPassword = _userManager.PasswordHasher.HashPassword(applicationUser, model.NewPassword);
                applicationUser.PasswordHash = hashedNewPassword;

                await _userManager.UpdateAsync(applicationUser);

                return await Task.FromResult(applicationUser.Email);
            }
            return await Task.FromResult(applicationUser.Email);
        }

        public async Task<string> ForgotPassword(ResetPasswordViewModel model)
        {
            var applicationUser = _userManager.Users.Where(c => c.PasswordReset == model.ResetId).FirstOrDefault();
            if (applicationUser != null)
            {
                string hashedNewPassword = _userManager.PasswordHasher.HashPassword(applicationUser, model.Password);
                applicationUser.PasswordHash = hashedNewPassword;

                applicationUser.PasswordReset = null;
                applicationUser.PasswordResetDate = null;
                await _userManager.UpdateAsync(applicationUser);

                return await Task.FromResult(applicationUser.Email);
            }
            return await Task.FromResult(applicationUser.Email);
        }

        public async Task<bool> ResetForgotPassword(string aspNetUserId, Guid resetId)
        {
            var updateAspNetUser = await _userManager.FindByIdAsync(aspNetUserId);

            if (updateAspNetUser != null)
            {
                updateAspNetUser.PasswordResetDate = DateTime.UtcNow;
                updateAspNetUser.PasswordReset = resetId;
                await _userManager.UpdateAsync(updateAspNetUser);
                return true;
            }
            return true;
        }

        public bool IsResetIdExist(Guid resetId)
        {
            return _userManager.Users.Any(c => c.PasswordReset == resetId);
        }

        public int GetRoleIdBaseonUserid(string id)
        {
            return unitOfWork.PersonRepository.GetRoleIdBaseonUserid(id);
        }
    }
}