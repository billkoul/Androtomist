using System;

namespace Androtomist.Models.Database
{
    public abstract class DBClass
    {
        protected IDatabaseConnector databaseConnector;
        protected Converters Converters;
        protected long UserId;

        protected readonly System.Globalization.CultureInfo invar_cul = System.Globalization.CultureInfo.InvariantCulture;

        public DBClass()
        {
            databaseConnector = (IDatabaseConnector)System.Web.HttpContext.Current.RequestServices.GetService(typeof(IDatabaseConnector));

            Converters = new Converters();

            Models.User.UserClaims claims = new Models.User.UserClaims();

            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                UserId = Convert.ToInt64(claims.GetSpecificClaim(System.Web.HttpContext.Current.User.Identity, System.Security.Claims.ClaimTypes.PrimarySid));
            else
                UserId = -1;
        }
    }
}
