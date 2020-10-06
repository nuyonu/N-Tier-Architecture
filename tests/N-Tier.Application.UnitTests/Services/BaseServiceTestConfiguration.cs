using AutoMapper;
using Microsoft.Extensions.Configuration;
using N_Tier.Application.MappingProfiles;
using N_Tier.DataAccess.Repositories;
using N_Tier.Shared.Services;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace N_Tier.Application.UnitTests.Services
{
    public class BaseServiceTestConfiguration
    {
        protected readonly IMapper _mapper;
        protected readonly IConfiguration _configuration;
        protected readonly ITodoListRepository _todoListRepository;
        protected readonly ITodoItemRepository _todoItemRepository;
        protected readonly IClaimService _claimService;

        public BaseServiceTestConfiguration()
        {
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TodoItemProfile());
                cfg.AddProfile(new TodoListProfile());
                cfg.AddProfile(new UserProfile());
            }).CreateMapper();

            var configurationBody = new Dictionary<string, string>
            {
                {"JwtConfiguration:SecretKey", "Super secret token key"},
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configurationBody)
                .Build();

            _todoListRepository = Substitute.For<ITodoListRepository>();
            _todoItemRepository = Substitute.For<ITodoItemRepository>();

            _claimService = Substitute.For<IClaimService>();
            _claimService.GetUserId().Returns(new Guid().ToString());
        }
    }
}
