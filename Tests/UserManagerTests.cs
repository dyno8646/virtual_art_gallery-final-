using NUnit.Framework;
using Moq;
using Virtual_Art_Gallery.com.hexaware.repo;
using Virtual_Art_Gallery.com.hexaware.dao;
using Virtual_Art_Gallery.com.hexaware.util;
using System.Data.SqlClient;
using Virtual_Art_Gallery.com.hexaware.entity;
using System;

namespace VirtualArtGalleryTests
{
    [TestFixture]
    public class UserManagerTests
    {
        [Test]
        public void Test_User_Registration_and_Deletion()
        {
            List<int> favoriteArtworkId = new List<int> { 1 };
            string c = "lav";
            using (SqlConnection connection = DBConnection.GetConnection()) ;
            VirtualArtGalleryImpl test = new VirtualArtGalleryImpl(c);

            User newUser = new User(17,"newuser", "password","@Email.com", "New","User",new DateTime(2001,11,01),"ProfilePic.jpg", favoriteArtworkId);

            bool registrationSuccess = test.Register(newUser);

            bool t2 = test.DeleteUser(newUser);

            Assert.That(registrationSuccess);
            Assert.That(t2);
        }

        [Test]
        public void Test_Existing_User_Registration_Failure()
        {
            List<int> favoriteArtworkId = new List<int> { 1 };
            string c = "lav";
            using (SqlConnection connection = DBConnection.GetConnection()) ;
            IVirtualArtGallery artGalleryService = new VirtualArtGalleryImpl(c);
            UserProfileManager test = new UserProfileManager(artGalleryService);

            User existingUser = new User(17, "john_doe", "password123", "@Email.com", "New", "User", new DateTime(2001, 11, 01), "ProfilePic.jpg", favoriteArtworkId);

            bool registrationSuccess = test.AddUserProfile(existingUser, existingUser.UserName);
            registrationSuccess = !registrationSuccess;

            Assert.That(registrationSuccess);
        }

        [Test]
        public void Test_Invalid_Password_Registration()
        {
            List<int> favoriteArtworkId = new List<int> { 1 };
            string c = "lav";
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                IVirtualArtGallery artGalleryService = new VirtualArtGalleryImpl(c);
                UserProfileManager test = new UserProfileManager(artGalleryService);
                User userWithInvalidPassword = new User(17, "john_doe", "123", "@Email.com", "New", "User", new DateTime(2001, 11, 01), "ProfilePic.jpg", favoriteArtworkId);

                bool registrationSuccess = test.AddUserProfile(userWithInvalidPassword, userWithInvalidPassword.UserName);

                Assert.That(!registrationSuccess);
            }
        }

        [Test]
        public void Test_Invalid_Email_Registration()
        {
            List<int> favoriteArtworkId = new List<int> { 1 };
            string c = "lav";
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                IVirtualArtGallery artGalleryService = new VirtualArtGalleryImpl(c);
                UserProfileManager test = new UserProfileManager(artGalleryService);

                User userWithInvalidEmail = new User(17, "john_doe", "password123", "invalidemail", "New", "User", new DateTime(2001, 11, 01), "ProfilePic.jpg", favoriteArtworkId);

                bool registrationSuccess = test.AddUserProfile(userWithInvalidEmail, userWithInvalidEmail.UserName);

                Assert.That(!registrationSuccess);
            }
        }

        //[Test]
        //public void Test_User_Login_Success()
        //{
        //    List<int> favoriteArtworkId = new List<int> { 1 };
        //    string c = "lav";
        //    using (SqlConnection connection = DBConnection.GetConnection()) ;
        //    VirtualArtGalleryImpl test = new VirtualArtGalleryImpl(c);

        //    User registeredUser = new User(17, "username", "password", "@Email.com", "New", "User", new DateTime(2001, 11, 01), "ProfilePic.jpg", favoriteArtworkId);

        //    bool loginSuccess = test.Login("username", "password");

        //    Assert.That(loginSuccess, Is.True);
        //}

        //[Test]
        //public void Test_Unregistered_User_Login_Failure()
        //{
        //    string c = "lav";
        //    using (SqlConnection connection = DBConnection.GetConnection()) ;
        //    VirtualArtGalleryImpl test = new VirtualArtGalleryImpl(c);

        //    bool loginSuccess = test.Login("nonexistentuser", "password");

        //    Assert.That(!loginSuccess);
        //}
    }
}

