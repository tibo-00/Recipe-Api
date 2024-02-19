using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Test
{
    public abstract class BaseIntegrationTest : IClassFixture<RecipeApiFactory>
    {
        private readonly IServiceScope _scope;
        protected readonly ISender Sender;
        protected BaseIntegrationTest(RecipeApiFactory factory)
        {
            _scope = factory.Services.CreateScope();

            Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        }
    }
}
