using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Art_Gallery.com.hexaware.entity
{
    public class Artwork
    {
        private int artworkID;
        private string title;
        private string description;
        private DateTime creationDate;
        private string medium;
        private string imageURL;
        public int ArtistID { get; set; }

        public Artwork(int artworkID, string title, string description, DateTime creationDate, string medium, string imageURL, int artistID)
        {
            this.ArtworkID = artworkID;
            this.Title = title;
            this.Description = description;
            this.CreationDate = creationDate;
            this.Medium = medium;
            this.ImageURL = imageURL;
            this.ArtistID = artistID;
        }
        #region ---> Getters Setters
        public int ArtworkID
        {
            get { return artworkID; }
            set { artworkID = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }
        public string Medium
        {
            get { return medium; }
            set { medium = value; }
        }
        public string ImageURL
        {
            get { return imageURL; }
            set { imageURL = value; }
        }
        #endregion

        public override string ToString()
        {
            return $"ArtworkID: {ArtworkID}, Title: {Title}, Description: {Description}, " +
                   $"CreationDate: {CreationDate}, Medium: {Medium}, ImageURL: {ImageURL}";
        }
    }
}
