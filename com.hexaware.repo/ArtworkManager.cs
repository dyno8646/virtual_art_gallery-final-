using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Virtual_Art_Gallery.com.hexaware.entity;
using Virtual_Art_Gallery.com.hexaware.dao;
using Virtual_Art_Gallery.com.hexaware.exception;
using System.Globalization;
using Virtual_Art_Gallery.com.hexaware.util;

namespace Virtual_Art_Gallery.com.hexaware.repo
{
    internal class ArtworkManager
    {
        private readonly IVirtualArtGallery artGalleryService;
        public ArtworkManager(IVirtualArtGallery artGalleryService)
        {
            this.artGalleryService = artGalleryService;
        }

        #region ---> Manage Artworks
        public void ManageArtworks(User loggedInUser)
        {
            while (true)
            {
                Console.WriteLine("\nChoose an action:\n");
                Console.WriteLine("1. Add Artwork");
                Console.WriteLine("2. Edit Artwork");
                Console.WriteLine("3. Remove Artwork");
                Console.WriteLine("4. Go back to the main menu\n");
                Console.Write("Enter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            AddNewArtwork(loggedInUser);
                            break;
                        case 2:
                            EditArtwork(loggedInUser);
                            break;
                        case 3:
                            RemoveArtwork(loggedInUser);
                            break;
                        case 4:
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please enter a number between 1 and 4.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }

                Console.WriteLine();
            }
        }
        #endregion

        #region ----> Add New Artwork
        public void AddNewArtwork(User loggedInUser)
        {
            VirtualArtGalleryImpl artgalleryService = new VirtualArtGalleryImpl(PropertyUtil.GetConnectionString());
            Console.WriteLine("Adding a new artwork:");

            Console.Write("Enter title: ");
            string title = Console.ReadLine();

            Console.Write("Enter description: ");
            string description = Console.ReadLine();

            Console.Write("Enter medium: ");
            string medium = Console.ReadLine();

            Console.Write("Enter imageurl: ");
            string imageURL = Console.ReadLine();

            try
            {
                List<Artist> existingArtists = artgalleryService.GetExistingArtistsFromDatabase();

                // Step 2: Display the list of artists
                Console.WriteLine("Select an artist or enter a new artist ID:");
                foreach (Artist artist in existingArtists)
                {
                    Console.WriteLine($"{artist.ArtistID}. {artist.Name}");
                }

                // Step 3: User selection
                Console.Write("Enter artist ID: ");
                if (int.TryParse(Console.ReadLine(), out int selectedArtistID))
                {
                    // Step 4: Validate artist ID
                    Artist selectedArtist = existingArtists.FirstOrDefault(a => a.ArtistID == selectedArtistID);
                    if (selectedArtist != null)
                    {
                        // Step 5: Add Artwork with the selected artist ID
                        Artwork newArtwork = new Artwork(
                            artworkID: 0,
                            title: title,
                            description: description,
                            creationDate: DateTime.Now,
                            medium: medium,
                            imageURL: imageURL,
                            artistID: selectedArtistID
                        );

                        int artworkId = artGalleryService.AddArtwork(newArtwork);

                        Console.WriteLine($"Artwork added successfully. Artwork ID: {artworkId}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid artist ID. Please select an existing artist.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid artist ID.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding artwork: {ex.Message}");
            }
        }
        #endregion

        #region ----> Edit Artwork
        public void EditArtwork(User loggedInUser)
        {
            Console.WriteLine("Editing an artwork:");
            Console.WriteLine("\n\n");
            Console.WriteLine("Select an artwork to edit:");
            List<Artwork> allArtworks = artGalleryService.BrowseArtworks();

            if (allArtworks.Count > 0)
            {
                Console.Write("\nEnter Artwork ID to edit: ");
                int selectedArtworkId;
                if (int.TryParse(Console.ReadLine(), out selectedArtworkId))
                {
                    try
                    {
                        Artwork selectedArtwork = artGalleryService.GetArtworkById(selectedArtworkId);

                        if (selectedArtwork != null && selectedArtwork.ArtistID == loggedInUser.UserID)
                        {
                            Console.WriteLine("\nCurrent Details:");
                            Console.WriteLine($"Title: {selectedArtwork.Title}");
                            Console.WriteLine($"Description: {selectedArtwork.Description}");
                            Console.WriteLine($"Medium: {selectedArtwork.Medium}");
                            Console.WriteLine($"ImageURL: {selectedArtwork.ImageURL}");
                            Console.WriteLine($"CreationDate: {selectedArtwork.CreationDate}");

                            Console.Write("\nEnter new title: ");
                            string newTitle = Console.ReadLine();

                            Console.Write("Enter new description: ");
                            string newDescription = Console.ReadLine();

                            Console.Write("Enter new medium: ");
                            string newMedium = Console.ReadLine();

                            Console.Write("Enter new image URL: ");
                            string newImageUrl = Console.ReadLine();

                            Console.Write("Enter new creation date (yyyy-MM-dd): ");
                            if (DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newCreationDate))
                            {
                                // Update the artwork
                                selectedArtwork.Title = newTitle;
                                selectedArtwork.Description = newDescription;
                                selectedArtwork.Medium = newMedium;
                                selectedArtwork.ImageURL = newImageUrl;
                                selectedArtwork.CreationDate = newCreationDate;

                                // Call the service method to update the artwork in the database
                                artGalleryService.UpdateArtwork(selectedArtwork);

                                Console.WriteLine($"Artwork with ID {selectedArtworkId} updated successfully.");
                            }
                            else
                            {
                                Console.WriteLine("Invalid date format. Artwork not updated.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Artwork ID. Please select an artwork_id that belongs to you.. to edit.");
                        }
                    }
                    catch (ArtWorkNotFoundException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error editing artwork: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid numeric Artwork ID.");
                }
            }
            else
            {
                Console.WriteLine("You have no artworks to edit.");
            }
        }
        #endregion

        #region ----> RemoveArtwork
        public void RemoveArtwork(User loggedInUser)
        {
            Console.WriteLine("Removing an artwork:");

            Console.WriteLine("Select an artwork to remove:");
            List<Artwork> allArtworks = artGalleryService.BrowseArtworks();

            if (allArtworks.Count > 0)
            {
                // Get user input for artwork selection
                Console.Write("Enter Artwork ID to remove: ");
                if (int.TryParse(Console.ReadLine(), out int selectedArtworkId))
                {
                    // Check if the selected artwork belongs to the user
                    var selectedArtwork = allArtworks.Find(a => a.ArtworkID == selectedArtworkId);

                    if (selectedArtwork != null)
                    {
                        // Call the service method to remove the artwork from the database
                        artGalleryService.DeleteArtwork(selectedArtwork);

                        Console.WriteLine($"Artwork with ID {selectedArtworkId} removed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid Artwork ID. Please select a valid artwork to remove.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid numeric Artwork ID.");
                }
            }
            else
            {
                Console.WriteLine("You have no artworks to remove.");
            }
        }
        #endregion

    }
}
