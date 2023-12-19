using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Virtual_Art_Gallery.com.hexaware.dao;
using Virtual_Art_Gallery.com.hexaware.entity;
using Virtual_Art_Gallery.com.hexaware.exception;
using Virtual_Art_Gallery.com.hexaware.util;

namespace Virtual_Art_Gallery.com.hexaware.repo
{
    public class UserProfileManager
    {
        private readonly IVirtualArtGallery artGalleryService;
        public UserProfileManager(IVirtualArtGallery artGalleryService)
        {
            this.artGalleryService = artGalleryService;
        }

        #region ---> Manage User Profiles
        public void ManageUserProfiles(User loggedInUser)
        {
            Console.WriteLine("\nChoose an action:\n");
            Console.WriteLine("1. Add User");
            Console.WriteLine("2. Edit User");
            Console.WriteLine("3. Delete User");
            Console.WriteLine("4. Go back to the main menu\n");
            Console.Write("Enter your choice: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        Console.Write("Enter username: ");
                        string username = Console.ReadLine();
                        AddUserProfile(loggedInUser,username);
                        break;
                    case 2:
                        EditUserProfile(loggedInUser);
                        break;
                    case 3:
                        DeleteUserProfile(loggedInUser);
                        break;
                    case 4:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 3.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
            Console.WriteLine();
        }
        #endregion

        #region ---> Add User Profile
        public bool AddUserProfile(User loggedInUser,string username)
        {
            Console.WriteLine("Adding a new user profile.");
            VirtualArtGalleryImpl galleryService = new VirtualArtGalleryImpl(PropertyUtil.GetConnectionString());


            //Console.Write("Enter username: ");
            //string username = Console.ReadLine();
            if (galleryService.UsernameExists(username))
            {
                Console.WriteLine("Username already taken. Please enter a different username.");
                return false;
            }
            else
            {
                Console.Write("Enter password: ");
                string password = Console.ReadLine();
                if (password.Length <= 5)
                {
                    Console.WriteLine("Password length should be greater than 5.");
                    return false;
                }
                else
                {
                    Console.Write("Enter email: ");
                    string email = Console.ReadLine();
                    if (!email.EndsWith("@email.com"))
                    {
                        Console.WriteLine("Invalid email format. Email must end with '@email.com'.");
                        return false;
                    }
                    else
                    {
                        Console.Write("Enter first name: ");
                        string firstName = Console.ReadLine();

                        Console.Write("Enter last name: ");
                        string lastName = Console.ReadLine();

                        Console.Write("Enter date of birth (YYYY-MM-DD): ");
                        string dobString = Console.ReadLine();

                        Console.Write("Enter profile picture name: ");
                        string profilePicture = Console.ReadLine();

                        Console.Write("Enter favorite: ");
                        string favorite = Console.ReadLine();

                        try
                        {
                            if (DateTime.TryParse(dobString, out DateTime dateOfBirth))
                            {
                                int n;
                                User newUserProfile = new User
                                {
                                    UserID = -1,
                                    UserName = username,
                                    Password = password,
                                    Email = email,
                                    FirstName = firstName,
                                    LastName = lastName,
                                    DateOfBirth = dateOfBirth,
                                    ProfilePicture = profilePicture,
                                    favoriteArtworks = new List<int>()
                                };
                                artGalleryService.AddUser(newUserProfile);
                                Console.WriteLine($"\nUser profile added successfully for {newUserProfile.UserName}.");
                                return true;
                            }
                            
                            else
                            {
                                Console.WriteLine("Invalid date format. Date of birth must be in YYYY-MM-DD format.");
                                return false;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error adding user profile: {ex.Message}");
                            return false;
                        }
                    }
                }
            }
        }
        #endregion

        #region ---> Edit user Profile
        public void EditUserProfile(User loggedInUser)
        {
            Console.WriteLine("Editing user profile:");

            Console.WriteLine($"Current User Profile for {loggedInUser.UserName}:");
            Console.WriteLine($"1. Username: {loggedInUser.UserName}");
            Console.WriteLine($"2. Email: {loggedInUser.Email}");
            Console.WriteLine($"3. First Name: {loggedInUser.FirstName}");
            Console.WriteLine($"4. Last Name: {loggedInUser.LastName}");
            Console.WriteLine($"5. Date of Birth: {loggedInUser.DateOfBirth.ToShortDateString()}");
            Console.WriteLine($"6. Profile Picture: {loggedInUser.ProfilePicture}");

            Console.Write("Select the number to edit (or enter 0 to exit): ");
            if (int.TryParse(Console.ReadLine(), out int selectedOption))
            {
                switch (selectedOption)
                {
                    case 1:
                        Console.Write("Enter new username: ");
                        loggedInUser.UserName = Console.ReadLine();
                        break;

                    case 2:
                        Console.Write("Enter new email: ");
                        loggedInUser.Email = Console.ReadLine();
                        break;

                    case 3:
                        Console.Write("Enter new first name: ");
                        loggedInUser.FirstName = Console.ReadLine();
                        break;

                    case 4:
                        Console.Write("Enter new last name: ");
                        loggedInUser.LastName = Console.ReadLine();
                        break;

                    case 5:
                        Console.Write("Enter new date of birth (yyyy-MM-dd): ");
                        if (DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newDateOfBirth))
                        {
                            loggedInUser.DateOfBirth = newDateOfBirth;
                        }
                        else
                        {
                            Console.WriteLine("Invalid date format. User profile not updated.");
                        }
                        break;

                    case 6:
                        Console.Write("Enter new profile picture: ");
                        loggedInUser.ProfilePicture = Console.ReadLine();
                        break;

                    case 0:
                        Console.WriteLine("Exiting user profile editor.");
                        return;

                    default:
                        Console.WriteLine("Invalid option. User profile not updated.");
                        return;
                }

                try
                {
                    // Call the service method to update the user profile in the database
                    artGalleryService.UpdateUserProfile(loggedInUser);

                    Console.WriteLine("User profile updated successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating user profile: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
        }
        #endregion

        #region ---> Delete User Profile
        public void DeleteUserProfile(User loggedInUser)
        {
            try
            {
                // Call the service method to delete the user profile from the database
                artGalleryService.DeleteUser(loggedInUser);
                Console.WriteLine($"User profile deleted successfully for {loggedInUser.UserName}.");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user profile: {ex.Message}");
            }
        }
        #endregion
    }
}
