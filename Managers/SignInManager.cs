using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Web;

namespace JM0ney.Framework.Authentication.Identity.Managers {

    public class SignInManager : SignInManager<UserManager, User, UserLogin, UserRole, UserClaim> {

        public SignInManager( UserManager userManager, IAuthenticationManager authenticationManager )
          : base( userManager, authenticationManager ) { }

        public static SignInManager Create( IdentityFactoryOptions<SignInManager> options, IOwinContext context ) {
            return new SignInManager( context.GetUserManager<UserManager>( ), context.Authentication );
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync( User user ) {
            return user.GenerateUserIdentityAsync( (Managers.UserManager) this.UserManager );
           //  return base.CreateUserIdentityAsync( user );
        }

    }


    public abstract class SignInManager<TUserManager, TUser, TUserLogin, TUserRole, TUserClaim> : SignInManager<TUser, Guid>
        where TUserManager : UserManager<TUser, TUserLogin, TUserRole, TUserClaim>
        where TUser : UserBase<TUser, TUserLogin, TUserRole, TUserClaim> 
        where TUserLogin : IdentityUserLogin<Guid>
        where TUserRole : IdentityUserRole<Guid>
        where TUserClaim : IdentityUserClaim<Guid> {

        public SignInManager( TUserManager userManager, IAuthenticationManager authenticationManager )
            : base( userManager, authenticationManager ) { }
    }

}