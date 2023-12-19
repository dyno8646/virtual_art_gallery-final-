using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Art_Gallery.com.hexaware.exception
{
    internal class GalleryNotFoundException:Exception
    {
        public GalleryNotFoundException(string message) : base(message)
        {
        }
    }
}
