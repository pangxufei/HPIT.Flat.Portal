
using HPIT.Survey.Portal.Filters;
using MVCLearn.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Portal.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomAuthenticationFilter());
            //filters.Add(new ActionResultFilter());
        }
    }
}