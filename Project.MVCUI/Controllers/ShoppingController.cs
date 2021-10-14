using PagedList;
using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.MVCUI.VMClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Controllers
{
    public class ShoppingController : Controller
    {

        OrderRepository _oRep;
        ProductRepository _pRep;
        CategoryRepository _cRep;
        OrderDetailRepository _odRep;

        public ShoppingController()
        {
            _oRep = new OrderRepository();
            _odRep = new OrderDetailRepository();
            _pRep = new ProductRepository();
            _cRep = new CategoryRepository();
        }


        // GET: Shopping
        public ActionResult ShoppingList(int? page,int? categoryID) //nullable int vermemizin sebebi aslında buradaki int'in kacıncı sayfada oldugumuz temsil edecek olmasıdır...Ancak birisi direkt alısveriş sayfasına ulastıgında hangi sayfada oldugu verisi olamayacagından dolayı bu şekilde de (yani sayfa numarası gonderilmeden de ) bu action'in calısabilmesini istiyoruz...
        {

            //string a = "Mehmet";

            //string b = a ?? "Çağrı"; // a null ise b'ye Cagrı degerini at. Ama a'nın degeri null degilse b'ye a'yı at...

            //page??1

            //page?? ifadesi page null ise demektir...

            PaginationVM pavm = new PaginationVM
            {
                PagedProducts = categoryID == null ? _pRep.GetActives().ToPagedList(page ?? 1, 9) : _pRep.Where(x => x.CategoryID == categoryID).ToPagedList(page ?? 1, 9),
                Categories = _cRep.GetActives()
            };
            if (categoryID != null) TempData["catID"] = categoryID;

            return View(pavm);
        }
    }
}