using BotsControll.Core.Dtos.Bots;
using BotsControll.Core.Models;
using BotsControll.Core.Models.Base;
using BotsControll.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotsControll.Api.Services.ModelsServices;

public class BotService
{
    private readonly IBotsRepository _botsRepository;

    public BotService(IBotsRepository botsRepository)
    {
        _botsRepository = botsRepository;
    }

    public Task<List<Bot>> GetOwned(IUserIdentity user)
    {
        return _botsRepository.GetOwnedByUserId(user.Id);
    }

    public Task<List<Bot>> GetAvailable(IUserIdentity user)
    {
        return _botsRepository.GetAvailableByUserId(user.Id);
    }

    public async Task<BotDto> Create(BotCreationDto botDto, IUserIdentity creator)
    {
        var botId = Guid.NewGuid().ToString();
        var bot = new Bot()
        {
            Id = botId,
            Name = botDto.Name,
            OwnerId = creator.Id,
            IsActive = false,
            BotGroups = botDto.Groups.Select(g => new BotGroup()
            {
                BotId = botId,
                GroupId = g.Id
            }).ToList(),
            Journals = new List<Journal>()
        };
        var createdBot = await _botsRepository.Add(bot);

        return (BotDto)createdBot;
    }

    public async Task<BotDto> Remove(string id, IUserIdentity remover)
    {
        try
        {
            var bot = await _botsRepository.GetById(id);
            if (bot.OwnerId != remover.Id)
                throw new Exception("You aren't bot owner");
            await _botsRepository.Delete(bot);
            return (BotDto)bot;
        }
        catch 
        {
            throw new Exception($"No bot with id {id}");
        }
    }

}