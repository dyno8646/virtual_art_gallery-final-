using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Virtual_Art_Gallery.com.hexaware.dao;
using Virtual_Art_Gallery.com.hexaware.entity;
using Virtual_Art_Gallery.com.hexaware.exception;

namespace Virtual_Art_Gallery.com.hexaware.repo
{
    internal class ArtistManager
    {
        private readonly IVirtualArtGallery artGalleryService;
        public ArtistManager(IVirtualArtGallery artGalleryService)
        {
            this.artGalleryService = artGalleryService;
        }

        #region ---> Manage Artists
        public void ManageArtists(User loggedInUser)
        {
            while (true)
            {
                Console.WriteLine("\nArtist Management Options:\n");
                Console.WriteLine("1. Add New Artist");
                Console.WriteLine("2. Edit Artist");
                Console.WriteLine("3. Remove Artist");
                Console.WriteLine("4. Return to Main Menu\n");

                Console.Write("\nSelect an option: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            AddNewArtist(loggedInUser);
                            break;

                        case 2:
                            EditArtist(loggedInUser);
                            break;

                        case 3:
                            RemoveArtist(loggedInUser);
                            break;

                        case 4:
                            return;

                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }
        #endregion

        #region ---> Add New Artist
        public void AddNewArtist(User loggedInUser)
        {
            Console.WriteLine("Adding a new artist:");

            Console.Write("Enter name: ");
            string name = Console.ReadLine();

            Console.Write("Enter biography: ");
            string biography = Console.ReadLine();

            Console.Write("Enter birth date (yyyy-MM-dd): ");
            if (DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime birthDate))
            {
                Console.Write("Enter nationality: ");
                string nationality = Console.ReadLine();

                Console.Write("Enter website: ");
                string website = Console.ReadLine();
                if (!website.StartsWith("www.") || !website.EndsWith(".com"))
                {
                    Console.WriteLine("Invalid website format. Website must start with 'www.' and end with '.com'.");
                    return;
                }

                Console.Write("Enter contact information\t(Enter Email):  ");
                string contactInformation = Console.ReadLine();
                if (!contactInformation.EndsWith("@email.com"))
                {
                    Console.WriteLine("Invalid email format. Email must end with '@email.com'.");
                    return;
                }

                try
                {
                    Artist newArtist = new Artist(
                        artistID: -1,
                        name: name,
                        biography: biography,
                        birthDate: birthDate,
                        nationality: nationality,
                        website: website,
                        contactInformation: contactInformation
                        );

                    artGalleryService.AddArtist(newArtist);
                    Console.WriteLine($"Artist {newArtist.Name} added successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding artist: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid date format. Please enter the birth date in yyyy-MM-dd format.");
            }
        }
        #endregion

        #region ---> Edit Artist
        public void EditArtist(User loggedInUser)
        {
            Console.WriteLine("Editing an artist:");
            Console.WriteLine("\n\n");
            Console.WriteLine("Select an artist to edit:");
            List<Artist> existingArtists = artGalleryService.GetExistingArtistsFromDatabase();

            if (existingArtists.Count > 0)
            {
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("| Artist ID |       Name          |");
                Console.WriteLine("-----------------------------------");

                foreach (var artist in existingArtists)
                {
                    Console.WriteLine($"| {artist.ArtistID,-10} | {artist.Name,-17} |"); 
                }

                Console.WriteLine("---------------------------------");


                Console.Write("Enter Artist ID to edit: ");
                if (int.TryParse(Console.ReadLine(), out int selectedArtistId))
                {
                    try
                    {
                        // Check if the selected artist belongs to the user
                        var selectedArtist = existingArtists.Find(a => a.ArtistID == selectedArtistId);

                        if (selectedArtist != null)
                        {
                            // Display the current details of the artist
                            Console.WriteLine("Current Details:");
                            Console.WriteLine($"Name: {selectedArtist.Name}");
                            Console.WriteLine($"Biography: {selectedArtist.Biography}");
                            Console.WriteLine($"BirthDate: {selectedArtist.BirthDate}");
                            Console.WriteLine($"Nationality: {selectedArtist.Nationality}");
                            Console.WriteLine($"Website: {selectedArtist.Website}");
                            Console.WriteLine($"Contact Information: {selectedArtist.ContactInformation}");

                            // Get updated information from the user
                            Console.Write("Enter new name: ");
                            string newName = Console.ReadLine();

                            Console.Write("Enter new biography: ");
                            string newBiography = Console.ReadLine();

                            Console.Write("Enter new birth date (yyyy-MM-dd): ");
                            if (DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newBirthDate))
                            {
                                Console.Write("Enter new nationality: ");
                                string newNationality = Console.ReadLine();

                                Console.Write("Enter new website: ");
                                string newWebsite = Console.ReadLine();

                                Console.Write("Enter new contact information: ");
                                string newContactInformation = Console.ReadLine();

                                // Update the artist
                                selectedArtist.Name = newName;
                                selectedArtist.Biography = newBiography;
                                selectedArtist.BirthDate = newBirthDate;
                                selectedArtist.Nationality = newNationality;
                                selectedArtist.Website = newWebsite;
                                selectedArtist.ContactInformation = newContactInformation;

                                // Call the service method to update the artist in the database
                                artGalleryService.UpdateArtist(selectedArtist);

                                Console.WriteLine($"Artist with ID {selectedArtistId} updated successfully.");
                            }
                            else
                            {
                                Console.WriteLine("Invalid date format. Artist not updated.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Artist ID. Please select a valid artist to edit.");
                        }
                    }
                    catch (ArtistNotFoundException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error editing artist: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid numeric Artist ID.");
                }
            }
            else
            {
                Console.WriteLine("No artists found in the database.");
            }
        }
        #endregion

        #region ---> Remove Artist
        public void RemoveArtist(User loggedInUser)
        {
            Console.WriteLine("Removing an artist:");

            Console.Write("Enter keyword to search for artists: ");
            string keyword = Console.ReadLine();

            List<Artist> matchingArtists = artGalleryService.SearchArtists(keyword);

            if (matchingArtists.Count > 0)
            {
                Console.WriteLine("Matching Artists:");
                foreach (var artist in matchingArtists)
                {
                    Console.WriteLine($"ID: {artist.ArtistID}, Name: {artist.Name}");
                }

                // Get user input for artist selection
                Console.Write("Enter Artist ID to remove: ");
                if (int.TryParse(Console.ReadLine(), out int selectedArtistId))
                {
                    // Check if the selected artist belongs to the user
                    var selectedArtist = matchingArtists.Find(a => a.ArtistID == selectedArtistId);

                    if (selectedArtist != null)
                    {
                        // Call the service method to remove the artist from the database
                        artGalleryService.DeleteArtist(selectedArtist);

                        Console.WriteLine($"Artist with ID {selectedArtistId} removed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid Artist ID. Please select a valid artist to remove.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid numeric Artist ID.");
                }
            }
            else
            {
                Console.WriteLine($"No artists found matching '{keyword}'.");
            }
        }
        #endregion

    }
}
