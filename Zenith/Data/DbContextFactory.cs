using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Zenith.Data
{
    public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseSqlServer(@"Data Source=.;Initial Catalog=Zenith;Integrated Security=True;Encrypt=False"
                                        , providerOptions => providerOptions.EnableRetryOnFailure())
                        .EnableSensitiveDataLogging().EnableThreadSafetyChecks(false)
                        .LogTo((message) => Debug.WriteLine(message));

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
