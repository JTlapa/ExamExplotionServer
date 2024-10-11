namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Accessory")]
    public partial class Accessory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Accessory()
        {
            PurchasedAccessory = new HashSet<PurchasedAccessory>();
        }

        public int accessoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string name { get; set; }

        public int price { get; set; }

        [StringLength(255)]
        public string description { get; set; }

        [StringLength(255)]
        public string imagesPackage { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchasedAccessory> PurchasedAccessory { get; set; }
    }
}
