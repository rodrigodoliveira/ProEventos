using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.Contracts;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Application
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserPersistence _userPersistence;

        public AccountService(UserManager<User> userManager,
                            SignInManager<User> signInManager,
                            IMapper mapper,
                            IUserPersistence userPersistence)
        {
            this._userPersistence = userPersistence;
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._mapper = mapper;
        }

        public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password)
        {
            try
            {
                var user = await _userManager.Users.SingleOrDefaultAsync(user => user.UserName == userUpdateDto.UserName.ToLower());
                return await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar verificar password. Erro: {ex.Message}", ex);
            }
        }

        public async Task<UserDto> CreateAccountAsync(UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (result.Succeeded)
                {
                    return _mapper.Map<UserDto>(user);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar criar usuário. Erro: {ex.Message}", ex);
            }
        }

        public async Task<UserUpdateDto> GetUserByUserNameAsync(string username)
        {
            try
            {
                var user = await _userPersistence.GetUserByUserNameAsync(username);
                if (user == null) return null;

                return _mapper.Map<UserUpdateDto>(user);

            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar obter usuário por username. Erro: {ex.Message}", ex);
            }
        }

        public async Task<UserUpdateDto> UpdateAccount(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _userPersistence.GetUserByUserNameAsync(userUpdateDto.UserName);
                if (user == null) return null;

                _mapper.Map(userUpdateDto, user);

                var token = await _userManager.GeneratePasswordResetTokenAsync(user); //reseta um token para que o usuario seja deslogado e gere um novo token
                var result = await _userManager.ResetPasswordAsync(user, token, userUpdateDto.Password); // pra funcionar tem que adicionar o .AddDefaultTokenProviders no startup.

                _userPersistence.Update<User>(user);
                if (await _userPersistence.SaveChangesAsync())
                {
                    var userReturn = await _userPersistence.GetUserByUserNameAsync(user.UserName);
                    return _mapper.Map(userReturn, userUpdateDto);
                }

                return null;

            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar atualizar usuário. Erro: {ex.Message}", ex);
            }
        }

        public async Task<bool> UserExistsAsync(string userName)
        {
            try
            {
                return await _userManager.Users
                    .AnyAsync(user => user.UserName == userName.ToLower());

            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar verificar se o usuário existe. Erro: {ex.Message}", ex);
            }
        }
    }
}