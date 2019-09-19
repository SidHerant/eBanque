namespace eBanque
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;
    using System.Web.Security;

    public class eBanqueContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Compte> Comptes { get; set; }
        public DbSet<Operation> Operations { get; set; }

        public eBanqueContext()
            : base("name=eBanqueContext")
        {
            Database.SetInitializer(new eBanqueDBInitializer());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Client>()
                .HasMany<Compte>(c => c.Beneficiaires)
                .WithMany(c => c.EstBeneficiaireDe)
                .Map(b =>
                        {
                            b.MapLeftKey("ClientId");
                            b.MapRightKey("CompteId");
                            
                            b.ToTable("BeneficiairesJonction");
                        });

            modelBuilder.Entity<Client>()
                .HasMany<Compte>(c => c.Comptes)
                .WithRequired(c => c.Proprietaire)
                .Map(p => p.MapKey("ProprietaireId"))
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Compte>()
                .HasMany<Operation>(c => c.Historique)
                .WithRequired(c => c.CompteProprietaire)
                .Map(p => p.MapKey("CompteProprietaireId"));
            modelBuilder.Entity<Compte>()
                .HasMany<Operation>(c => c.Historique)
                .WithOptional(c => c.CompteLie)
                .Map(p => p.MapKey("CompteLieId"))
                .WillCascadeOnDelete(false); 

            base.OnModelCreating(modelBuilder);
        }

        public class eBanqueDBInitializer : CreateDatabaseIfNotExists<eBanqueContext>
        {
            protected override void Seed(eBanqueContext context)
            {


                // Création des Clients
                IList<Client> clients = new List<Client>();
                clients.Add(new Client()
                {
                    Id = 1,
                    Nom = "Picsou",
                    Prenom = "Balthazar",
                    Email = "balthazar.picsou@pisci.ne",
                    hashPwd = FormsAuthentication.HashPasswordForStoringInConfigFile("argent", "md5"),
                    Comptes = new List<Compte>(),
                    Beneficiaires = new List<Compte>()
                });
                clients.Add(new Client()
                {
                    Id = 2,
                    Nom = "Sus",
                    Prenom = "Cré",
                    Email = "cre.sus@antique.gr",
                    hashPwd = FormsAuthentication.HashPasswordForStoringInConfigFile("imrich", "md5"),
                    Comptes = new List<Compte>(),
                    Beneficiaires = new List<Compte>()
                });
                clients.Add(new Client()
                {
                    Id = 3,
                    Nom = "Roche",
                    Prenom = "Gav",
                    Email = "gav.roche.@victor.hugo",
                    hashPwd = FormsAuthentication.HashPasswordForStoringInConfigFile("miserable", "md5"),
                    Comptes = new List<Compte>(),
                    Beneficiaires = new List<Compte>()
                });
                clients.Add(new Client()
                {
                    Id = 4,
                    Nom = "De Sinope",
                    Prenom = "Diogène",
                    Email = "diogenelebg67@gmail.com",
                    hashPwd = FormsAuthentication.HashPasswordForStoringInConfigFile("unhomme", "md5"),
                    Comptes = new List<Compte>(),
                    Beneficiaires = new List<Compte>()
                });


                // Création des Comptes
                IList<Compte> comptes = new List<Compte>();
                // Comptes de Picsou (ID == 1)
                comptes.Add(new Compte()
                {
                    Id = 1,
                    Nom = "Compte Principale",
                    Solde = 1000000,
                    Proprietaire = clients.FirstOrDefault(c => c.Id == 1),
                    Historique = new List<Operation>()
                });
                comptes.Add(new Compte()
                {
                    Id = 2,
                    Nom = "Stock piscine",
                    Solde = 1000000,
                    Proprietaire = clients.FirstOrDefault(c => c.Id == 1),
                    Historique = new List<Operation>()
                });
                clients.FirstOrDefault(c => c.Id == 1)?.Comptes.AddRange(comptes.Where(c => c.Proprietaire.Id == 1));

                // Comptes Crésus (ID == 2)
                comptes.Add(new Compte()
                {
                    Id = 3,
                    Nom = "Budget fête",
                    Solde = 1000000,
                    Proprietaire = clients.FirstOrDefault(c => c.Id == 2),
                    Historique = new List<Operation>()
                });
                comptes.Add(new Compte()
                {
                    Id = 4,
                    Nom = "Compte Principale",
                    Solde = 1234567,
                    Proprietaire = clients.FirstOrDefault(c => c.Id == 2),
                    Historique = new List<Operation>()
                });
                comptes.Add(new Compte()
                {
                    Id = 5,
                    Nom = "Budget Donnations",
                    Solde = 1000,
                    Proprietaire = clients.FirstOrDefault(c => c.Id == 2),
                    Historique = new List<Operation>()
                });
                clients.FirstOrDefault(c => c.Id == 2)?.Comptes.AddRange(comptes.Where(c => c.Proprietaire.Id == 2));

                // Comptes Diogène (ID == 4)
                comptes.Add(new Compte()
                {
                    Id = 6,
                    Nom = "Compte Principale",
                    Solde = -150,
                    Proprietaire = clients.FirstOrDefault(c => c.Id == 4),
                    Historique = new List<Operation>()
                });
                clients.FirstOrDefault(c => c.Id == 4)?.Comptes.AddRange(comptes.Where(c => c.Proprietaire.Id == 4));

                // Ajout des comptes bénéficiaire
                clients.FirstOrDefault(c => c.Id == 2)?.Beneficiaires.Add(comptes.FirstOrDefault(c => c.Id == 1));
                clients.FirstOrDefault(c => c.Id == 2)?.Beneficiaires.Add(comptes.FirstOrDefault(c => c.Id == 6));
                clients.FirstOrDefault(c => c.Id == 4)?.Beneficiaires.Add(comptes.FirstOrDefault(c => c.Id == 4));



                // Création de l'historique des opérations
                IList<Operation> operations = new List<Operation>();
                operations.Add(new Operation()
                {
                    Id = 1,
                    Date = new DateTime(2012, 12, 21, 12, 12, 00),
                    Montant = 1000,
                    CompteProprietaire = comptes.FirstOrDefault(c => c.Id == 1),
                    CompteLie = comptes.FirstOrDefault(c => c.Id == 3),
                    Label = "Participation à la récéption de l'apocalypse",
                    Type = TypeOperation.Virement
                });

                operations.Add(new Operation()
                {
                    Id = 2,
                    Date = new DateTime(2018, 01, 01, 12, 00, 00),
                    Montant = 500,
                    CompteProprietaire = comptes.FirstOrDefault(c => c.Id == 2),
                    CompteLie = comptes.FirstOrDefault(c => c.Id == 1),
                    Label = "1er ajout pour la piscine",
                    Type = TypeOperation.Virement
                });
                operations.Add(new Operation()
                {
                    Id = 3,
                    Date = new DateTime(2018, 02, 01, 12, 00, 00),
                    Montant = 500,
                    CompteProprietaire = comptes.FirstOrDefault(c => c.Id == 2),
                    CompteLie = comptes.FirstOrDefault(c => c.Id == 1),
                    Label = "2eme ajout pour la piscine",
                    Type = TypeOperation.Virement
                });
                operations.Add(new Operation()
                {
                    Id = 4,
                    Date = new DateTime(2018, 03, 01, 12, 00, 00),
                    Montant = 500,
                    CompteProprietaire = comptes.FirstOrDefault(c => c.Id == 2),
                    CompteLie = comptes.FirstOrDefault(c => c.Id == 1),
                    Label = "3eme ajout pour la piscine",
                    Type = TypeOperation.Virement
                });
                operations.Add(new Operation()
                {
                    Id = 5,
                    Date = new DateTime(2018, 04, 01, 12, 00, 00),
                    Montant = 5000,
                    CompteProprietaire = comptes.FirstOrDefault(c => c.Id == 2),
                    CompteLie = comptes.FirstOrDefault(c => c.Id == 1),
                    Label = "4eme ajout pour la piscine",
                    Type = TypeOperation.Virement
                });

                operations.Add(new Operation()
                {
                    Id = 6,
                    Date = new DateTime(2019, 12, 01, 12, 00, 00),
                    Montant = 50,
                    CompteProprietaire = comptes.FirstOrDefault(c => c.Id == 5),
                    CompteLie = comptes.FirstOrDefault(c => c.Id == 6),
                    Label = "Charité divers",
                    Type = TypeOperation.Virement
                });

                operations.Add(new Operation()
                {
                    Id = 7,
                    Date = new DateTime(2019, 10, 1, 12, 00, 00),
                    Montant = 20,
                    CompteProprietaire = comptes.FirstOrDefault(c => c.Id == 3),
                    Type = TypeOperation.Retrait
                });

                operations.Add(new Operation()
                {
                    Id = 8,
                    Date = new DateTime(2019, 10, 1, 12, 05, 00),
                    Montant = 20,
                    CompteProprietaire = comptes.FirstOrDefault(c => c.Id == 3),
                    Type = TypeOperation.Depot
                });



                context.Clients.AddRange(clients);
                context.Comptes.AddRange(comptes);
                context.Operations.AddRange(operations);
                base.Seed(context);
            }
        }
        public static eBanqueContext Create()
        {
            return new eBanqueContext();
        }
    }


    public class Client
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nom { get; set; }

        [Required]
        [MaxLength(50)]
        public string Prenom { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        public string hashPwd { get; set; }

        public virtual List<Compte> Comptes { get; set; }

        public virtual List<Compte> Beneficiaires { get; set; }
    }
    
    public class Compte
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Solde { get; set; }

        [MaxLength(100)]
        public string Nom { get; set; }

        public virtual Client Proprietaire { get; set; }

        public virtual List<Client> EstBeneficiaireDe { get; set; }

        public virtual List<Operation> Historique { get; set; }
    }

    public enum TypeOperation
    {
        Depot,
        Retrait,
        Virement
    }
    public class Operation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Montant { get; set; }

        [Required]
        public TypeOperation Type { get; set; }

        [MaxLength(255)]
        public string Label { get; set; }
        
        public virtual Compte CompteProprietaire { get; set; }

        public virtual Compte CompteLie { get; set; }
    }
}