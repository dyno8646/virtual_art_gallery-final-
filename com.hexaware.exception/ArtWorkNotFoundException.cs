using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Art_Gallery.com.hexaware.exception
{
    internal class ArtWorkNotFoundException:Exception
    {
        public ArtWorkNotFoundException() : base("Artwork not found.")
        {
        }

        public ArtWorkNotFoundException(string message) : base(message)
        {
        }

    }
}
