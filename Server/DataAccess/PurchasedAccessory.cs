//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class PurchasedAccessory
    {
        public int playerId { get; set; }
        public int accessoryId { get; set; }
        public Nullable<bool> inUse { get; set; }
    
        public virtual Accessory Accessory { get; set; }
        public virtual Player Player { get; set; }
    }
}
