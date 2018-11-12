﻿using System;
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
            CollectionAssert.AreEqual(albums, actual);
        }
    }
}
