using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Art_Gallery.com.hexaware.entity
{
    public class Gallery
    {
        private int galleryID;
        private string name;
        private string description;
        private string location;
        public int CuratorID { get; set; }
        private string openingHours;

        public Gallery() { }
        public Gallery(int galleryId, string name, string description, string location, int curatorID, string openingHours)
        {
            GalleryID = galleryId;
            Name = name;
            Description = description;
            Location = location;
            CuratorID = curatorID;
            OpeningHours = openingHours;
        }
        #region ---> Getters Setters
        public int GalleryID
        {
            get { return galleryID; }
            set { galleryID = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string Location
        {
            get { return location; }
            set { location = value; }
        }
        public string OpeningHours
        {
            get { return openingHours; }
            set { openingHours = value; }
        }
        #endregion

        public override string ToString()
        {
            return $"GalleryID ={GalleryID}\nName ={Name}\nDescription ={Description}\nLocation ={Location}\ncuratorID :{CuratorID}\nOpeningHours ={OpeningHours}";
        }
    }
}
