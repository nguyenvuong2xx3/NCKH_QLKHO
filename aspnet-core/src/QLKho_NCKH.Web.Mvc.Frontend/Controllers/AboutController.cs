using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using QLKho_NCKH.Controllers;

namespace QLKho_NCKH.Web.Controllers
{
    [AbpMvcAuthorize]
    public class AboutController : QLKho_NCKHControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}
