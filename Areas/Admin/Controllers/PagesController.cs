using MVC_Store.Models.Data;
using MVC_Store.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Store.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            // Объявляем список для  представления(PageVM)
            List<PageVM> pageList;


            //Инициализируем список(DB)
            //подключение к БД
            //using (Db db = new Db())
            using (Db db = new Db())

            {
                //переменной pageList присваиваються объекты из БД, сначала внеся в массив ToArray(), затем сорти-
                //руя (x => x.Sorting) после этого выбираем Select все объекты и конвертируем массив в лист ToList()
                pageList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }

            // Возвращаем список pageList в представление

            return View(pageList);
        }
        //Создание метода добавления страниц
        // GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }
        // GET: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            //Проверка модели на валидность
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            using (Db db = new Db())
            {

                //Объявляем пересенную для краткого описания (slug)
                string slug;

                //Инициализация класса PageDTO
                PagesDTO dto = new PagesDTO();

                //Присвоение заголовка к модели
                dto.Title = model.Title.ToUpper();

                //Проверяем есть-ли краткое описние, если нет присваеваем
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" " , "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", " -").ToLower();
                }

                //Проверка заголовка и краткого описания на уникальность
                if(db.Pages.Any(x=> x.Title == model.Title))
                {
                    ModelState.AddModelError(" ", "that title already exsist.");
                    return View(model);
                }
                else if (db.Pages.Any(x => x.Slug == model.Slug))
                {
                    ModelState.AddModelError(" ", "that Slug already exsist.");
                    return View(model);
                }
                //Присваеваем значения моделям
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                //dto.Sorting = 100;


                // Сохраняем модель в БД
                db.Pages.Add(dto);
                db.SaveChanges();
            }

            //Передаём сообщение через  TempData
            TempData["SM"] = "Вы добавили новую старницу";

            // Переадресуем пользователя на метод Index
            return RedirectToAction("Index");

        }

    }
}