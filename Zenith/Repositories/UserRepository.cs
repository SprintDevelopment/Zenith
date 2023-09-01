using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Zenith.Assets.Extensions;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class UserRepository : Repository<User>
    {
        UserPermissionRepository UserPermissionRepository = new UserPermissionRepository();

        public override IEnumerable<User> Find(Expression<Func<User, bool>> predicate)
        {
            return _context.Set<User>()
                .Include(u => u.Permissions)
                .Where(predicate).AsEnumerable();
        }

        public override User Single(dynamic id)
        {
            string username = (string)id;
            return _context.Set<User>()
                .Include(u => u.Permissions)
                .SingleOrDefault(u => u.Username == username);
        }

        public override User Add(User user)
        {
            base.Add(user);
            UserPermissionRepository.AddRange(user.Permissions.Select(p => { p.Username = user.Username; return p; }));

            return user;
        }

        public override User Update(User user, dynamic userId)
        {
            base.Update(user, user.Username);

            user.Permissions.ToList().ForEach(p =>
            {
                if (p.UserPermissionId == 0)
                {
                    p.Username = user.Username;
                    UserPermissionRepository.Add(p);
                }
                else
                    UserPermissionRepository.Update(p, p.UserPermissionId);
            });

            return user;
        }
    }
}
