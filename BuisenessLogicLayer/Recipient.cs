//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BuisenessLogicLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class Recipient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Recipient()
        {
            this.RecipientListsRelations = new HashSet<RecipientListsRelation>();
        }
    
        public int RecipientId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string SureName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecipientListsRelation> RecipientListsRelations { get; set; }
    }
}
