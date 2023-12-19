using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Virtual_Art_Gallery.com.hexaware.entity;
using Virtual_Art_Gallery.com.hexaware.util;

namespace Virtual_Art_Gallery.com.hexaware.dao
{
    public interface IVirtualArtGallery
    {
        private static IVirtualArtGallery artGalleryService = new VirtualArtGalleryImpl(PropertyUtil.GetConnectionString());
        User Login(string username, string password);
        bool Register(User user);
        bool UsernameExists(string username);
        List<Artwork> BrowseArtworks();
        List<Artwork> GetArtworks();
        List<Artist> SearchArtists(string keyword);
        List<Gallery> ViewGalleries();
        List<Artist> GetExistingArtistsFromDatabase();
        Artwork GetArtworkById(int artworkId);
        User GetUserProfile(int userId);
        int AddArtwork(Artwork artwork);
        bool UpdateArtwork(Artwork artwork);
        bool DeleteArtwork(Artwork artwork);
        int AddArtist(Artist artist);
        bool UpdateArtist(Artist artist);
        bool DeleteArtist(Artist artist);
        int AddGallery(Gallery gallery);
        bool UpdateGallery(Gallery gallery);
        void EditGallery(Gallery gallery);
        bool DeleteGallery(Gallery gallery);
        int AddUser(User user);
        void UpdateUserProfile(User user);
        bool DeleteUser(User user);
        void Logout();
    }
}
