namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View
{
    public class LayerDisplayViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityDisplayModel<Camaguey.Persistence.Entities.Layer, System.Guid>
    {
        public System.Guid Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string StylePresentantionJSBlock { get; set; }

        public string InteractivePresentantionJSBlock { get; set; }

        public string CRS { get; set; }

        public string EPSG { get; set; }

        public decimal WestBoundLongitude { get; set; }

        public decimal EastBoundLatitude { get; set; }

        public decimal SouthBoundLatitude { get; set; }

        public decimal NorthBoundLatitude { get; set; }

        public void Import(Camaguey.Persistence.Entities.Layer entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Title = entity.Title;
            StylePresentantionJSBlock = entity.StylePresentantionJSBlock;
            InteractivePresentantionJSBlock = entity.InteractivePresentantionJSBlock;
            CRS = entity.CRS;
            EPSG = entity.EPSG;
            WestBoundLongitude = entity.WestBoundLongitude;
            EastBoundLatitude = entity.EastBoundLatitude;
            SouthBoundLatitude = entity.SouthBoundLatitude;
            NorthBoundLatitude = entity.NorthBoundLatitude;
        }
    }
}
