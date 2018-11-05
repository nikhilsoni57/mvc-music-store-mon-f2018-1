using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

// reference Models namespace
using mon_f2018.Models;

namespace mon_f2018.Controllers
{
    public class StoreController : Controller
    {
        // connect to db 
        MusicStoreModel db = new MusicStoreModel();

        // GET: Store
        public ActionResult Index()
        {
            var genres = db.Genres.OrderBy(g => g.Name).ToList();
            return View(genres);
        }

        // GET: Store/Product
        public ActionResult Product(string ProductName)
        {
            ViewBag.ProductName = ProductName;
            return View();
        }

        // GET: Store/Albums/Genre-Name
        public ActionResult Albums(string genre)
        {
            //// mock up some album data
            //var albums = new List<Album>();

            //for (int i = 1; i <= 10; i++)
            //{
            //    albums.Add(new Album { Title = "Album " + i.ToString() });
            //}

            var albums = db.Albums.Where(a => a.Genre.Name == genre).OrderBy(a => a.Title).ToList();
            ViewBag.genre = genre;
            return View(albums);
        }

        // GET: Store/AddToCart/5
        public ActionResult AddToCart(int AlbumId)
        {
            // create new cart item
            GetCartId();
            string CurrentCartId = Session["CartId"].ToString();

            Cart cartItem = new Cart
            {
                AlbumId = AlbumId,
                Count = 1,
                DateCreated = DateTime.Now,
                CartId = CurrentCartId
            };

            // save to db
            db.Carts.Add(cartItem);
            db.SaveChanges();

            // show cart page
            return RedirectToAction("ShoppingCart");
        }

        private void GetCartId()
        {
            // is there already a CartId?
            if (Session["CartId"] == null)
            {
                // is user logged in?
                if (User.Identity.Name == "")
                {
                    // generate unique id that is session-specific
                    Session["CartId"] = Guid.NewGuid().ToString();
                }
                else
                {
                    Session["CartId"] = User.Identity.Name;
                }
            }
        }

        // GET: Store/ShoppingCart
        public ActionResult ShoppingCart()
        {
            // get current cart for display
            GetCartId();
            string CurrentCartId = Session["CartId"].ToString();

            var cartItems = db.Carts.Where(c => c.CartId == CurrentCartId);

            // load view and pass it the list of items in this user's cart
            return View(cartItems);
        }

        // GET: Store/RemoveFromCart/5
        public ActionResult RemoveFromCart(int RecordId)
        {
            // remove item from this user's cart
            Cart cartItem = db.Carts.SingleOrDefault(c => c.RecordId == RecordId);
            db.Carts.Remove(cartItem);
            db.SaveChanges();

            // reload cart page
            return RedirectToAction("ShoppingCart");
        }
    }
}