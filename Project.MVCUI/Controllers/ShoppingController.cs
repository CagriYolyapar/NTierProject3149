using PagedList;
using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.ENTITIES.Models;
using Project.MVCUI.Models.ShoppingTools;
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

        public ActionResult AddToCart(int id)
        {
            Cart c = Session["scart"] == null ? new Cart() : Session["scart"] as Cart;
            Product eklenecekUrun = _pRep.Find(id);

            CartItem ci = new CartItem
            {
                ID = eklenecekUrun.ID,
                Name = eklenecekUrun.ProductName,
                Price = eklenecekUrun.UnitPrice,
                ImagePath = eklenecekUrun.ImagePath
            };
            c.SepeteEkle(ci);
            Session["scart"] = c;
            return RedirectToAction("ShoppingList");
        }

        public ActionResult CartPage()
        {
            if(Session["scart"] != null)
            {
                CartPageVM cpvm = new CartPageVM();
                Cart c = Session["scart"] as Cart;
                cpvm.Cart = c;
                return View(cpvm);
            }

            TempData["bos"] = "Sepetinizde ürün bulunmamatadır";
            return RedirectToAction("ShoppingList");
        }

        public ActionResult DeleteFromCart(int id)
        {
            if (Session["scart"] != null)
            {
                Cart c = Session["scart"] as Cart;
                c.SepettenSil(id);
                if(c.Sepetim.Count == 0)
                {
                    Session.Remove("scart");
                    TempData["sepetBos"] = "Sepetinizde ürün bulunmamaktadır";
                    return RedirectToAction("ShoppingList");
                }
                return RedirectToAction("CartPage");
            }

            return RedirectToAction("ShoppingList");
        }
    }
}