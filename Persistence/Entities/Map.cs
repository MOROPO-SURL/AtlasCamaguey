using System.Collections.Generic;

namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class Map : Framework.Guama.Core.Persistence.Entities.Contract.IEntity<string>
    {
        public Map()
        {
            KeyWordBridges = new List<MapKeyWordBridge>();
            ImageBridges = new List<MapImageBridge>();
            VideoBridges = new List<MapVideoBridge>();
            DocumentBridges = new List<MapDocumentBridge>();
            AuthorBridges = new List<MapAuthorBridge>();
            Layers = new List<Layer>();
        }

        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.RegularExpression("^S[0-9]+_E[0-9]+_M[0-9]+$")]
        public string Id { get; set; }

        public int Priority { get; set; } = 1;

        [System.ComponentModel.DataAnnotations.Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public long Scale { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string CRS { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string EPSG { get; set; }

        public decimal WestBoundLongitude { get; set; }

        public decimal EastBoundLatitude { get; set; }

        public decimal SouthBoundLatitude { get; set; }

        public decimal NorthBoundLatitude { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("ImageId")]
        public Framework.Guama.Media.Persistence.Entities.Image Image { get; set; }
        public System.Guid? ImageId { get; set; }

        public List<MapKeyWordBridge> KeyWordBridges { get; set; }

        public List<MapImageBridge> ImageBridges { get; set; }

        public List<MapVideoBridge> VideoBridges { get; set; }

        public List<MapDocumentBridge> DocumentBridges { get; set; }

        public List<MapAuthorBridge> AuthorBridges { get; set; }

        public List<Layer> Layers { get; set; }
    }
}
