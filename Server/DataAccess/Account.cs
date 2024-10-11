namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Account")]
    public partial class Account
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Account()
        {
            Player = new HashSet<Player>();
        }

        public int accountId { get; set; }

        [StringLength(50)]
        public string name { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        [Required]
        [StringLength(50)]
        public string gamertag { get; set; }

        [Required]
        [StringLength(50)]
        public string password { get; set; }

        [StringLength(20)]
        public string status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Player> Player { get; set; }
    }
}
