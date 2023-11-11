using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Zenith.Models;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

namespace Zenith.Repositories
{
    public class ConfigurationRepository : Repository<Configuration>
    {
        public override Configuration Single(dynamic id)
        {
            string stringId = id.ToString();
            var config = base.Single(stringId);

            return config ?? new Configuration { Key = stringId, Value = string.Empty };
        }

        public void AddOrUpdateRange(List<Configuration> configs)
        {
            configs.ForEach(config =>
            {
                if (_context.Set<Configuration>().Any(c => c.Key == config.Key))
                {
                    var old = base.Single(config.Key);
                    _context.Entry(old).State = EntityState.Detached;
                    _context.Entry(config).State = EntityState.Modified;
                }
                else
                    _context.Set<Configuration>().Add(config);
            });

            _context.SaveChanges();
        }
    }
}
