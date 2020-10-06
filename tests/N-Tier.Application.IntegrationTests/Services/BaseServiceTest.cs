using AutoMapper;
using Microsoft.EntityFrameworkCore;
using N_Tier.Application.MappingProfiles;
using N_Tier.DataAccess.Persistence;
using N_Tier.Shared.Services;
using NSubstitute;
using System;

namespace N_Tier.Application.IntegrationTests.Services
{
    public class BaseServiceTest
    {
        protected readonly DatabaseContext _context;
        protected readonly IMapper _mapper;

        public BaseServiceTest()
        {
            DbContextOptions<DatabaseContext> options;
            var builder = new DbContextOptionsBuilder<DatabaseContext>();
            builder.UseInMemoryDatabase("Integration tests");
            options = builder.Options;

            var claimService = Substitute.For<IClaimService>();

            claimService.GetUserId().Returns(new Guid().ToString());

            _context = new DatabaseContext(options, claimService);

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TodoItemProfile());
                cfg.AddProfile(new TodoListProfile());
                cfg.AddProfile(new UserProfile());
            });

            _mapper = mockMapper.CreateMapper();
        }
    }
}
