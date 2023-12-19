using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Virtual_Art_Gallery.com.hexaware.entity;
using Virtual_Art_Gallery.com.hexaware.exception;
using Virtual_Art_Gallery.com.hexaware.util;

namespace Virtual_Art_Gallery.com.hexaware.dao
{
    internal class VirtualArtGalleryImpl : IVirtualArtGallery
    {
        //private readonly string connectionString = @"Server=LAPTOP-NPQJ1I97\SQLEXPRESS01;Database=Virtual_Art_Gallery;Integrated Security=True;TrustServerCertificate=true";
        //private List<User> registeredUsers;
        //private List<Artwork> artworks;
        //private List<Artist> artists;
        //private List<Gallery> galleries;
        private static IVirtualArtGallery artGalleryService = new VirtualArtGalleryImpl(PropertyUtil.GetConnectionString());
        //List<int> favoriteArtworks = new List<int> { 1, 3, 5 };


        private readonly string connectionString;

        public VirtualArtGalleryImpl(string connectionString)
        {
            this.connectionString = connectionString;
        }
        #region
        //private SqlConnection OpenConnection()
        //{
        //    SqlConnection connection = new SqlConnection(connectionString);
        //    connection.Open();
        //    return connection;
        //}
        #endregion


        #region ---> Get Artworks
        public List<Artwork> GetArtworks()
        {
            List<Artwork> artworks = new List<Artwork>();

            using (SqlConnection connection = DBConnection.GetConnection())
            {
                string query = "SELECT Artwork_ID, Title, Description, Creation_Date, Medium, Image_URL, Artist_ID FROM Artwork";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Artwork artwork = new Artwork(
                            artworkID: reader.GetInt32(0),
                            title: reader.GetString(1),
                            description: reader.GetString(2),
                            creationDate: reader.GetDateTime(3),
                            medium: reader.GetString(4),
                            imageURL: reader.GetString(5),
                            artistID:reader.GetInt32(6)
                        );

                        artworks.Add(artwork);
                    }
                }
            }

