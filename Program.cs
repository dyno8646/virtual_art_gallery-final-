using Virtual_Art_Gallery.com.hexaware.dao;
using Virtual_Art_Gallery.com.hexaware.entity;
using Virtual_Art_Gallery.com.hexaware.exception;
using Virtual_Art_Gallery.com.hexaware.repo;
using Virtual_Art_Gallery.com.hexaware.util;
internal class Program
{
    enum MenuOptions
    {
        Login = 1,
        Register,
        Exit,
        BrowseArtworks,
        SearchArtists,
        ViewGalleries,
        UserProfile,

        // New CRUD options
        ArtworkManagerOptions,
        ArtistManagerOptions,
        GalleryManagerOptions,
        UserProfileManagerOptions,
        Logout
    }
    private static IVirtualArtGallery artGalleryService = new VirtualArtGalleryImpl(PropertyUtil.GetConnectionString());

    private static User loggedInUser = null;
    private static void Main(string[] args)
    {
        Console.WriteLine("\n\n------------------------------   V I R T U A L   A R T   G A L L E R Y   ---------------------------------\n\n");
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.White; // Set default text color

            if (loggedInUser == null)
            {
                Console.WriteLine("1. " + ColorizeText("Login", ConsoleColor.Cyan));
                Console.WriteLine("2. " + ColorizeText("Register", ConsoleColor.Green));
                Console.WriteLine("3. " + ColorizeText("Exit", ConsoleColor.Red));
            }
            else
            {
                Console.WriteLine("\nMain Menu\n");
                Console.WriteLine("4. " + ColorizeText("Browse Artworks", ConsoleColor.Green));
                Console.WriteLine("5. " + ColorizeText("Search Artists", ConsoleColor.Yellow));
                Console.WriteLine("6. " + ColorizeText("View Galleries", ConsoleColor.Blue));
                Console.WriteLine("7. " + ColorizeText("User Profile", ConsoleColor.Cyan));

                // CRUD Options
                Console.WriteLine("8. " + ColorizeText("Manage Artworks", ConsoleColor.Green));
                Console.WriteLine("9. " + ColorizeText("Manage Artists", ConsoleColor.Yellow));
                Console.WriteLine("10. " + ColorizeText("Manage Galleries", ConsoleColor.Blue));
                Console.WriteLine("11. " + ColorizeText("Manage User Profile", ConsoleColor.DarkCyan));

                Console.WriteLine("\n12. " + ColorizeText("Logout", ConsoleColor.DarkGreen));
            }

            Console.Write("\nSelect an option: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch ((MenuOptions)choice)
                {
                    case MenuOptions.Login:
                        Login();
                        break;

                    case MenuOptions.Register:
                        Register();
                        break;

                    case MenuOptions.Exit:
                        Console.WriteLine("\nExiting the application. Goodbye!");
                        Environment.Exit(0);
                        break;

                    case MenuOptions.BrowseArtworks:
                        BrowseArtworks();
                        break;

                    case MenuOptions.SearchArtists:
                        SearchArtists();
                        break;

                    case MenuOptions.ViewGalleries:
                        ViewGalleries();
                        break;

                    case MenuOptions.UserProfile:
                        UserProfile();
                        break;

                    // CRUD Cases
                    case MenuOptions.ArtworkManagerOptions:
                        ArtworkManagerOptions();
                        break;

                    case MenuOptions.ArtistManagerOptions:
                        ArtistManagerOptions();
                        break;

                    case MenuOptions.GalleryManagerOptions:
                        GalleryManagerOptions();
                        break;

                    case MenuOptions.UserProfileManagerOptions:
                        UserProfileManagerOptions();
                        break;

                    case MenuOptions.Logout:
                        Console.WriteLine("\nLogging out.");
                        loggedInUser = null;
                        Console.WriteLine("You have been logged out.");
                        break;

                    default:
                        Console.WriteLine(ColorizeText("Invalid option. Please try again.", ConsoleColor.DarkYellow));
                        break;
                }
            }
            else
            {
                Console.WriteLine(ColorizeText("Invalid input. Please enter a number.", ConsoleColor.White));
            }

            Console.WriteLine();
        }
    }
    #region ---> Colorize Text
    private static string ColorizeText(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        return text;
    }
    #endregion

    #region ---> Login
    private static void Login()
    {
        Console.Write("\nEnter username: ");
        string username = Console.ReadLine();

        Console.Write("\nEnter password: ");
        string password = Console.ReadLine();

        try
        {
            loggedInUser = artGalleryService.Login(username, password);
            Console.WriteLine($"\nWelcome, {loggedInUser.UserName}!");
        }
        catch (UserNotFoundException)
        {
            Console.WriteLine("Invalid username or password.\n\n");
            Console.WriteLine("1. Try again?\n");
            Console.WriteLine("2. Do you want to register? ");
            

            Console.Write("\nSelect an option: \n");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        break;
                    case 2:
                        Register();
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
    #endregion

    #region ---> Register
    private static void Register()
    {
        Console.WriteLine("Registering a new user.");
        VirtualArtGalleryImpl galleryService = new VirtualArtGalleryImpl(PropertyUtil.GetConnectionString());

        Console.Write("Enter username: ");
        string username = Console.ReadLine();

        if (galleryService.UsernameExists(username))
        {
            Console.WriteLine("Username already taken. Please enter a different username.");
        }
        else
        {
            Console.Write("Enter password: ");
            string password = Console.ReadLine();
            if (password.Length <= 5)
            {
                Console.WriteLine("Password length should be greater than 5.");
            }
            else
            {
                Console.Write("Enter email: ");
                string email = Console.ReadLine();
                if (!email.EndsWith("@email.com"))
                {
                    Console.WriteLine("Invalid email format. Email must end with '.email'.");
                }
                else
                {
                    Console.Write("Enter first name: ");
                    string firstname = Console.ReadLine();

                    Console.Write("Enter last name: ");
                    string lastname = Console.ReadLine();

                    Console.Write("Enter date of birth (YYYY-MM-DD): ");
                    string dobString = Console.ReadLine();

                    Console.Write("Enter pic name: ");
                    string pic = Console.ReadLine();

                    Console.Write("Enter fav: ");
                    string fav = Console.ReadLine();

                    try
                    {
                        if (DateTime.TryParse(dobString, out DateTime dob))
                        {
                            User newUser = new User(
                                userId: -1,
                                userName: username,
                                password: password,
                                email: email,
                                firstName: firstname,
                                lastName: lastname,
                                dateOfBirth: dob,
                                profilePicture: pic,
                                favoriteArtworks: new List<int>());

                            artGalleryService.Register(newUser);
                            Console.WriteLine("Registration successful!");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error during registration: {ex.Message}");
                    }
                }
            }
        }
    }
    #endregion

    #region ---> Browse Artworks
    private static void BrowseArtworks()
    {
        try
        {
            List<Artwork> artworks = artGalleryService.GetArtworks();

            if (artworks.Any())
            {
                Console.WriteLine("Browsing Artworks:");
                Console.ForegroundColor = ConsoleColor.Cyan;

                // Display table headers
                Console.WriteLine(ColorizeText("ArtworkID\tTitle\t\t\t\t\tDescription\t\t\t\tMedium", ConsoleColor.Green));
                Console.WriteLine("-----------------------------------------------------------------------------------------------------------");

                foreach (var artwork in artworks)
                {
                    // Display artwork details in a tabular format
                    Console.WriteLine($"{artwork.ArtworkID,-10}\t{artwork.Title,-25}\t{artwork.Description,-40}\t{artwork.Medium}");
                }
                Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine("No artworks found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during browsing artworks: {ex.Message}");
        }
    }
    #endregion

    #region ---> Search Artists
    private static void SearchArtists()
    {
        Console.Write("Enter the keyword to search for artists: ");
        string keyword = Console.ReadLine();
        List<Artist> matchingArtists = artGalleryService.SearchArtists(keyword);

        if (matchingArtists.Count > 0)
        {
            Console.WriteLine($"Artists matching '{keyword}':");

            // In the loop where artists are displayed
            foreach (var artist in matchingArtists)
            {
                Console.WriteLine($"\n-------------------------------------------------------------");
                Console.WriteLine($"| ID: {artist.ArtistID,-5} | Name: {artist.Name,-20} |");
                Console.WriteLine($"| Biography: {artist.Biography,-45} |");
                Console.WriteLine($"| Nationality: {artist.Nationality,-15} |");
                Console.WriteLine($"| Website: {artist.Website,-40} |");
                Console.WriteLine($"| Contact Information: {artist.ContactInformation,-30} |");
                Console.WriteLine($"-------------------------------------------------------------\n");
            }
        }
        else
        {
            throw new ArtistNotFoundException($"No artists found matching '{keyword}'.");
        }
    }
    #endregion

    #region ---> View Galleries
    private static void ViewGalleries()
    {
        try
        {
            Console.ForegroundColor = ConsoleColor.White;
            List<Gallery> galleries = artGalleryService.ViewGalleries();

            if (galleries.Count > 0)
            {
                Console.WriteLine("\nAvailable Galleries:\n");
                Console.WriteLine("-------------------------------------------------------------");
                Console.WriteLine("| Gallery ID | Gallery Name                                |");
                Console.WriteLine("-------------------------------------------------------------");

                foreach (var gallery in galleries)
                {
                    Console.WriteLine($"| {gallery.GalleryID,-11} | {gallery.Name,-42} |");
                }

                Console.WriteLine("-------------------------------------------------------------");


                Console.Write("Enter the ID of the gallery to view details: ");
                if (int.TryParse(Console.ReadLine(), out int selectedGalleryId))
                {
                    Gallery selectedGallery = galleries.FirstOrDefault(g => g.GalleryID == selectedGalleryId);

                    // Check if selectedGallery is not null
                    if (selectedGallery != null)
                    {
                        Console.WriteLine("\n-------------------------------------------------------------");
                        Console.WriteLine($"| Gallery Details for '{selectedGallery.Name}'");
                        Console.WriteLine("-------------------------------------------------------------");
                        Console.WriteLine($"| Description: {selectedGallery.Description,-60} ");
                        Console.WriteLine($"| Location: {selectedGallery.Location,-65} ");
                        Console.WriteLine($"| Curator: {selectedGallery.CuratorID,-58} ");
                        Console.WriteLine($"| Opening Hours: {selectedGallery.OpeningHours,-54} ");
                        Console.WriteLine("-------------------------------------------------------------\n");
                    }

                    else
                    {
                        Console.WriteLine($"Gallery with ID {selectedGalleryId} not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid gallery ID.");
                }
            }
            else
            {
                Console.WriteLine("No galleries available.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while viewing galleries: {ex.Message}");
        }
    }
    #endregion

    #region ---> User Profile
    private static void UserProfile()
    {
        if (loggedInUser != null)
        {
            try
            {
                User userProfile = artGalleryService.GetUserProfile(loggedInUser.UserID);

                Console.WriteLine("\n-------------------------------------------------------------");
                Console.WriteLine($"| User Profile for {userProfile.UserName}");
                Console.WriteLine("-------------------------------------------------------------");
                Console.WriteLine($"| UserID:\t\t {userProfile.UserID,-30} |");
                Console.WriteLine($"| Username:\t\t {userProfile.UserName,-30} |");
                Console.WriteLine($"| Email:\t\t {userProfile.Email,-30} |");
                Console.WriteLine($"| First Name:\t\t {userProfile.FirstName,-30} |");
                Console.WriteLine($"| Last Name:\t\t {userProfile.LastName,-30} |");
                Console.WriteLine($"| Date of Birth:\t {userProfile.DateOfBirth.ToShortDateString(),-30} |");
                Console.WriteLine($"| Profile Picture:\t {userProfile.ProfilePicture,-30} |");
                Console.WriteLine("-------------------------------------------------------------\n");

                if (userProfile.favoriteArtworks.Count > 0)
                {
                    Console.WriteLine("\nFavorite Artworks:");
                    Console.WriteLine("ArtworkID\tTitle\t\tDescription\tMedium");
                    foreach (var artworkId in userProfile.favoriteArtworks)
                    {
                        Artwork favoriteArtwork = artGalleryService.GetArtworkById(artworkId);

                        Console.WriteLine($"{favoriteArtwork.ArtworkID}\t\t{favoriteArtwork.Title}\t\t{favoriteArtwork.Description}\t\t{favoriteArtwork.Medium}");
                    }

                }
                else
                {
                    Console.WriteLine("\nNo favorite artworks.");
                }
            }
            catch (UserNotFoundException ex)
            {
                Console.WriteLine($"Error while fetching user profile: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("User not logged in. Please login first.");
        }
    }
    #endregion

    #region ---> ArtworkManagerOptions
    private static void ArtworkManagerOptions()
    {
        if (loggedInUser != null)
        {
            ArtworkManager artworkManager = new ArtworkManager(artGalleryService);
            artworkManager.ManageArtworks(loggedInUser);
        }
        else
        {
            Console.WriteLine("User not logged in. Please log in first.");
        }
    }
    #endregion

    #region ---> ArtistManagerOptions
    private static void ArtistManagerOptions()
    {
        if (loggedInUser != null)
        {
            ArtistManager artistManager = new ArtistManager(artGalleryService);
            artistManager.ManageArtists(loggedInUser);
        }
        else
        {
            Console.WriteLine("User not logged in. Please log in first.");
        }
    }
    #endregion

    #region
    //private static void AddNewArtist()
    //{
    //    if (loggedInUser != null)
    //    {
    //        ArtistManager artistManager = new ArtistManager(artGalleryService);
    //        artistManager.AddNewArtist(loggedInUser);
    //    }
    //    else
    //    {
    //        Console.WriteLine("User not logged in. Please login first.");
    //    }
    //}
    #endregion

    #region ---> GalleryManagerOptions
    private static void GalleryManagerOptions()
    {
        if (loggedInUser != null)
        {
            GalleryManager galleryManager = new GalleryManager(artGalleryService);
            galleryManager.ManageGalleries(loggedInUser);
        }
        else
        {
            Console.WriteLine("User not logged in. Please log in first.");
        }
    }
    #endregion

    #region ---> UserProfileManagerOptions
    private static void UserProfileManagerOptions()
    {
        if (loggedInUser != null)
        {
            UserProfileManager userManager = new UserProfileManager(artGalleryService);
            userManager.ManageUserProfiles(loggedInUser);
        }
        else
        {
            Console.WriteLine("User not logged in. Please log in first.");
        }
    }
    #endregion

    #region

    //private static void AddNewGallery()
    //{
    //    if (loggedInUser != null)
    //    {
    //        GalleryManager galleryManager = new GalleryManager(artGalleryService);
    //        galleryManager.AddNewGallery(loggedInUser);
    //    }
    //    else
    //    {
    //        Console.WriteLine("User not logged in. Please log in first.");
    //    }
    //}

    //private static void EditUserProfile()
    //{
    //    //Console.WriteLine("Implementation ongoing! Please Wait :)");
    //    if (loggedInUser != null)
    //    {
    //        UserProfileManager UserManager = new UserProfileManager(artGalleryService);
    //        UserManager.EditUserProfile(loggedInUser);
    //    }
    //    else
    //    {
    //        Console.WriteLine("User not logged in. Please log in first.");
    //    }
    //}

    //private static void EditArtwork()
    //{
    //    if (loggedInUser != null)
    //    {
    //        new ArtworkManager(new VirtualArtGalleryImpl()).EditArtwork(loggedInUser);
    //    }
    //    else
    //    {
    //        Console.WriteLine("User not logged in. Please log in first.");
    //    }
    //}

    //private static void RemoveArtwork()
    //{
    //    if (loggedInUser != null)
    //    {
    //        new ArtworkManager(new VirtualArtGalleryImpl()).RemoveArtwork(loggedInUser);
    //    }
    //    else
    //    {
    //        Console.WriteLine("User not logged in. Please log in first.");
    //    }
    //}
    #endregion

}
