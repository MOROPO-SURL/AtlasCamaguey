using System.Collections.Generic;

namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class Map : Framework.Guama.Core.Persistence.Entities.Contract.IEntity<string>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.RegularExpression("^s[0-9]+_e[0-9]+_t[0-9]+_m[0-9]+(_[a_z])?$")]
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

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey(nameof(ImageId))]
        public Framework.Guama.Media.Persistence.Entities.Image Image { get; set; }
        public System.Guid? ImageId { get; set; }

        public List<MapImage> Images { get; set; } = new List<MapImage>();

        public List<MapVideo> Videos { get; set; } = new List<MapVideo>();

        public List<MapDocument> Documents { get; set; } = new List<MapDocument>();

        public List<Layer> Layers { get; set; } = new List<Layer>();

        public List<MapKeyWordBridge> KeyWordBridges { get; set; } = new List<MapKeyWordBridge>();

        public List<MapAuthorBridge> AuthorBridges { get; set; } = new List<MapAuthorBridge>();

        public string PreLayersJSBlock { get; set; }

        public string PostLayersJSBlock { get; set; }
    }
}
