using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace JM0ney.Framework.Authentication.Identity.Managers {

    public class RoleManager : RoleManager<Role, UserRole> {

        public RoleManager( IRoleStore<Role, Guid> store )
            : base( store ) {
        }

        public static RoleManager Create( IdentityFactoryOptions<RoleManager> options, IOwinContext context ) {
            return new RoleManager( new RoleStore<Role, Guid, UserRole>( context.Get<DataContext.AuthenticationDbContext>( ) ) );
        }
    }

    public class RoleManager<TRole, TUserRole> : Microsoft.AspNet.Identity.RoleManager<TRole, Guid>
        where TRole : IdentityRole<Guid, TUserRole>
        where TUserRole : IdentityUserRole<Guid> {

        public RoleManager( IRoleStore<TRole, Guid> store )
            : base( store ) {
        }

    }

}
