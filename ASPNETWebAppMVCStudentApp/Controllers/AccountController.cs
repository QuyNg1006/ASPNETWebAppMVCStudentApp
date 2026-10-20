using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using ASPNETWebAppMVCStudentApp.Models;   

namespace ASPNETWebAppMVCStudentApp.Controllers
{
    public class AccountController : Controller
    {
        private SchoolDBEntities db = new SchoolDBEntities();

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            // Nếu đã đăng nhập thì vào thẳng trang chính
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Kiểm tra thông tin đăng nhập trong bảng tblUser
            var user = db.tblUsers.FirstOrDefault(u =>
                u.Username == model.Username &&
                u.Password == model.Password &&
                u.IsActive);

            if (user == null)
            {
                ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu. Vui lòng nhập lại.");
                return View(model);
            }

            // Đăng nhập thành công -> tạo cookie xác thực
            FormsAuthentication.SetAuthCookie(user.Username, model.RememberMe);

            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}