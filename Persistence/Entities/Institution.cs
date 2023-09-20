namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class Institution : Framework.Guama.Core.Persistence.Entities.Contract.IEntity<System.Guid>
    {
        public Institution()
        {
            Id = System.Guid.NewGuid();
            Identity = new Framework.Guama.Identity.Persistence.Entities.OrganizationIdentity();
        }

        [System.ComponentModel.DataAnnotations.Key]
        public System.Guid Id { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("IdentityId")]
        public Framework.Guama.Identity.Persistence.Entities.OrganizationIdentity Identity { get; set; }
        public System.Guid? IdentityId { get; set; }

        public string Alias { get; set; }
    }
}
