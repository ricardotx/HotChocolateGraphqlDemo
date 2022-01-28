﻿using HotChocolateGraphqlDemo.Api.Src.Data.Entities;
using HotChocolateGraphqlDemo.Api.Src.Data.Repositories;
using HotChocolateGraphqlDemo.Api.Src.Models;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotChocolateGraphqlDemo.Api.Src.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository _repo;

        public UserService(IRepository repo)
        {
            _repo = repo;
        }

        public async Task<User> CreateAsync(User user)
        {
            await _repo.Users.AddAsync(user);
            await _repo.SaveChangesAsync();
            return user;
        }

        public async Task<string> DeleteAsync(Guid userId)
        {
            var dataModel = await _repo.Users.FindByIdAsync(userId);
            _repo.Users.Remove(dataModel);
            await _repo.SaveChangesAsync();
            return $"The user with the id: '{ userId}' has been successfully deleted from db.";
        }

        public async Task<bool> DeleteAsync(Guid[] userIds)
        {
            for (int i = 0; i < userIds.Length; i++)
            {
                await DeleteAsync(userIds[i]);
            }

            return true;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var dataModels = await _repo.Users.FindAllAsync();
            return dataModels;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var dataModel = await _repo.Users.FindOneAsync(x => x.Email == email);
            return dataModel;
        }

        public async Task<User> GetByIdAsync(Guid userId)
        {
            var dataModel = await _repo.Users.FindByIdAsync(userId);
            return dataModel;
        }

        public async Task<User> UpdateAsync(User user)
        {
            var dataModel = await _repo.Users.FindByIdAsync(user.Id);
            dataModel.Name = user.Name ?? dataModel.Name;
            dataModel.Status = Enum.IsDefined(typeof(EnumUserStatus), user.Status) ? user.Status : dataModel.Status;
            dataModel.RoleId = user.RoleId != Guid.Empty ? user.RoleId : dataModel.RoleId;
            await _repo.SaveChangesAsync();
            return dataModel;
        }
    }
}
