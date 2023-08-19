namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend.Filter
{
    public class TracesFilterViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Filter.Contract.IEntityFilterModel<Framework.Guama.Core.Persistence.ORM.EF.Entities.Trace, long>
    {
        public string Author { get; set; }

        public System.Linq.Expressions.Expression<System.Func<Framework.Guama.Core.Persistence.ORM.EF.Entities.Trace, bool>> GetSelector()
        {
            if (string.IsNullOrEmpty(Author)) return entity => true;

            return entity => entity.Author.ToLower().Contains(Author.ToLower());
        }
    }
}
