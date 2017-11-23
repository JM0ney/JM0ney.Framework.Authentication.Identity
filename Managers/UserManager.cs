using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JM0ney.Framework.Authentication.Identity.Managers {

    public class UserManager : UserManager<User, UserLogin, UserRole, UserClaim> {

        public UserManager( Microsoft.AspNet.Identity.IUserStore<User, Guid> store )
            : base( store ) {
        }

        // TODO:  Need to find a way to abstract this out, so this can be implemented in a more extensible manner.
        public static UserManager Create( IdentityFactoryOptions<UserManager> options, Microsoft.Owin.IOwinContext context ) {
            DataContext.AuthenticationDbContext dataContext = context.Get<DataContext.AuthenticationDbContext>( );
            UserManager manager = new UserManager( new UserStore<User, Role, Guid, UserLogin, UserRole, UserClaim>( dataContext ) );
            manager.UserValidator = manager.GetUserValidator( );
            manager.PasswordValidator = manager.GetPasswordValidator( );
            manager.UserLockoutEnabledByDefault = manager.LockoutEnabledByDefault;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes( manager.LockoutTimespanMinutes );
            manager.MaxFailedAccessAttemptsBeforeLockout = manager.LockoutFailedAttemptsAllowed;

            //// Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            //// You can write your own provider and plug it in here.
            //manager.RegisterTwoFactorProvider( "Phone Code", new PhoneNumberTokenProvider<ApplicationUser> {
            //    MessageFormat = "Your security code is {0}"
            //} );
            //manager.RegisterTwoFactorProvider( "Email Code", new EmailTokenProvider<ApplicationUser> {
            //    Subject = "Security Code",
            //    BodyFormat = "Your security code is {0}"
            //} );

            Microsoft.AspNet.Identity.IIdentityMessageService service = manager.GetEmailService( );
            if ( service != null ) {
                manager.EmailService = service;
            }

            service = manager.GetSMSService( );
            if ( service != null ) {
                manager.SmsService = service;
            }

            var dataProtectionProvider = options.DataProtectionProvider;
            if ( dataProtectionProvider != null ) {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<User, Guid>( dataProtectionProvider.Create( "ASP.NET Identity" ) );
            }

            return manager;
        }

    }

    public class UserManager<TUser, TUserLogin, TUserRole, TUserClaim> : Microsoft.AspNet.Identity.UserManager<TUser, Guid>
        //where TUser : IdentityUser<Guid, TUserLogin, TUserRole, TUserClaim> //, IUserIdentityMapper<TUser, TUserLogin, TUserRole, TUserClaim>
        where TUser : UserBase<TUser, TUserLogin, TUserRole, TUserClaim> //, IUserIdentityMapper<TUser, TUserLogin, TUserRole, TUserClaim>
        where TUserLogin : IdentityUserLogin<Guid>
        where TUserRole : IdentityUserRole<Guid>
        where TUserClaim : IdentityUserClaim<Guid> {

        public UserManager( Microsoft.AspNet.Identity.IUserStore<TUser, Guid> store )
            : base( store ) {
        }

        protected bool LockoutEnabledByDefault {
            get { return true; }
        }

        protected byte LockoutTimespanMinutes {
            get { return 15; }
        }

        protected byte LockoutFailedAttemptsAllowed {
            get { return 3; }
        }

        protected String AppendUrlParts( String leftPart, String rightPart ) {
            String realUrl = leftPart;
            if ( realUrl.EndsWith( "/" ) && rightPart.StartsWith( "/" ) ) {
                rightPart = rightPart.Substring( 1 );
            }
            return realUrl + rightPart;
        }

        protected Microsoft.AspNet.Identity.IUserTokenProvider<TUser, Guid> GetDefaultUserTokenProvider( Microsoft.Owin.Security.DataProtection.IDataProtectionProvider dataProtectionProvider ) {
            return new DataProtectorTokenProvider<TUser, Guid>( dataProtectionProvider.Create( "ASP.NET Identity" ) );
        }

        /// <summary>
        /// Returns a password validator that requires 6 characters, an uppercase character, a lowercase character, and non letter or digit
        /// </summary>
        protected Microsoft.AspNet.Identity.IIdentityValidator<String> GetPasswordValidator( ) {
            Microsoft.AspNet.Identity.PasswordValidator validator = new Microsoft.AspNet.Identity.PasswordValidator( );
            validator.RequireNonLetterOrDigit = validator.RequireUppercase = validator.RequireLowercase = true;
            validator.RequiredLength = 6;
            return validator;
        }

        /// <summary>
        /// Returns a user validator that requires an unique email address
        /// </summary>
        protected Microsoft.AspNet.Identity.IIdentityValidator<TUser> GetUserValidator( ) {
            Microsoft.AspNet.Identity.UserValidator<TUser, Guid> validator = new Microsoft.AspNet.Identity.UserValidator<TUser, Guid>( this );
            validator.RequireUniqueEmail = true;
            return validator;
        }

        protected Microsoft.AspNet.Identity.IIdentityMessageService GetEmailService( ) {
            return new Messaging.EmailMessenger( );
        }

        protected Microsoft.AspNet.Identity.IIdentityMessageService GetSMSService( ) {
            return null;
        }

        public Result UnlockOrActivateAccount( TUser userAccount ) {
            IdentityResult identityResult = this.ResetAccessFailedCount( userAccount.Id );
            if ( identityResult.Succeeded )
                identityResult = this.SetLockoutEndDate( userAccount.Id, DateTimeOffset.MinValue );

            return identityResult.Succeeded ?
                Result.SuccessResult( ) :
                Result.ErrorResult( String.Join( "; ", identityResult.Errors ) );
        }

        public Result DeactivateAccount( TUser userAccount ) {
            IdentityResult identityResult = this.SetLockoutEndDate( userAccount.Id, DateTimeOffset.MaxValue );
            return identityResult.Succeeded ?
                Result.SuccessResult( ) :
                Result.ErrorResult( String.Join( "; ", identityResult.Errors ) );
        }

        //////// TODO:  Need to find a way to abstract this out, so this can be implemented in a more extensible manner.
        //////public static UserManager Create( IdentityFactoryOptions<UserManager> options, Microsoft.Owin.IOwinContext context ) {
        //////    DataContext.AuthenticationDbContext dataContext = context.Get<DataContext.AuthenticationDbContext>( );
        //////    UserManager manager = new UserManager( new UserStore<User, Role, Guid, UserLogin, UserRole, UserClaim>( dataContext ) );
        //////    manager.UserValidator = manager.GetUserValidator( );
        //////    manager.PasswordValidator = manager.GetPasswordValidator( );
        //////    manager.UserLockoutEnabledByDefault = manager.LockoutEnabledByDefault;
        //////    manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes( manager.LockoutTimespanMinutes );
        //////    manager.MaxFailedAccessAttemptsBeforeLockout = manager.LockoutFailedAttemptsAllowed;

        //////    //// Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
        //////    //// You can write your own provider and plug it in here.
        //////    //manager.RegisterTwoFactorProvider( "Phone Code", new PhoneNumberTokenProvider<ApplicationUser> {
        //////    //    MessageFormat = "Your security code is {0}"
        //////    //} );
        //////    //manager.RegisterTwoFactorProvider( "Email Code", new EmailTokenProvider<ApplicationUser> {
        //////    //    Subject = "Security Code",
        //////    //    BodyFormat = "Your security code is {0}"
        //////    //} );

        //////    Microsoft.AspNet.Identity.IIdentityMessageService service = manager.GetEmailService( );
        //////    if ( service != null ) { 
        //////        manager.EmailService = service;
        //////    }

        //////    service = manager.GetSMSService( );
        //////    if ( service != null ) {
        //////        manager.SmsService = service;
        //////    }

        //////    var dataProtectionProvider = options.DataProtectionProvider;
        //////    if ( dataProtectionProvider != null ) {
        //////        manager.UserTokenProvider =
        //////            new DataProtectorTokenProvider<User, Guid>( dataProtectionProvider.Create( "ASP.NET Identity" ) );
        //////    }

        //////    return manager;
        //////}
    }

}
