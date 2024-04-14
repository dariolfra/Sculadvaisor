using Microsoft.AspNetCore.Mvc.Filters;
using System;
using Microsoft.AspNetCore.Mvc;

namespace schooladvisor.Filters
{
    public class OnlyAdmin:ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ISession session = context.HttpContext.Session;
            string utente = session.GetString("utente");
            if (utente == "admin") return; // si puo procedere con l'azione richiesta
            else
            {
                context.Result = new RedirectToActionResult("AccessoNegato","Home",null);
            }
        }
    }
}
