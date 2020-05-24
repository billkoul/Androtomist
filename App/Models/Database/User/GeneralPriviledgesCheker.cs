using System;
using Androtomist.Models.Database.Entities;
using System.Web;
using System.Security.Claims;

namespace Androtomist
{
    public class GeneralPriviledgesCheker
    {
        Models.User.UserClaims claims = new Models.User.UserClaims();

        public bool IsPowerfull()
        {
            long USER_ID_current = Convert.ToInt64(claims.GetSpecificClaim(HttpContext.Current.User.Identity, ClaimTypes.PrimarySid));
            User userCurrent = new User(USER_ID_current);


            if (userCurrent.USER_LEVEL == USER_LEVEL.ADMIN)
                return true;

            return false;
        }
    }
}