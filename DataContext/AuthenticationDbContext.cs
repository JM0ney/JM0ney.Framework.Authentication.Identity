using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JM0ney.Framework.Authentication.Identity.DataContext {

    public class AuthenticationDbContext : AuthenticationDbContext<User, Role, UserLogin, UserRole, UserClaim> {

        public static AuthenticationDbContext Create( ) {
            return new AuthenticationDbContext( );
        }

    }


    public class AuthenticationDbContext<TUser, TRole, TUserLogin, TUserRole, TUserClaim> : IdentityDbContext<TUser, TRole, Guid, TUserLogin, TUserRole, TUserClaim>
        where TUser : UserBase<TUser, TUserLogin, TUserRole, TUserClaim>
        where TRole : IdentityRole<Guid, TUserRole>
        where TUserLogin : IdentityUserLogin<Guid>
        where TUserRole : IdentityUserRole<Guid>
        where TUserClaim : IdentityUserClaim<Guid> {

        
        protected override void OnModelCreating( DbModelBuilder modelBuilder ) {
            base.OnModelCreating( modelBuilder );
            modelBuilder.Entity<TRole>( ).Property( x => x.Id ).HasColumnName( "Identity" );
            modelBuilder.Entity<TUser>( ).Property( x => x.Id ).HasColumnName( "Identity" );
            modelBuilder.Entity<TUserClaim>( ).Property( x => x.Id ).HasColumnName( "Identity" );
            modelBuilder.Entity<TUserClaim>( ).Property( x => x.UserId ).HasColumnName( "UserIdentity" );
            modelBuilder.Entity<TUserLogin>( ).Property( x => x.UserId ).HasColumnName( "UserIdentity" );
            modelBuilder.Entity<TUserRole>( ).Property( x => x.UserId ).HasColumnName( "UserIdentity" );
            modelBuilder.Entity<TUserRole>( ).Property( x => x.RoleId ).HasColumnName( "RoleIdentity" );
        }
    }

}
