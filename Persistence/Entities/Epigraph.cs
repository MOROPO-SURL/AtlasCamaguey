using System.Collections.Generic;

namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class Epigraph : Framework.Guama.Core.Persistence.Entities.Contract.INamedEntity<System.Guid>
    {
        public Epigraph()
        {
            Id = System.Guid.NewGuid();
            KeyWordBridges = new List<EpigraphKeyWordBridge>();
            ImageBridges = new List<EpigraphImageBridge>();
            VideoBridges = new List<EpigraphVideoBridge>();
            DocumentBridges = new List<EpigraphDocumentBridge>();
            MapBridges = new List<EpigraphMapBridge>();
        }

        [System.ComponentModel.DataAnnotations.Key]
        public System.Guid Id { get; set; }

        public int Priority { get; set; } = 1;

        [System.ComponentModel.DataAnnotations.Required]
        public string Name { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Description { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("ImageId")]
        public Framework.Guama.Media.Persistence.Entities.Image Image { get; set; }
        public System.Guid? ImageId { get; set; }

        public Enumerations.EpigraphLayout Layout { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("SectionId")]
        public Section Section { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public System.Guid? SectionId { get; set; }

        public List<EpigraphKeyWordBridge> KeyWordBridges { get; set; }
        
        public List<EpigraphImageBridge> ImageBridges { get; set; }

        public List<EpigraphVideoBridge> VideoBridges { get; set; }

        public List<EpigraphDocumentBridge> DocumentBridges { get; set; }

        public List<EpigraphMapBridge> MapBridges { get; set; }
    }
}
