namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class Institution : Framework.Guama.Core.Persistence.Entities.Contract.IEntity<System.Guid>
    {
        [System.ComponentModel.DataAnnotations.Key]
        public System.Guid Id { get; set; } = System.Guid.NewGuid();

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey(nameof(IdentityId))]
        public Framework.Guama.Identity.Persistence.Entities.OrganizationIdentity Identity { get; set; }
        public System.Guid? IdentityId { get; set; }

        public string Alias { get; set; }
    }
}