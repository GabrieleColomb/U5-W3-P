namespace U5_W3_P.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DettaglioOrdini")]
    public partial class DettaglioOrdini
    {
        [Key]
        public int IdDettaglio { get; set; }

        [StringLength(50)]
        public string Quantità { get; set; }

        public int? ProdottoId { get; set; }

        public int? OrdineId { get; set; }

        public virtual Ordini Ordini { get; set; }

        public virtual Prodotto Prodotto { get; set; }
    }
}
