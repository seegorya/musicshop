//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace mozzhhevelnik.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ordetails
    {
        public int Id { get; set; }
        public int ordid { get; set; }
        public int cdid { get; set; }
        public int amount { get; set; }
        public double price { get; set; }
    
        public virtual disks disks { get; set; }
        public virtual order order { get; set; }
    }
}
