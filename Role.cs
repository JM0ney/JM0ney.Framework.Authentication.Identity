using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JM0ney.Framework.Authentication.Identity {

    public class Role : Role<UserRole> {
    }

    public class Role<TUserRole> : IdentityRole<Guid, TUserRole>
        where TUserRole : UserRole {
    }

}