            return artworks;
        }
        #endregion

        #region ---> Login
        public User Login(string username, string password)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                string query = "SELECT User_ID, UserName, Password, Email, First_Name, Last_Name, Date_Of_Birth, Profile_Picture FROM Users WHERE UserName = @username AND Password = @password";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            User user = new User(
                                userId: reader.GetInt32(0),
                                userName: reader.GetString(1),
                                password: reader.GetString(2),
                                email: reader.GetString(3),
                                firstName: reader.GetString(4),
                                lastName: reader.GetString(5),
                                dateOfBirth: reader.GetDateTime(6),
                                profilePicture: reader.GetString(7),
                                favoriteArtworks: new List<int>());

                            Console.WriteLine($"\nUser {username} logged in successfully.");
                            return user;
                        }
                    }
                }
            }

            throw new UserNotFoundException("Invalid username or password.");
        }
        #endregion

        #region ---> Register
        public bool Register(User user)
        {
            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    int n;
                    int lastUserId = GetLastUserId(connection);
                    user.UserID = lastUserId + 1;

                    string query = "INSERT INTO Users (USER_ID, UserName, Password, Email, First_Name, Last_Name, Date_Of_Birth, Profile_Picture) " +
                                   "VALUES (@userId, @userName, @password, @email, @firstName, @lastName, @dateOfBirth, @profilePicture)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", user.UserID);
                        command.Parameters.AddWithValue("@userName", user.UserName);
                        command.Parameters.AddWithValue("@password", user.Password);
                        command.Parameters.AddWithValue("@email", user.Email);
                        command.Parameters.AddWithValue("@firstName", user.FirstName);
                        command.Parameters.AddWithValue("@lastName", user.LastName);
                        command.Parameters.AddWithValue("@dateOfBirth", user.DateOfBirth);
                        command.Parameters.AddWithValue("@profilePicture", user.ProfilePicture);

                        n = command.ExecuteNonQuery();
                        if (n > 0)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                //Console.WriteLine($"User {user.UserName} registered successfully.");
            }
            catch (DuplicateUsernameException ex)
            {
                Console.WriteLine($"Error during registration: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region ---> Username Exists
        public bool UsernameExists(string username)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                string query = "SELECT COUNT(1) FROM Users WHERE UserName = @userName";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userName", username);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }
        #endregion

        #region --->GetLastUserID
        private int GetLastUserId(SqlConnection connection)
        {
            string query = "SELECT TOP 1 User_ID FROM Users ORDER BY User_ID DESC";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int lastUserId))
                {
                    return lastUserId;
                }
                return 0;
            }
        }
        #endregion

        #region ---> Browse Artworks
        public List<Artwork> BrowseArtworks()
        {
            try
            {
                List<Artwork> artworks = GetArtworks();

                if (artworks.Count > 0)
                {
                    Console.WriteLine("Browsing Artworks:");

                    foreach (var artwork in artworks)
                    {
                        Console.WriteLine($"{artwork.ArtworkID}. {artwork.Title} - {artwork.Description} - ${artwork.Medium}");
                    }
                }
                else
                {
                    Console.WriteLine("No artworks found.");
                }

                return artworks;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during browsing artworks: {ex.Message}");
                return new List<Artwork>();
            }
        }
        #endregion

        #region ---> Search Artists
        public List<Artist> SearchArtists(string keyword)
        {
            List<Artist> matchingArtists = new List<Artist>();

            using (SqlConnection connection = DBConnection.GetConnection())
            {
                string query = $"SELECT Artist_ID, Name, Biography, BirthDate, Nationality, Website, Contact_Information FROM Artist WHERE Name LIKE @keyword";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@keyword", $"%{keyword}%");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Artist artist = new Artist(
                                artistID: reader.GetInt32(0),
                                name: reader.GetString(1),
                                biography: reader.GetString(2),
                                birthDate: reader.GetDateTime(3),
                                nationality: reader.GetString(4),
                                website: reader.GetString(5),
                                contactInformation: reader.GetString(6)
                            );

                            matchingArtists.Add(artist);
                        }
                    }
                }
            }

            return matchingArtists;
        }
        #endregion

        #region ---> View Galleries
        public List<Gallery> ViewGalleries()
        {
            List<Gallery> galleries = new List<Gallery>();

            using (SqlConnection connection = DBConnection.GetConnection())
            {
                string query = "SELECT Gallery_ID, Name, Description, Location, Curator, OpeningHours FROM Gallery";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Gallery gallery = new Gallery(
                            galleryId: reader.GetInt32(0),
                            name: reader.GetString(1),
                            description: reader.GetString(2),
                            location: reader.GetString(3),
                            curatorID: reader.GetInt32(4),
                            openingHours: reader.GetString(5)
                        );

                        galleries.Add(gallery);
                    }
                }
            }

            return galleries;
        }
        #endregion

        #region ---> GetArtworkById
        public Artwork GetArtworkById(int artworkId)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                string query = "SELECT Artwork_ID, Title, Description, Medium, Image_URL, Creation_Date, Artist_ID FROM Artwork WHERE Artwork_ID = @artworkId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@artworkId", artworkId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Artwork(
                                artworkID: reader.GetInt32(0),
                                title: reader.GetString(1),
                                description: reader.GetString(2),
                                creationDate: reader.GetDateTime(5),
                                medium: reader.GetString(3),
                                imageURL: reader.GetString(4),
                                artistID:reader.GetInt32(6)
                                );
                        }
                    }
                }
            }
            throw new ArtWorkNotFoundException($"Artwork with ID {artworkId} not found.");
        }
        #endregion

        #region ---> GetGalleryById
        public Gallery GetGalleryById(int galleryId)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                string query = "SELECT Gallery_ID, Name, Description, Location, Curator, OpeningHours FROM Gallery WHERE Gallery_ID = @galleryId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@galleryId", galleryId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Gallery(
                                galleryId: reader.GetInt32(0),
                                name: reader.GetString(1),
                                description: reader.GetString(2),
                                location: reader.GetString(3),
                                curatorID: reader.GetInt32(4),
                                openingHours: reader.GetString(5)
                            );
                        }
                    }
                }
            }
            throw new GalleryNotFoundException($"Gallery with ID {galleryId} not found.");
        }
        #endregion

        #region ---> GetUserProfile
        public User GetUserProfile(int userId)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                string query = "SELECT User_ID, UserName, Password, Email, First_Name, Last_Name, Date_Of_Birth, Profile_Picture FROM Users WHERE User_ID = @userId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            User userProfile = new User(
                                userId: reader.GetInt32(0),
                                userName: reader.GetString(1),
                                password: reader.GetString(2),
                                email: reader.GetString(3),
                                firstName: reader.GetString(4),
                                lastName: reader.GetString(5),
                                dateOfBirth: reader.GetDateTime(6),
                                profilePicture: reader.GetString(7),
                                favoriteArtworks: new List<int>()
                            );

                            if (userProfile.favoriteArtworks.Count > 0)
                            {
                                Console.WriteLine("Favorite Artworks:");
                                foreach (var artworkId in userProfile.favoriteArtworks)
                                {
                                    Artwork favoriteArtwork = artGalleryService.GetArtworkById(artworkId);

                                    Console.WriteLine($"- ArtworkID: {favoriteArtwork.ArtworkID}");
                                    Console.WriteLine($"  Title: {favoriteArtwork.Title}");
                                    Console.WriteLine($"  Description: {favoriteArtwork.Description}");
                                    Console.WriteLine($"  Medium: {favoriteArtwork.Medium}");
                                }
                            }
                            return userProfile;
                        }
                    }
                }
            }

            throw new UserNotFoundException($"User with ID {userId} not found.");
        }
        #endregion

        #region ---> GetExistingArtistsFromDatabase
        public List<Artist> GetExistingArtistsFromDatabase()
        {
            List<Artist> existingArtists = new List<Artist>();

            using (SqlConnection connection = DBConnection.GetConnection())
            {
                string query = "SELECT * FROM Artist";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int artistID = reader.GetInt32(reader.GetOrdinal("Artist_ID"));
                            string artistName = reader.GetString(reader.GetOrdinal("Name"));

                            Artist artist = new Artist
                            {
                                ArtistID = artistID,
                                Name = artistName
                            };

                            existingArtists.Add(artist);
                        }
                    }
                }
            }

            return existingArtists;
        }
        #endregion

        #region ---> Add Artwork
        public int AddArtwork(Artwork artwork)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                int lastArtworkId = GetLastArtworkId(connection);
                artwork.ArtworkID = lastArtworkId + 1;

                string query = "INSERT INTO Artwork (Artwork_ID,Title, Description, Creation_Date, Medium,Image_URL, Artist_ID) " +
                       "VALUES (@artworkId, @title, @description, @creationdate, @medium, @imageUrl, @artistId);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@artworkId", artwork.ArtworkID);
                    command.Parameters.AddWithValue("@title", artwork.Title);
                    command.Parameters.AddWithValue("@description", artwork.Description);
                    command.Parameters.AddWithValue("@creationdate", artwork.CreationDate);
                    command.Parameters.AddWithValue("@medium", artwork.Medium);
                    command.Parameters.AddWithValue("@imageUrl", artwork.ImageURL);
                    command.Parameters.AddWithValue("@artistId", artwork.ArtistID);
                    command.ExecuteNonQuery();

                    // Use ExecuteScalar to get the newly generated Artwork_ID
                    //int newArtworkId = Convert.ToInt32(command.ExecuteScalar());
                }
                return artwork.ArtworkID;
            }
            
        }
        #endregion

        #region ---> GetLastArtworkId
        private int GetLastArtworkId(SqlConnection connection)
        {
            string query = "SELECT TOP 1 Artwork_ID FROM Artwork ORDER BY Artwork_ID DESC";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                }
            }
            return 0;
        }
        #endregion

        #region ---> Update Artwork
        public bool UpdateArtwork(Artwork artwork)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                string query = "UPDATE Artwork SET Title = @title, Description = @description, Medium = @medium, Image_URL = @imageUrl, Creation_Date = @creationDate WHERE Artwork_ID = @artworkId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@title", artwork.Title);
                    command.Parameters.AddWithValue("@description", artwork.Description);
                    command.Parameters.AddWithValue("@medium", artwork.Medium);
                    command.Parameters.AddWithValue("@imageUrl", artwork.ImageURL);
                    command.Parameters.AddWithValue("@creationDate", artwork.CreationDate);
                    command.Parameters.AddWithValue("@artworkId", artwork.ArtworkID);

                    int n = command.ExecuteNonQuery();
                    if (n > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
        }
        #endregion

        #region ---> DeleteArtwork
        public bool DeleteArtwork(Artwork artwork)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                int n;
                string query = "DELETE FROM Artwork WHERE Artwork_ID = @artworkId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@artworkId", artwork.ArtworkID);

                    n= command.ExecuteNonQuery();
                }
                if (n > 0)
                {
                    return true;
                }
                return false;
            }
        }
        #endregion

        #region ---> Add Artist
        public int AddArtist(Artist artist)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                int lastArtistId = GetLastArtistId(connection);
                artist.ArtistID = lastArtistId + 1;

                string query = "INSERT INTO Artist (Artist_ID, Name, Biography, BirthDate, Nationality, Website, Contact_Information) " +
                               "VALUES (@artistId, @name, @biography, @birthDate, @nationality, @website, @contactInformation)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@artistId", artist.ArtistID);
                    command.Parameters.AddWithValue("@name", artist.Name);
                    command.Parameters.AddWithValue("@biography", artist.Biography);
                    command.Parameters.AddWithValue("@birthDate", artist.BirthDate);
                    command.Parameters.AddWithValue("@nationality", artist.Nationality);
                    command.Parameters.AddWithValue("@website", artist.Website);
                    command.Parameters.AddWithValue("@contactInformation", artist.ContactInformation);

                    command.ExecuteNonQuery();
                }

                return artist.ArtistID;
            }
        }
        #endregion

        #region ---> GetLastArtistId
        private int GetLastArtistId(SqlConnection connection)
        {
            string query = "SELECT TOP 1 Artist_ID FROM Artist ORDER BY Artist_ID DESC";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int lastArtistId))
                {
                    return lastArtistId;
                }
                return 0;
            }
        }
        #endregion

        #region ---> UpdateArtist
        public bool UpdateArtist(Artist artist)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                int n;
                string query = @"UPDATE Artist
                             SET Name = @Name,
                                 Biography = @Biography,
                                 BirthDate = @BirthDate,
                                 Nationality = @Nationality,
                                 Website = @Website,
                                 Contact_Information = @ContactInformation
                             WHERE Artist_ID = @ArtistID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", artist.Name);
                    command.Parameters.AddWithValue("@Biography", artist.Biography);
                    command.Parameters.AddWithValue("@BirthDate", artist.BirthDate);
                    command.Parameters.AddWithValue("@Nationality", artist.Nationality);
                    command.Parameters.AddWithValue("@Website", artist.Website);
                    command.Parameters.AddWithValue("@ContactInformation", artist.ContactInformation);
                    command.Parameters.AddWithValue("@ArtistID", artist.ArtistID);

                    n = command.ExecuteNonQuery();
                }
                if (n > 0)
                {
                    return true;
                }
                return false;
            }
        }
        #endregion

        #region ---> Delete Artist
        public bool DeleteArtist(Artist artist)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                int n;
                string query = "DELETE FROM Artist WHERE Artist_ID = @ArtistID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ArtistID", artist.ArtistID);

                    n = command.ExecuteNonQuery();
                }
                if (n > 0)
                {
                    return true;
                }
                return false;
            }
        }
        #endregion

        #region ---> Add Gallery
        public int AddGallery(Gallery gallery)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                int lastGalleryId = GetLastGalleryId(connection);
                gallery.GalleryID = lastGalleryId + 1;

                string query = "INSERT INTO Gallery (Gallery_ID, Name, Description, Location, Curator, OpeningHours) " +
                               "VALUES (@galleryId, @name, @description, @location, @curator, @openingHours)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@galleryId", gallery.GalleryID);
                    command.Parameters.AddWithValue("@name", gallery.Name);
                    command.Parameters.AddWithValue("@description", gallery.Description);
                    command.Parameters.AddWithValue("@location", gallery.Location);
                    command.Parameters.AddWithValue("@curator", gallery.CuratorID);
                    command.Parameters.AddWithValue("@openingHours", gallery.OpeningHours);

                    command.ExecuteNonQuery();
                }

                return gallery.GalleryID;
            }
        }
        #endregion

        #region ---> GetLastGalleryId
        private int GetLastGalleryId(SqlConnection connection)
        {
            string query = "SELECT TOP 1 Gallery_ID FROM Gallery ORDER BY Gallery_ID DESC";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                }
            }

            return 0;
        }
        #endregion

        #region ---> UpdateGallery
        public bool UpdateGallery(Gallery gallery)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                int n;
                string query = "UPDATE Gallery SET Name = @name, Description = @description, Location = @location, OpeningHours = @openingHours WHERE Gallery_ID = @galleryId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", gallery.Name);
                    command.Parameters.AddWithValue("@description", gallery.Description);
                    command.Parameters.AddWithValue("@location", gallery.Location);
                    command.Parameters.AddWithValue("@openingHours", gallery.OpeningHours);
                    command.Parameters.AddWithValue("@galleryId", gallery.GalleryID);

                    n = command.ExecuteNonQuery();
                }
                if (n > 0)
                {
                    return true;
                }
                return false;
            }
        }
        #endregion

        #region ---> Edit Gallery
        public void EditGallery(Gallery gallery)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                string query = "SELECT Name, Description, Location, OpeningHours FROM Gallery WHERE Gallery_ID = @galleryId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@galleryId", gallery.GalleryID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine("Editing Gallery:");
                            Console.Write("Enter new name (press Enter to keep the current name): ");
                            string newName = Console.ReadLine();
                            gallery.Name = string.IsNullOrEmpty(newName) ? reader.GetString(0) : newName;

                            Console.Write("Enter new description (press Enter to keep the current description): ");
                            string newDescription = Console.ReadLine();
                            gallery.Description = string.IsNullOrEmpty(newDescription) ? reader.GetString(1) : newDescription;

                            Console.Write("Enter new location (press Enter to keep the current location): ");
                            string newLocation = Console.ReadLine();
                            gallery.Location = string.IsNullOrEmpty(newLocation) ? reader.GetString(2) : newLocation;

                            Console.Write("Enter new opening hours (press Enter to keep the current opening hours): ");
                            string newOpeningHours = Console.ReadLine();
                            gallery.OpeningHours = string.IsNullOrEmpty(newOpeningHours) ? reader.GetString(3) : newOpeningHours;

                            UpdateGallery(gallery);

                            Console.WriteLine($"Gallery {gallery.Name} updated successfully.");
                        }
                        else
                        {
                            Console.WriteLine($"Gallery with ID {gallery.GalleryID} not found.");
                        }
                    }
                }
            }
        }
        #endregion

        #region ---> Delete Gallery
        public bool DeleteGallery(Gallery gallery)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                int n;
                string query = "DELETE FROM Gallery WHERE Gallery_ID = @galleryId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@galleryId", gallery.GalleryID);

                    n = command.ExecuteNonQuery();
                }
                if (n > 0)
                {
                    return true;
                }
                return false;
            }
        }
        #endregion

        #region ---> Add User
        public int AddUser(User user)
        {
            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    int lastUserId = GetLastUserId(connection);
                    user.UserID = lastUserId + 1;

                    string query = "INSERT INTO Users (USER_ID, UserName, Password, Email, First_Name, Last_Name, Date_Of_Birth, Profile_Picture) " +
                                   "VALUES (@userId, @userName, @password, @email, @firstName, @lastName, @dateOfBirth, @profilePicture)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", user.UserID);
                        command.Parameters.AddWithValue("@userName", user.UserName);
                        command.Parameters.AddWithValue("@password", user.Password);
                        command.Parameters.AddWithValue("@email", user.Email);
                        command.Parameters.AddWithValue("@firstName", user.FirstName);
                        command.Parameters.AddWithValue("@lastName", user.LastName);
                        command.Parameters.AddWithValue("@dateOfBirth", user.DateOfBirth);
                        command.Parameters.AddWithValue("@profilePicture", user.ProfilePicture);

                        command.ExecuteNonQuery();
                    }
                    return user.UserID;
                }
            }
            catch (DuplicateUsernameException ex)
            {
                Console.WriteLine($"Error during user profile addition: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region ---> UpdateUserProfile
        public void UpdateUserProfile(User user)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                string query = "UPDATE Users SET Username = @Username, Email = @Email, First_Name = @FirstName, Last_Name = @LastName, Date_Of_Birth = @DateOfBirth, Profile_Picture = @ProfilePicture WHERE User_ID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", user.UserID);
                    command.Parameters.AddWithValue("@Username", user.UserName);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                    command.Parameters.AddWithValue("@ProfilePicture", user.ProfilePicture);

                    command.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region ---> Delete User
        public bool DeleteUser(User user)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                int n;
                string query = "DELETE FROM Users WHERE USER_ID = @userId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", user.UserID);
                    n = command.ExecuteNonQuery();
                }
                if (n > 0)
                {
                    return true;
                }
                return false;
            }
        }
        #endregion

        #region ---> LogOut
        public void Logout()
        {
            Console.WriteLine("Logging out.");
        }
        #endregion

        private void CloseConnection(SqlConnection connection)
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}