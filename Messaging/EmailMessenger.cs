using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace JM0ney.Framework.Authentication.Identity.Messaging {
    
    public class EmailMessenger : IIdentityMessageService {

        #region IIdentityMessageService Members

        public Task SendAsync( IdentityMessage message ) {
            var result = JM0ney.Framework.Web.EmailHelper.SendEmail( message.Destination, message.Subject, message.Body );
            return Task.FromResult( result.IsSuccess );
        }

        #endregion

    }

}
