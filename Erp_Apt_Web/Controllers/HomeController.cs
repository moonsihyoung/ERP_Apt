using Erp_Apt_Lib.Logs;
using Erp_Apt_Staff;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Erp_Apt_Web.Controllers
{
    public class HomeController : Controller
    {
        private IStaff_Lib _staff_Lib;
        private IReferral_career_Lib _Career_Lib;
        private IPresent_Lib _Present_Lib;
        private ILogs_Lib _logs_Lib;

        public HomeController(
            IStaff_Lib staff_Lib,
            IReferral_career_Lib referral_Career_Lib,
            IPresent_Lib present_Lib,
            ILogs_Lib logs_Lib)
        {
            _staff_Lib = staff_Lib;
            _Career_Lib = referral_Career_Lib;
            _Present_Lib = present_Lib;
            _logs_Lib = logs_Lib;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string User_Code, string sw_Pass)
        {
            Referral_career_Entity ann = new Referral_career_Entity();
            int in_be = await _staff_Lib.Login(User_Code, sw_Pass);
            int LevelCount = await _staff_Lib.Staff_LevelCount(User_Code);
            if (LevelCount > 1)
            {
                if (in_be > 0)
                {
                    ann = await _Career_Lib.Detail(User_Code);
                    int career = await _Career_Lib.be(User_Code);
                    if (career > 0)
                    {
                        // claims 리스트에 사용자의 인증 정보를 저장함
                        var claims = new List<Claim>()
                        {
                            new Claim("User_Code", User_Code),
                            new Claim(ClaimTypes.Name, ann.User_Name),
                            new Claim("Apt_Code", ann.Apt_Code),
                            new Claim("Apt_Name", ann.Apt_Name),
                            new Claim("LevelCount", LevelCount.ToString())
                        };
                        // 쿠키에 인증 정보를 추가함
                        var ci = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci));
                        HttpContext.Session.SetString("SessionVariable1", "Testing123");

                        await _staff_Lib.VisitCount_Add(User_Code); //방문수 증가

                        Logs_Entites dnn = new Logs_Entites();
                        dnn.Apt_Code = ann.Apt_Code;
                        dnn.Note = ann.User_Name;
                        dnn.Application = "피시 전산 로그인";
                        dnn.LogEvent = "클릭";
                        dnn.Callsite = "";
                        dnn.Exception = "";

                        // HTTP 헤더에서 X-Forwarded-For 값을 가져옴
                        string ipAddress = Request.Headers["X-Forwarded-For"];
                        // 값이 없으면 X-Real-IP 값을 가져옴
                        if (string.IsNullOrEmpty(ipAddress))
                        {
                            dnn.ipAddress = Request.Headers["X-Real-IP"];
                        }
                        // 값이 없으면 RemoteIpAddress 값을 가져옴
                        if (string.IsNullOrEmpty(ipAddress))
                        {
                            dnn.ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                        }
                        else
                        {
                            dnn.ipAddress = "109.120.13.1";
                        }

                        //dnn.ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                        dnn.Level = "3";
                        dnn.Logger = ann.User_ID;
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
            }
            else
            {
                TempData["message"] = "아직 승인되지 않았거나, 퇴사한 직원입니다.";
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
