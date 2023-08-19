using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using MOROPO.Framework.Guama.Media.Persistence.Entities;
using MOROPO.Framework.Guama.Media.Presentation.Web.Common.Managers.Facade;
using MOROPO.Framework.Guama.Security.Personal.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Managers
{
    public class UserProfileManager : Framework.Guama.Core.Presentation.Web.Security.Managers.Facade.IUserProfileManagerFacade
    {
        private readonly IEntityRepository<User, string> userRepository;
        private readonly IEntityRepository<Image, System.Guid> imageRepository;
        private readonly IImageFileManagerFacade imageFileManager;

        public UserProfileManager(IImageFileManagerFacade imageFileManager, IEntityRepository<Image, System.Guid> imageRepository, IEntityRepository<User, string> userRepository)
        {
            this.userRepository = userRepository;
            this.imageRepository = imageRepository;
            this.imageFileManager = imageFileManager;
        }

        public Framework.Guama.Core.Presentation.Web.Security.Models.View.Contrat.IUserProfileViewModel GetUserProfile(string username)
        {
            Models.View.UserProfileViewModel result = null;

            try
            {
                var entity = userRepository.Get(username);

                result = new Models.View.UserProfileViewModel
                {
                    Name = entity.Identity.Name,
                    LastName = entity.Identity.LastName,
                    Alias = entity.Identity.Alias,
                    Sex = entity.Identity.Sex,
                    About = entity.Identity.About,
                    ImageId = entity.Identity.ImageId,
                    ImageExtension = entity.Identity.Image == null? null : MimeTypes.MimeTypeMap.GetExtension(entity.Identity.Image.ContentType)
                };
            }
            catch { }

            return result;
        }

        public void PreValidateUserProfile(string username, Framework.Guama.Core.Presentation.Web.Security.Models.View.Contrat.IUserProfileViewModel profileModel, Microsoft.AspNetCore.Mvc.Controller controller)
        {
            controller.ModelState.ClearValidationState("ImageFile");
            controller.ModelState.MarkFieldValid("ImageFile");

            if (controller.Request.Form.Files.Count > 0)
            {
                var formFile = controller.Request.Form.Files[0];

                if (formFile.Length > 0)
                {
                    (profileModel as Models.View.UserProfileViewModel).ImageFile = formFile;

                    var updateModelTask = controller.TryUpdateModelAsync(profileModel);

                    updateModelTask.Wait();
                }
            }
        }

        public void UpdateUserProfile(string username, Framework.Guama.Core.Presentation.Web.Security.Models.View.Contrat.IUserProfileViewModel profileModel)
        {
            var concreteProfileModel = profileModel as Models.View.UserProfileViewModel;

            var entity = userRepository.Get(username);

            entity.Identity.Name = concreteProfileModel.Name;
            entity.Identity.LastName = concreteProfileModel.LastName;
            entity.Identity.Alias = concreteProfileModel.Alias;
            entity.Identity.Sex = concreteProfileModel.Sex;
            entity.Identity.About = concreteProfileModel.About;

            if (concreteProfileModel.ImageFile == null)
            {
                if (concreteProfileModel.ResetImage && entity.Identity.Image != null)
                {
                    imageFileManager.DeleteImageFile(entity.Identity.Image);

                    imageRepository.Delete(entity.Identity.Image, username);
                }
            }
            else
            {
                if (entity.Identity.Image != null)
                {
                    imageFileManager.DeleteImageFile(entity.Identity.Image);

                    imageRepository.Delete(entity.Identity.Image, username);
                }

                using (var memoryStream = new System.IO.MemoryStream())
                {
                    concreteProfileModel.ImageFile.CopyTo(memoryStream);

                    var image = new Image { ContentType = concreteProfileModel.ImageFile.ContentType, Length = concreteProfileModel.ImageFile.Length, About = "User " + entity.Id };

                    imageRepository.Add(image, username, false);

                    imageFileManager.SaveImageFileContent(image.Id, memoryStream.ToArray(), concreteProfileModel.ImageFile.ContentType);

                    entity.Identity.ImageId = image.Id;
                    entity.Identity.Image = image;
                }
            }

            userRepository.Update(entity, username);
        }
    }
}
