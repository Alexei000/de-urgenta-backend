﻿using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Models;
using DeUrgenta.Group.Api.Options;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeUrgenta.Group.Api.CommandHandlers
{
    public class AddGroupHandler : IRequestHandler<AddGroup, Result<GroupModel, ValidationResult>>
    {
        private readonly IValidateRequest<AddGroup> _validator;
        private readonly DeUrgentaContext _context;
        private readonly GroupsConfig _groupsConfig;

        public AddGroupHandler(IValidateRequest<AddGroup> validator, DeUrgentaContext context,
            IOptions<GroupsConfig> groupsConfig)
        {
            _validator = validator;
            _context = context;
            _groupsConfig = groupsConfig.Value;
        }

        public async Task<Result<GroupModel, ValidationResult>> Handle(AddGroup request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, cancellationToken);

            var newBackpack = new Backpack {Name = $"Backpack for {request.Group.Name}"};

            var group = new Domain.Api.Entities.Group {Admin = user, Name = request.Group.Name, Backpack = newBackpack};

            var newGroup = await _context.Groups.AddAsync(group, cancellationToken);

            await _context.UsersToGroups.AddAsync(
                new UserToGroup {User = user, Group = group}, cancellationToken);

            await _context.BackpacksToUsers.AddAsync(new BackpackToUser {Backpack = newBackpack, User = user},
                cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return new GroupModel
            {
                Id = newGroup.Entity.Id,
                Name = newGroup.Entity.Name,
                NumberOfMembers = newGroup.Entity.GroupMembers.Count,
                MaxNumberOfMembers = _groupsConfig.MaxUsers,
                IsCurrentUserAdmin = newGroup.Entity.AdminId == user.Id
            };
        }
    }
}