using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Virtual_Art_Gallery.com.hexaware.dao;
using Virtual_Art_Gallery.com.hexaware.entity;
using Virtual_Art_Gallery.com.hexaware.util;
using System.Data.SqlClient;

namespace Virtual_Art_Gallery_Testing
{
    internal class ArtistManagerTests
    {
        [Test]
        public void Test_Add_Artist_and_Delete_Artist()
        {
            string c = "lav";
            using (SqlConnection connection = DBConnection.GetConnection()) ;
            VirtualArtGalleryImpl test = new VirtualArtGalleryImpl(c);

            Artist artist = new Artist(100, "ArtistName", "ArtistBio",new DateTime(2020,10,20),"Nationality","Website", "Contact");

            int artistId = test.AddArtist(artist);
            bool t2 = test.DeleteArtist(artist);

            Assert.That(artistId>0);
            Assert.That(t2);
        }

        [Test]
        public void Test_Search_Artists()
        {
            List<Artist> artists = null;

            string c = "lav";
            using (SqlConnection connection = DBConnection.GetConnection()) ;
            VirtualArtGalleryImpl test = new VirtualArtGalleryImpl(c);

            string searchKeyword = "Anita";

            artists = test.SearchArtists(searchKeyword);

            Assert.That(artists.Count > 0);
        }

        [Test]
        public void Test_Update_Artist()
        {
            string c = "lav";
            using (SqlConnection connection = DBConnection.GetConnection()) ;
            VirtualArtGalleryImpl test = new VirtualArtGalleryImpl(c);

            Artist artist = new Artist(1, "Alice Johnson", "Renowned artist with a passion for nature", new DateTime(1980,05,15), "American", "www.alicejohnson.com", "Contact Alice at alice@email.com");

            bool updateSuccess = test.UpdateArtist(artist);

            Assert.That(updateSuccess);
        }
    }
}
