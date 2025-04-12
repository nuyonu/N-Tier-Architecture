using System;
using System.Collections.Generic;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using N_Tier.Application.MappingProfiles;
using N_Tier.DataAccess.Repositories;
using N_Tier.Shared.Services;
using NSubstitute;

namespace N_Tier.Application.UnitTests.Services;

public class BaseServiceTestConfiguration
{
    protected readonly IClaimService ClaimService;
    protected readonly IConfiguration Configuration;
    protected readonly IMapper Mapper;
    protected readonly ITodoItemRepository TodoItemRepository;
    protected readonly ITodoListRepository TodoListRepository;

    protected BaseServiceTestConfiguration()
    {
        var config = new TypeAdapterConfig();
        
        config.Scan(typeof(IMappingProfilesMarker).Assembly);
        
        // Mapper = new MapperConfiguration(cfg => { cfg.AddMaps(typeof(TodoItemProfile)); }).CreateMapper();
        Mapper = new Mapper(config);

        var configurationBody = new Dictionary<string, string>
        {
            { "JwtConfiguration:SecretKey", "tI079UygByXy52J552Xb4odrUjYXjrPBDuK6FhFv6Qa6eD6SZG" }
        };

        Configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configurationBody)
            .Build();

        TodoListRepository = Substitute.For<ITodoListRepository>();
        TodoItemRepository = Substitute.For<ITodoItemRepository>();

        ClaimService = Substitute.For<IClaimService>();
        ClaimService.GetUserId().Returns(Guid.Empty.ToString());
    }
}
