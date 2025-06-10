namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend.Display
{
    public class TraceDisplayViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityDisplayModel<Framework.Guama.Core.Persistence.ORM.EF.Entities.Trace, long>
    {
        public long Id { get; set; }

        public Microsoft.EntityFrameworkCore.EntityState Type { get; set; }

        public string Author { get; set; }

        public System.DateTime Registered { get; private set; }

        public string EntityName { get; set; }

        public string BeforeData { get; set; }

        public string AfterData { get; set; }

        public void Import(Framework.Guama.Core.Persistence.ORM.EF.Entities.Trace entity)
        {
            Id = entity.Id;
            Type = entity.Type;
            Author = entity.Author;
            Registered = entity.Registered;
            EntityName = entity.EntityName;
            BeforeData = entity.BeforeData;
            AfterData = entity.AfterData;
        }
    }
}
