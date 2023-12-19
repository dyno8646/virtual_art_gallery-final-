using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Art_Gallery.com.hexaware.entity
{
    public class User
    {
        private int userID;
        private string username;
        private string password;
        private string email;
        private string firstName;
        private string lastName;
        private DateTime dateOfBirth;
        private string profilePicture;
        public List<int> favoriteArtworks { get; set; }

        public User() { }
        public User(int userId, string userName, string password, string email, string firstName, string lastName, DateTime dateOfBirth, string profilePicture, List<int> favoriteArtworks)
        {
            UserID = userId;
            UserName = userName;
            Password = password;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            ProfilePicture = profilePicture;
            this.favoriteArtworks = favoriteArtworks;
        }
        #region ---> Getters Setters
        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        public string UserName
        {
            get { return username; }
            set { username = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; }
        }
        public string ProfilePicture
        {
            get { return profilePicture; }
            set { profilePicture = value; }
        }
        #endregion

        public override string ToString()
        {
            return $"UserID :{UserID}\nUserName :{UserName}\nPassword :{Password}\nEmail{Email}\nFirstName :{FirstName}\nLastName :{LastName}\nDateOfBirth :{DateOfBirth}\nProfilePicture :{ProfilePicture}";
        }
    }
}