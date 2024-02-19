namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class Layer : Framework.Guama.Core.Persistence.Entities.Contract.INamedEntity<System.Guid>
    {
        public Layer()
        {
            Id = System.Guid.NewGuid();
        }

        [System.ComponentModel.DataAnnotations.Key]
        public System.Guid Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Name { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Title { get; set; }

        public string StylePresentantionJSBlock { get; set; }

        public string InteractivePresentantionJSBlock { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string CRS { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string EPSG { get; set; }

        public decimal WestBoundLongitude { get; set; }

        public decimal EastBoundLatitude { get; set; }

        public decimal SouthBoundLatitude { get; set; }

        public decimal NorthBoundLatitude { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("MapId")]
        public Map Map { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string MapId { get; set; }
    }
}
