using Moncore.Domain.Entities.Base;

namespace Moncore.Domain.Entities
{
    public class Photo : BaseEntity
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
    }

}
