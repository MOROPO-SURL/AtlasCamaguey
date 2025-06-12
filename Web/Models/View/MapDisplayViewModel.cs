using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using System.Linq;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View
{
    public class MapDisplayViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityDisplayModel<Map, string>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public uint Priority { get; set; }

        public long Scale { get; set; }

        public string Description { get; set; }

        public string CRS { get; set; }

        public string EPSG { get; set; }

        public decimal WestBoundLongitude { get; set; }

        public decimal EastBoundLatitude { get; set; }

        public decimal SouthBoundLatitude { get; set; }

        public decimal NorthBoundLatitude { get; set; }

        public System.Guid? ImageId { get; set; }
        public string ImageExtension { get; set; }

        public LayerDisplayViewModel[] Layers { get; set; }

        public string PreLayersJSBlock { get; set; }

        public string PostLayersJSBlock { get; set; }

        public System.Guid EphigraphId { get; set; }

        public void Import(Map entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Scale = entity.Scale;
            Description = entity.Description;
            Priority = (uint)entity.Priority;
            CRS = entity.CRS;
            EPSG = entity.EPSG;
            WestBoundLongitude = entity.WestBoundLongitude;
            EastBoundLatitude = entity.EastBoundLatitude;
            SouthBoundLatitude = entity.SouthBoundLatitude;
            NorthBoundLatitude = entity.NorthBoundLatitude;
            ImageId = entity.ImageId;
            ImageExtension = entity.Image == null ? null : MimeTypes.MimeTypeMap.GetExtension(entity.Image.ContentType);
            PreLayersJSBlock = entity.PreLayersJSBlock;
            PostLayersJSBlock = entity.PostLayersJSBlock;

            Layers = entity.Layers.Where(_entity => _entity.Enabled).OrderBy(_entity => _entity.Priority).Select(_entity =>
            {
                var model = new LayerDisplayViewModel();

                model.Import(_entity);

                return model;
            }).ToArray();
        }
    }
}

