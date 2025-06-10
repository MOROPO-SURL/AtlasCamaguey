namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend.Input
{
    public class TraceInputViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityInputModel<Framework.Guama.Core.Persistence.ORM.EF.Entities.Trace, long>
    {
        public long Id { get; set; }

        public Framework.Guama.Core.Persistence.ORM.EF.Entities.Trace Export()
        {
            throw new System.NotImplementedException();
        }

        public void Import(Framework.Guama.Core.Persistence.ORM.EF.Entities.Trace entity)
        {
            throw new System.NotImplementedException();
        }

        public void Mergue(Framework.Guama.Core.Persistence.ORM.EF.Entities.Trace entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
