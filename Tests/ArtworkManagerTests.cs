using NUnit.Framework;
using Moq;
using Virtual_Art_Gallery.com.hexaware.entity;
using Virtual_Art_Gallery.com.hexaware.dao;
using Virtual_Art_Gallery.com.hexaware.util;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using Virtual_Art_Gallery.com.hexaware.repo;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Virtual_Art_Gallery.com.hexaware.exception;

namespace VirtualArtGalleryTests
{
    [TestFixture]
    public class ArtworkManagerTests
    {
        [Test]
        public void Test_To_Add_and_Delete_Artwork()
        {
            string c = "lav";
            using (SqlConnection connection = DBConnection.GetConnection()) ;
            VirtualArtGalleryImpl test = new VirtualArtGalleryImpl(c);

            Artwork artwork = new Artwork(100, "Title", "Description", new DateTime(2023, 12, 19), "Medium", "ImageUrl", 1);

            int t1 = test.AddArtwork(artwork);
            bool t = false;
            if (t1 == 12)
            {
                t = true;
            }

            bool t2 = test.DeleteArtwork(artwork);

            Assert.That(t);
            Assert.That(t2);
        }

        [Test]
        public void Test_To_Update_Artwork()
        {
            string c = "lav";
            using (SqlConnection connection = DBConnection.GetConnection()) ;
            VirtualArtGalleryImpl test = new VirtualArtGalleryImpl(c);

            Artwork artwork = new Artwork(1, "Enchanted Garden", "A mesmerizing portrayal of a magical garden", new DateTime(2023,01,01), "Oil on Canvas", "image_url_1.jpg", 1) ;

            bool p1 = test.UpdateArtwork(artwork);

            Assert.That(p1);
        }

        [Test]
        public void Test_To_Search_Artwork()
        {
            Artwork a= null;

            string c = "lav";
            using (SqlConnection connection = DBConnection.GetConnection()) ;
            VirtualArtGalleryImpl test = new VirtualArtGalleryImpl(c);

            a = test.GetArtworkById(1);
            bool p = (a != null);

            Assert.That(p);

        }

        [Test]
        public void Test_Get_Non_Existent_Artwork()
        {
            string c = "lav";
            using (SqlConnection connection = DBConnection.GetConnection()) ;
            VirtualArtGalleryImpl test = new VirtualArtGalleryImpl(c);

            int nonExistentArtworkId = 9999;
            Assert.Throws<ArtWorkNotFoundException>(() => test.GetArtworkById(nonExistentArtworkId));
        }



    }
}
