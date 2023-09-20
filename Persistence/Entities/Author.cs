namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class Author : Framework.Guama.Core.Persistence.Entities.Contract.IEntity<System.Guid>
    {
        public Author()
        {
            Id = System.Guid.NewGuid();
            Identity = new Framework.Guama.Identity.Persistence.Entities.PersonIdentity();
        }

        [System.ComponentModel.DataAnnotations.Key]
        public System.Guid Id { get; set; }

        public Framework.Guama.Identity.Persistence.Entities.PersonIdentity Identity { get; set; }
        public System.Guid? IdentityId { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("InstitutionId")]
        public Institution Institution { get; set; }
        public System.Guid? InstitutionId { get; set; }

        public string NamePrefix { get; set; }
    }
}
