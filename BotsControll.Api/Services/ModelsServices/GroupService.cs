using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BotsControll.Core.Dtos.Groups;
using BotsControll.Core.Models;
using BotsControll.Core.Models.Base;
using BotsControll.Core.Repositories;

namespace BotsControll.Api.Services.ModelsServices;

public class GroupService
{
    private readonly IGroupRepository _groupRepository;

    public GroupService(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<Group> Create(GroupCreationDto group, IUserIdentity creator)
    {
        var newGroup = new Group()
        {
            Id = Guid.NewGuid().ToString(),
            OwnerId = creator.Id,
            Name = group.Name,
        };

        return await _groupRepository.Add(newGroup);
    }

    public async Task<Group> Remove(string id, IUserIdentity remover)
    {
        var group = await _groupRepository.GetById(id);
        if (group.OwnerId != remover.Id)
            throw new Exception("You aren't group owner");

        return await _groupRepository.Delete(group);
    }

    public async Task<List<Group>> GetOwned(IUserIdentity user)
    {
        return await _groupRepository.GetOwnedByUserId(user.Id);
    }
}