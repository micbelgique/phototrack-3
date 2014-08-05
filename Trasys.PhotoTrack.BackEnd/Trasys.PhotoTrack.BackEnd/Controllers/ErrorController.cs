using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Trasys.PhotoTrack.BackEnd.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index(int errorCode)
        {
            ViewBag.ErrorCode = errorCode;
            switch (errorCode)
            {
                case 400:
                    ViewBag.ErrorMessage = "Bad Request";
                    break;
                case 401:
                    ViewBag.ErrorMessage = "Unauthorized";
                    break;
                case 402:
                    ViewBag.ErrorMessage = "Payment Required";
                    break;
                case 403:
                    ViewBag.ErrorMessage = "Forbidden";
                    break;
                case 404:
                    ViewBag.ErrorMessage = "Not found";
                    break;
                case 405:
                    ViewBag.ErrorMessage = "Method Not Allowed";
                    break;  
                case 406:
                    ViewBag.ErrorMessage = "Not Acceptable";
                    break; 
                case 407:
                    ViewBag.ErrorMessage = "Bad Request";
                    break;
                case 408:
                    ViewBag.ErrorMessage = "Unauthorized";
                    break;
                case 409:
                    ViewBag.ErrorMessage = "Payment Required";
                    break;
                case 410:
                    ViewBag.ErrorMessage = "Gone";
                    break;
                case 411:
                    ViewBag.ErrorMessage = "Length Required";
                    break;
                case 500:
                    ViewBag.ErrorMessage = "Internal Server Error";
                    break;  
                case 501:
                    ViewBag.ErrorMessage = "Not Implemented";
                    break;
                case 502:
                    ViewBag.ErrorMessage = "Internal Server Error";
                    break;
                case 503:
                    ViewBag.ErrorMessage = "Service Unavailable";
                    break;
                case 504:
                    ViewBag.ErrorMessage = "Gateway Timeout";
                    break;
                case 505:
                    ViewBag.ErrorMessage = "HTTP Version Not Supported";
                    break;
              
                default:
                    ViewBag.ErrorMessage = "Unknow error"; 
                    break;
            }
            return View();
        }
    }
}