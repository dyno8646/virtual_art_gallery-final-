using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Art_Gallery.com.hexaware.entity
{
    internal class User_Favorite_Artwork
    {
        public int UserID { get; set; }
        public int ArtworkID { get; set; }

        public User_Favorite_Artwork() { }

        public User_Favorite_Artwork(int artworkID, int userID)
        {
            this.ArtworkID = artworkID;
            this.UserID = userID;
        }

        public override string ToString()
        {
            return $"ArtworkID: {ArtworkID}, UserID: {UserID}";
        }
    }
}
