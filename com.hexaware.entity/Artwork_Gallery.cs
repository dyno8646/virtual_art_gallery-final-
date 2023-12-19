using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Art_Gallery.com.hexaware.entity
{
    internal class Artwork_Gallery
    {
        public Artwork ArtworkID { get; set; }
        public Gallery GalleryID { get; set; }

        public Artwork_Gallery() { }

        public Artwork_Gallery(Artwork artworkID, Gallery galleryID)
        {
            this.ArtworkID = artworkID;
            this.GalleryID = galleryID;
        }

        public override string ToString()
        {
            return $"ArtworkID: {ArtworkID}, GalleryID: {GalleryID}";
        }
    }
}
