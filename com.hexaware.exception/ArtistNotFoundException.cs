using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Art_Gallery.com.hexaware.exception
{
    internal class ArtistNotFoundException:Exception
    {
        public ArtistNotFoundException() : base("Artist not found.")
        {
        }

        public ArtistNotFoundException(string message) : base(message)
        {
        }

    }
}
