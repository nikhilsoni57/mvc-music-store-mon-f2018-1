using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mon_f2018.Models
{
    public interface IAlbumsMock
    {
        IQueryable<Album> Albums { get; }
        IQueryable<Artist> Artists { get; }
        Album Save(Album album);
        void Delete(Album album);
    }
}
