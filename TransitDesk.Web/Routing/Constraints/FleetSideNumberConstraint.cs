using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TransitDesk.Web.Routing.Constraints;

public class FleetSideNumberConstraint : IRouteConstraint
{
    public bool Match(
        HttpContext? httpContext,
        IRouter? route,
        string routeKey,
        RouteValueDictionary values,
        RouteDirection routeDirection)
    {
        if (values.TryGetValue(routeKey, out var rawVal) && rawVal != null)
        {
            if (int.TryParse(rawVal.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int number))
            {
                return number >= 1000 && number <= 9999;
            }
        }

        return false;
    }
}