using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Virtual_Art_Gallery.com.hexaware.dao;
using Virtual_Art_Gallery.com.hexaware.entity;
using Virtual_Art_Gallery.com.hexaware.util;
using System.Data.SqlClient;
using Virtual_Art_Gallery.com.hexaware.exception;

namespace Virtual_Art_Gallery_Testing
{
    internal class GalleryManagerTests
    {
        [Test]
        public void Test_Create_Gallery()
        {
            string c = "lav";
            using (SqlConnection connection = DBConnection.GetConnection()) ;
            VirtualArtGalleryImpl test = new VirtualArtGalleryImpl(c);

            Gallery gallery = new Gallery(100, "ArtHub Gallery", "A vibrant space for contemporary art", "Downtown City", 4, "Monday-Friday: 10 AM - 6 PM, Saturday: 12 PM - 4 PM");

            int galleryId = test.AddGallery(gallery);
            bool t2 = test.DeleteGallery(gallery);
            
            Assert.That(galleryId > 0);
            Assert.That(t2);
        }

        [Test]
        public void Test_Search_Galleries()
        {
            Gallery a = null;
            List<Gallery> galleries = null;

            string c = "lav";
            using (SqlConnection connection = DBConnection.GetConnection()) ;
            VirtualArtGalleryImpl test = new VirtualArtGalleryImpl(c);


            a = test.GetGalleryById(1);
            bool p = (a != null);

            Assert.That(p);
        }

        [Test]
        public void Test_Update_Gallery()
        {
            string c = "lav";
            using (SqlConnection connection = DBConnection.GetConnection()) ;
            VirtualArtGalleryImpl test = new VirtualArtGalleryImpl(c);

            Gallery gallery = new Gallery(1, "ArtHub Gallery", "A vibrant space for contemporary art", "Downtown City", 4, "Monday-Friday: 10 AM - 6 PM, Saturday: 12 PM - 4 PM");

            bool updateSuccess = test.UpdateGallery(gallery);

            Assert.That(updateSuccess);
        }

        [Test]
        public void Test_Get_Non_Existent_Gallery()
        {
            string c = "lav";
            using (SqlConnection connection = DBConnection.GetConnection()) ;
            VirtualArtGalleryImpl test = new VirtualArtGalleryImpl(c);

            int nonExistentGalleryId = 9999;

            Assert.Throws<GalleryNotFoundException>(() => test.GetGalleryById(nonExistentGalleryId));
        }

    }
}
