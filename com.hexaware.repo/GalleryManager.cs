using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Virtual_Art_Gallery.com.hexaware.dao;
using Virtual_Art_Gallery.com.hexaware.entity;

namespace Virtual_Art_Gallery.com.hexaware.repo
{
    internal class GalleryManager
    {
        private readonly IVirtualArtGallery artGalleryService;

        public GalleryManager(IVirtualArtGallery artGalleryService)
        {
            this.artGalleryService = artGalleryService;
        }

        #region ---> Manage Galleries
        public void ManageGalleries(User loggedInUser)
        {
            Console.WriteLine("Choose an action:");
            Console.WriteLine("\n1. Add Gallery");
            Console.WriteLine("2. Edit Gallery");
            Console.WriteLine("3. Delete Gallery");
            Console.WriteLine("4. Go back to the main menu\n");
            Console.Write("Enter your choice: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        AddNewGallery(loggedInUser);
                        break;
                    case 2:
                        EditGallery(loggedInUser);
                        break;
                    case 3:
                        DeleteGallery(loggedInUser);
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

        #region ---> Add New Gallery
        public void AddNewGallery(User loggedInUser)
        {
            Console.WriteLine("Adding a new gallery:");

            Console.Write("Enter gallery name: ");
            string name = Console.ReadLine();

            Console.Write("Enter description: ");
            string description = Console.ReadLine();

            Console.Write("Enter location: ");
            string location = Console.ReadLine();

            Console.Write("Enter opening hours: ");
            string openingHours = Console.ReadLine();

            try
            {
                Gallery newGallery = new Gallery
                {
                    Name = name,
                    Description = description,
                    Location = location,
                    CuratorID = loggedInUser.UserID,
                    OpeningHours = openingHours
                };

                artGalleryService.AddGallery(newGallery);

                Console.WriteLine($"Gallery {newGallery.Name} added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding gallery: {ex.Message}");
            }
        }
        #endregion

        #region ---> Edit Gallery
        public void EditGallery(User loggedInUser)
        {
            Console.WriteLine("\nEditing a gallery:");
            var galleries = artGalleryService.ViewGalleries();
            if (galleries.Count > 0)
            {
                Console.WriteLine("\nSelect a gallery to edit:\n");
                foreach (var gallery in galleries)
                {
                    Console.WriteLine($"{gallery.GalleryID}. {gallery.Name}");
                }

                Console.Write("\nEnter the GalleryID to edit: ");
                if (int.TryParse(Console.ReadLine(), out int galleryId))
                {
                    var galleryToEdit = galleries.Find(g => g.GalleryID == galleryId);

                    if (galleryToEdit != null)
                    {
                        // Prompt user for updated information
                        Console.Write("Enter new gallery name: ");
                        galleryToEdit.Name = Console.ReadLine();

                        Console.Write("Enter new description: ");
                        galleryToEdit.Description = Console.ReadLine();

                        Console.Write("Enter new location: ");
                        galleryToEdit.Location = Console.ReadLine();

                        Console.Write("Enter new opening hours: ");
                        galleryToEdit.OpeningHours = Console.ReadLine();

                        artGalleryService.UpdateGallery(galleryToEdit);

                        Console.WriteLine($"Gallery {galleryToEdit.Name} updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Gallery with ID {galleryId} not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid GalleryID.");
                }
            }
            else
            {
                Console.WriteLine("No galleries found.");
            }
        }
        #endregion

        #region ---> Delete Gallery
        public void DeleteGallery(User loggedInUser)
        {
            Console.WriteLine("Deleting a gallery:");

            // Retrieve and display existing galleries for the user to choose from
            var galleries = artGalleryService.ViewGalleries();
            if (galleries.Count > 0)
            {
                Console.WriteLine("Select a gallery to delete:");
                foreach (var gallery in galleries)
                {
                    Console.WriteLine($"{gallery.GalleryID}. {gallery.Name}");
                }

                Console.Write("Enter the GalleryID to delete: ");
                if (int.TryParse(Console.ReadLine(), out int galleryId))
                {
                    var galleryToDelete = galleries.Find(g => g.GalleryID == galleryId);

                    if (galleryToDelete != null)
                    {
                        artGalleryService.DeleteGallery(galleryToDelete);

                        Console.WriteLine($"Gallery {galleryToDelete.Name} deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Gallery with ID {galleryId} not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid GalleryID.");
                }
            }
            else
            {
                Console.WriteLine("No galleries found.");
            }
        }
        #endregion
    }
}
