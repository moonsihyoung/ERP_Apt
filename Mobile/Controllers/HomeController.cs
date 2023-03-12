using Erp_Apt_Lib.Logs;
using Erp_Apt_Staff;
using Erp_Entity;
using Erp_Lib;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Mobile.Controllers
{
    public class HomeController : Controller
    {
        private IStaff_Lib _staff_Lib;
        private IReferral_career_Lib _Career_Lib;
        private IPresent_Lib _Present_Lib;
        private IIn_AptPeople_Lib _AptPople_Lib;
        private ILogs_Lib _logs_Lib;

        public HomeController(
            IStaff_Lib staff_Lib,
            IReferral_career_Lib referral_Career_Lib,
            IPresent_Lib present_Lib,
            IIn_AptPeople_Lib aptPople_Lib,
            ILogs_Lib logs_Lib)
        {
            _staff_Lib = staff_Lib;
            _Career_Lib = referral_Career_Lib;
            _Present_Lib = present_Lib;
            _AptPople_Lib = aptPople_Lib;
            _logs_Lib = logs_Lib;
        }
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 로그인
        /// </summary>
        /// <param name="User_Code"></param>
        /// <param name="sw_Pass"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Index(string Mobile, string sw_Pass)
        {
            if (!string.IsNullOrWhiteSpace(Mobile) && !string.IsNullOrWhiteSpace(sw_Pass))
            {
                In_AptPeople_Entity ann = new In_AptPeople_Entity();

                string MobileB = Mobile.Replace("-", "");
                string MobileA = MobileB.Substring(0, 3) + "-" + MobileB.Substring(3, 4) + "-" + MobileB.Substring(7, 4);

                int in_be = await _AptPople_Lib.Log_views_M(MobileA, MobileB, sw_Pass);
                //Referral_career_Entity ann = new Referral_career_Entity();
                //int in_be = await _staff_Lib.Login(User_Code, sw_Pass);
                //int LevelCount = await _staff_Lib.Staff_LevelCount(User_Code);
                if (in_be > 0)
                {
                    ann = await _AptPople_Lib.Detail_M(Mobile);

                    if (ann.LevelCount >= 2)
                    {
                        var claims = new List<Claim>()
                        {
                            new Claim("User_Code", ann.User_Code),
                            new Claim(ClaimTypes.Name, ann.User_Name),
                            new Claim("Apt_Code", ann.Apt_Code),
                            new Claim("Apt_Name", ann.Apt_Name),
                            new Claim("LevelCount", ann.LevelCount.ToString()),
                            new Claim("Dong", ann.Dong),
                            new Claim("Ho", ann.Ho),
                            new Claim("Mobile", ann.Mobile)
                        };

                        var ci = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci));
                        HttpContext.Session.SetString("SessionVariable1", "Testing123");

                        await _AptPople_Lib.VisitCount_Add(ann.User_Code);

                        Logs_Entites dnn = new Logs_Entites();
                        dnn.Apt_Code = ann.Apt_Code;
                        dnn.Note = ann.User_Name;
                        dnn.Application = "모바일 로그인";
                        dnn.LogEvent = "클릭";
                        dnn.Callsite = "";
                        dnn.Exception = "";
                        dnn.ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                        dnn.Level = "3";
                        dnn.Logger = ann.Dong + "-" + ann.Ho;
                        dnn.Message = ann.Apt_Name + " " + ann.User_Name;
                        dnn.MessageTemplate = "";
                        dnn.Properties = "";
                        dnn.TimeStamp = DateTime.Now.ToShortDateString();
                        await _logs_Lib.add(dnn);


                        return LocalRedirect(Url.Content("~/"));
                    }
                    else
                    {
                        TempData["message"] = "아직 승인되지 않았습니다. 관리사무소에 문의바랍니다.";
                        //ViewBag.Message = string.Format("로그인되지 않았습니다. \n 아이디나 암호를 확인하세요.");
                        return View();
                    }

                }
                else
                {
                    TempData["message"] = "로그인되지 않았습니다. 아이디나 암호를 확인하세요.";
                    //ViewBag.Message = string.Format("로그인되지 않았습니다. \n 아이디나 암호를 확인하세요.");
                    return View();
                }
                //return View(); 
            }
            else
            {
                TempData["message"] = "로그인되지 않았습니다. 아이디나 암호를 확인하세요.";
                //ViewBag.Message = string.Format("로그인되지 않았습니다. \n 아이디나 암호를 확인하세요.");
                return View();
            }
        }

        /// <summary>
        /// 로그아웃
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> LogOut()
        {
            // 로그아웃
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return LocalRedirect("/");
        }
    }
}
