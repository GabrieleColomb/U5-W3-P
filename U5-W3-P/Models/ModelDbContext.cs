using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace U5_W3_P.Models
{
    public partial class ModelDbContext : DbContext
    {
        public ModelDbContext()
            : base("name=ModelDbContext1")
        {
        }

        public virtual DbSet<Clienti> Clienti { get; set; }
        public virtual DbSet<DettaglioOrdini> DettaglioOrdini { get; set; }
        public virtual DbSet<Ordini> Ordini { get; set; }
        public virtual DbSet<Prodotto> Prodotto { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clienti>()
                .HasMany(e => e.Ordini)
                .WithRequired(e => e.Clienti)
                .HasForeignKey(e => e.ClienteId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ordini>()
                .Property(e => e.Importo)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Ordini>()
                .HasMany(e => e.DettaglioOrdini)
                .WithOptional(e => e.Ordini)
                .HasForeignKey(e => e.OrdineId);

            modelBuilder.Entity<Prodotto>()
                .Property(e => e.Prezzo)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Prodotto>()
                .Property(e => e.TempoConsegna)
                .IsUnicode(false);

            modelBuilder.Entity<Prodotto>()
                .HasMany(e => e.DettaglioOrdini)
                .WithOptional(e => e.Prodotto)
                .HasForeignKey(e => e.ProdottoId);
        }
    }
}
