using ImportPersonDataLib;
using ImportPersonDataLib.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace WebWorkWithImportPerson.Controllers
{
    public class ImportPersonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ImportExcel()
        {
            return View();
        }

        public IActionResult Import()
        {            
            var appSettingsJson = AppSettingsJson.GetAppSettings();
            string connectionString = appSettingsJson["ConnectionStrings:DefaultConnection"];
            string sql = appSettingsJson["SqlQuery"];

            IImportPersonData importPerson = new ImportPersonDataFromDb(sql, connectionString);

            var result = importPerson.Import();
            if (!result)
            {
                ViewBag.Text = "При импорте данных произошла ошибка.";
                ViewBag.Text2 = "Информация в базе данных в поле \"ErrorMessage\" и в каталоге \"Документы\\logfile.txt\".";
            }
            else
            {
                ViewBag.Text = "Импорт данных прошел успешно.";
                ViewBag.Text2 = string.Empty;
            }
                        
            return View();
        }
    }
}
