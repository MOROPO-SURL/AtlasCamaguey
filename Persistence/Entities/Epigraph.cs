using System.Collections.Generic;

namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class Epigraph : Framework.Guama.Core.Persistence.Entities.Contract.INamedEntity<System.Guid>
    {
        [System.ComponentModel.DataAnnotations.Key]
        public System.Guid Id { get; set; } = System.Guid.NewGuid();

        public int Priority { get; set; } = 1;

        [System.ComponentModel.DataAnnotations.Required]
        public string Name { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Description { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey(nameof(ImageId))]
        public Framework.Guama.Media.Persistence.Entities.Image Image { get; set; }
        public System.Guid? ImageId { get; set; }

        public Enumerations.EpigraphLayout Layout { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey(nameof(SectionId))]
        public Section Section { get; set; }
        public System.Guid SectionId { get; set; }

        public List<EpigraphKeyWordBridge> KeyWordBridges { get; set; } = new List<EpigraphKeyWordBridge>();

        public List<EpigraphImageBridge> ImageBridges { get; set; } = new List<EpigraphImageBridge>();

        public List<EpigraphVideoBridge> VideoBridges { get; set; } = new List<EpigraphVideoBridge>();

        public List<EpigraphDocumentBridge> DocumentBridges { get; set; } = new List<EpigraphDocumentBridge>();
    }
}
