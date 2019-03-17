using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TranslationTool.Models;
using Google.Cloud.Translation.V2;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using TranslationTool.Service;
using System.Data.Entity;

namespace TranslationTool.Controllers
{
    public class TranslationController : Controller
    {

        TranslationDbContext dbContext = new TranslationDbContext();
        TranslationService service = new TranslationService();

        public ActionResult Index(Translation translation)
        {
            return View(translation);
        }

        
        public JsonResult Translate(string SerbianTranslation)
        {

            if (!Request.IsAjaxRequest())
            {
                return null;
            }

            Translation result = null;
            foreach (Translation translation in dbContext.Translations)
            {
                if (translation.SerbianTranslation.Equals(SerbianTranslation))
                {
                    result = translation;

                    DateTime currentDateAndTime = DateTime.Now;
                    
                    result.LastTimeCalledDateAndTime = currentDateAndTime;

                    
                    break;
                }
            }
            
            if (result != null)
            {
                dbContext.Entry(result).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
            else{
                result = service.WebServiceTranslate(SerbianTranslation);
                if(result == null)
                {
                    return new JsonResult { };
                }
                else
                {
                    dbContext.Translations.Add(result);
                    dbContext.SaveChanges();
                }
            }

            string EnglishTranslation = result.EnglishTranslation;

            return new JsonResult { Data = new { StringValue = EnglishTranslation, EnglishTranslation = EnglishTranslation } };

        }
        
        public ActionResult Final(Translation translation)
        {
            
            return View(translation);
        }

    }
}