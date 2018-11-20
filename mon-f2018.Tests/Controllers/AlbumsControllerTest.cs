using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

// add reference to web project controllers
using mon_f2018.Controllers;
using mon_f2018.Models;
using System.Collections.Generic;
using System.Linq;

namespace mon_f2018.Tests.Controllers
{
    [TestClass]
    public class AlbumsControllerTest
    {
        Mock<IAlbumsMock> mock;
        List<Album> albums;
        AlbumsController controller;

        [TestInitialize]
        public void TestInitialize()
        {
            // arrange mock data for all unit tests
            mock = new Mock<IAlbumsMock>();

            albums = new List<Album>
            {
                new Album { AlbumId = 100, Title = "One Hundred", Price = 6.99m, Artist = new Artist {
                    ArtistId = 4000, Name = "Some One" }
                },
                new Album { AlbumId = 200, Title = "Two Hundred", Price = 7.99m, Artist = new Artist {
                    ArtistId = 4001, Name = "Another Person" }
                },
                new Album { AlbumId = 300, Title = "Three Hundred", Price = 8.99m, Artist = new Artist {
                    ArtistId = 4002, Name = "Third Artist" }
                }
            };

            // populate interface from mock data
            mock.Setup(m => m.Albums).Returns(albums.AsQueryable());

            controller = new AlbumsController(mock.Object);
        }

