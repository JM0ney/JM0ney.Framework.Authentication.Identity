using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JM0ney.Framework.Authentication.Identity {

    public class User : UserBase<User, UserLogin, UserRole, UserClaim> {


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync( Managers.UserManager manager ) {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
             var userIdentity = await manager.CreateIdentityAsync( ( User ) this, DefaultAuthenticationTypes.ApplicationCookie );
            // Add custom user claims here

            return userIdentity;
        }

    }

    public abstract class UserBase<TUser, TUserLogin, TUserRole, TUserClaim> : 
        IdentityUser<Guid, TUserLogin, TUserRole, TUserClaim> //, IUserIdentityMapper<TUser, TUserLogin, TUserRole, TUserClaim>
        where TUser : UserBase<TUser, TUserLogin, TUserRole, TUserClaim>
        where TUserLogin : IdentityUserLogin<Guid>
        where TUserRole : IdentityUserRole<Guid>
        where TUserClaim : IdentityUserClaim<Guid> {

        private String _FirstName = String.Empty;
        private String _LastName = String.Empty;


        public string FirstName {
            get {
                return _FirstName;
            }

            set {
                this._FirstName = value;
            }
        }

        public string LastName {
            get {
                return _LastName;
            }

            set {
                this._LastName = value;
            }
        }
        
    }

}
