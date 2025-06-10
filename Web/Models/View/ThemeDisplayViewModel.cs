namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View
{
	public class ThemeDisplayViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityDisplayModel<Camaguey.Persistence.Entities.Theme, System.Guid>
    {
        public System.Guid Id { get; set; }

        public string Name { get; set; }

        public int Priority { get; set; }

        public EpigraphDisplayViewModel Epigraph { get; set; }

        public MapDisplayViewModel[] Maps { get; set; }

        public void Import(Camaguey.Persistence.Entities.Theme entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Priority = entity.Priority;
            Epigraph = new EpigraphDisplayViewModel();

            Epigraph.Import(entity.Epigraph);
        }
    }
}