        // GET: Albums
        #region
        [TestMethod]
        public void IndexReturnsView()
        {        
            // act
            ViewResult result = controller.Index() as ViewResult;

            // assert
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void IndexReturnsAlbums()
        {
            // act - does the viewresults Model equal a list of albums?
            var actual = (List<Album>)((ViewResult) controller.Index()).Model;

            // assert
            CollectionAssert.AreEqual(albums.OrderBy(a => a.Artist.Name).ThenBy(a => a.Title).ToList(), actual);
        }
        #endregion

        // GET: Albums/Details/100
        #region
        [TestMethod]
        public void DetailsNoId()
        {
            // act
            var result = (ViewResult)controller.Details(null);

            // assert
            Assert.AreEqual("Error", result.ViewName);
        }

        [TestMethod]
        public void DetailsInvalidId()
        {
            // act
            var result = (ViewResult)controller.Details(67830);

            // assert
            Assert.AreEqual("Error", result.ViewName);
        }

        [TestMethod]
        public void DetailsValidId()
        {
            // act - cast the model as an Album object
            Album actual = (Album)((ViewResult)controller.Details(300)).Model;

            // assert - is this the first mock album in our array?
            Assert.AreEqual(albums[2], actual);
        }

        [TestMethod]
        public void DetailsViewLoads()
        {
            // act
            ViewResult result = (ViewResult)controller.Details(300);

            // assert
            Assert.AreEqual("Details", result.ViewName);
        }
        #endregion

        // GET: Albums/Edit/5
        #region
        [TestMethod]
        public void EditNoId()
        {
            // arrange
            int? id = null;

            // act 
            var result = (ViewResult)controller.Edit(id);

            // assert
            Assert.AreEqual("Error", result.ViewName);
        }

        [TestMethod]
        public void EditInvalidId()
        {
            // act
            var result = (ViewResult)controller.Edit(8983);

            // assert
            Assert.AreEqual("Error", result.ViewName);
        }

        [TestMethod]
        public void EditViewBagArtist()
        {
            // act
            ViewResult actual = (ViewResult)controller.Edit(100);

            // assert
            Assert.IsNotNull(actual.ViewBag.ArtistId);
        }

        [TestMethod]
        public void EditViewBagGenre()
        {
            // act
            ViewResult actual = (ViewResult)controller.Edit(100);

            // assert
            Assert.IsNotNull(actual.ViewBag.GenreId);
        }

        [TestMethod]
        public void EditViewLoads()
        {
            // act
            ViewResult actual = (ViewResult)controller.Edit(100);

            // assert
            Assert.AreEqual("Edit", actual.ViewName);
        }

        [TestMethod]
        public void EditLoadsAlbum()
        {
            // act
            Album actual = (Album)((ViewResult)controller.Edit(100)).Model;

            // assert
            Assert.AreEqual(albums[0], actual);
        }
        #endregion

        // GET: Albums/Create
        #region

        [TestMethod]
        public void CreateViewLoads() {
            // act
            var result = (ViewResult)controller.Create();

            // assert
            Assert.AreEqual("Create", result.ViewName);
        }

        [TestMethod]
        public void CreateViewBagArtist()
        {
            // act
            ViewResult result = (ViewResult)controller.Create();

            // assert
            Assert.IsNotNull(result.ViewBag.ArtistId);
        }

        [TestMethod]
        public void CreateViewBagGenre()
        {
            // act
            ViewResult result = (ViewResult)controller.Create();

            // assert
            Assert.IsNotNull(result.ViewBag.GenreId);
        }

        #endregion

        // GET: Albums/Delete
        #region

        [TestMethod]
        public void DeleteNoId()
        {
            // act
            var result = (ViewResult)controller.Delete(null);

            // assert
            Assert.AreEqual("Error", result.ViewName);
        }

        [TestMethod]
        public void DeleteInvalidId()
        {
            // act
            var result = (ViewResult)controller.Delete(3739);

            // assert
            Assert.AreEqual("Error", result.ViewName);
        }

        [TestMethod]
        public void DeleteValidIdLoadsView()
        {
            // act
            var result = (ViewResult)controller.Delete(100);

            // assert
            Assert.AreEqual("Delete", result.ViewName);
        }

        [TestMethod]
        public void DeleteValidIdLoadsAlbum()
        {
            // act
            Album result = (Album)((ViewResult)controller.Delete(100)).Model;

            // assert
            Assert.AreEqual(albums[0], result);
        }

        #endregion

        // POST: Albums/Edit
        #region

        [TestMethod]
        public void EditPostLoadsIndex()
        {
            // act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Edit(albums[0]);

            // assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void EditPostViewBagArtist()
        {
            // arrange
            Album invalid = new Album { AlbumId = 27 };
            controller.ModelState.AddModelError("Error", "Won't Save");

            // act
            ViewResult result = (ViewResult)controller.Edit(invalid);

            // assert
            Assert.IsNotNull(result.ViewBag.ArtistId);
        }

        [TestMethod]
        public void EditPostViewBagGenre()
        {
            // arrange
            Album invalid = new Album { AlbumId = 27 };
            controller.ModelState.AddModelError("Error", "Won't Save");

            // act
            ViewResult result = (ViewResult)controller.Edit(invalid);

            // assert
            Assert.IsNotNull(result.ViewBag.GenreId);
        }
        
        [TestMethod]
        public void EditPostInvalidLoadsView()
        {
            // arrange
            Album invalid = new Album { AlbumId = 27 };
            controller.ModelState.AddModelError("Error", "Won't Save");

            // act
            ViewResult result = (ViewResult)controller.Edit(invalid);

            // assert
            Assert.AreEqual("Edit", result.ViewName);
        }

        [TestMethod]
        public void EditPostInvalidLoadsAlbum()
        {
            // arrange
            Album invalid = new Album { AlbumId = 100 };
            controller.ModelState.AddModelError("Error", "Won't Save");

            // act
            Album result = (Album)((ViewResult)controller.Edit(invalid)).Model;

            // assert
            Assert.AreEqual(invalid, result);
        }

        #endregion

        // POST: Albums/Create
        #region
        [TestMethod]
        public void CreateValidAlbum()
        {
            // arrange
            Album newAlbum = new Album
            {
                AlbumId = 400,
                Title = "Four Hundred",
                Price = 9.99m,
                Artist = new Artist
                {
                    ArtistId = 4004,
                    Name = "Some Four"
                }
            };

            // act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Create(newAlbum);

            // assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void CreateInvalidAlbum()
        {
            // arrange
            Album invalid = new Album();

            // act
            controller.ModelState.AddModelError("Cannot create", "create exception");
            ViewResult result = (ViewResult)controller.Create(invalid);

            // assert
            Assert.AreEqual("Create", result.ViewName);
        }

        [TestMethod]
        public void CreateInvalidViewBagArtist()
        {
            // arrange
            Album invalid = new Album();

            // act
            controller.ModelState.AddModelError("Cannot create", "create exception");
            ViewResult result = (ViewResult)controller.Create(invalid);

            // assert
            Assert.IsNotNull(result.ViewBag.ArtistId);
        }

        [TestMethod]
        public void CreateInvalidViewBagGenre()
        {
            // arrange
            Album invalid = new Album();

            // act
            controller.ModelState.AddModelError("Cannot create", "create exception");
            ViewResult result = (ViewResult)controller.Create(invalid);

            // assert
            Assert.IsNotNull(result.ViewBag.GenreId);
        }
        #endregion

        // POST: Albums/DeleteConfirmed/100
        #region
        [TestMethod]
        public void DeleteConfirmedNoId()
        {
            // act
            ViewResult result = (ViewResult)controller.DeleteConfirmed(null);

            // assert
            Assert.AreEqual("Error", result.ViewName);
        }

        [TestMethod]
        public void DeleteConfirmedInvalidId()
        {
            // act
            ViewResult result = (ViewResult)controller.DeleteConfirmed(3972);

            // assert
            Assert.AreEqual("Error", result.ViewName);
        }

        [TestMethod]
        public void DeleteConfirmedValidId()
        {
            // act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.DeleteConfirmed(100);

            // assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
        #endregion
    }
}
