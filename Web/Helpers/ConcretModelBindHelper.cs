namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Helpers
{
    public class ConcretModelBindHelper : Framework.Guama.Core.Presentation.Web.Common.ModelBinding.Contract.IConcretModelBindHelper
    {
        public void DoConcretBind(object model, Microsoft.AspNetCore.Mvc.ModelBinding.ModelBindingContext bindingContext)
        {
            var concreteModel = model as Models.View.UserProfileViewModel;

            concreteModel.Name = bindingContext.ValueProvider.GetValue("Name").FirstValue;

            concreteModel.LastName = bindingContext.ValueProvider.GetValue("LastName").FirstValue;

            concreteModel.Sex = System.Enum.Parse<Framework.Guama.Identity.Persistence.Enumerations.Sex>(bindingContext.ValueProvider.GetValue("Sex").FirstValue);

            concreteModel.About = bindingContext.ValueProvider.GetValue("About").FirstValue;
        }
    }
}
