using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Security;

namespace eBanque
{
    public class eBanqueService : IeBanqueServiceREST
    {

        public List<Client> GetData()
        {
            
            using (eBanqueContext ctx = new eBanqueContext())
            {
                var clients = ctx.Clients.ToList();
                var comptes = ctx.Comptes.ToList();
                clients.Add(new Client { });

                return clients;
            }
        }
        public ClientDTO GetClient(string id)
        {
            int ID = Int32.Parse(id);
            ClientDTO clientDTO;
            using (eBanqueContext ctx = new eBanqueContext())
            {
                Client client = ctx.Clients.FirstOrDefault(c => c.Id == ID);
                clientDTO = new ClientDTO()
                {
                    Id = client.Id,
                    Nom = client.Nom,
                    Prenom = client.Prenom,
                    Email = client.Email,
                    Comptes = client.Comptes.Select(c => new CompteDTO
                    {
                        Id = c.Id,
                        Nom = c.Nom,
                        Solde = c.Solde,
                        Historique = c.Historique.Select(o => new OperationDTO
                        {
                            Id = o.Id,
                            Date = o.Date,
                            Label = o.Label,
                            Montant = o.Montant,
                            Type = o.Type,
                            CompteLie = new CompteLieDTO
                            {
                                NomCompte = o.CompteLie.Nom,
                                NomProprietaire = o.CompteLie.Proprietaire.Nom,
                                PrenomProprietaire = o.CompteLie.Proprietaire.Prenom
                            }
                        }).ToList()
                    }).ToList(),
                    Beneficiaires = client.Beneficiaires.Select(b => new CompteLieDTO
                    {
                        NomCompte = b.Nom,
                        NomProprietaire = b.Proprietaire.Nom,
                        PrenomProprietaire = b.Proprietaire.Prenom
                    }).ToList()
                };
            }
            return clientDTO;
        }

        public string addBeneficiaire(string mail, string hashPwd, int compteId)
        {
            throw new NotImplementedException();
        }

        public string CloturerCompteCompte(string mail, string hashPwd, int compteId)
        {
            throw new NotImplementedException();
        }

        public string Depot(string mail, string hashPwd, int compteId, float montant)
        {
            throw new NotImplementedException();
        }

        public string GetSolde(string mail, string hashPwd, int compteId)
        {
            throw new NotImplementedException();
        }

        public string Historique(string mail, string hashPwd, int compteId)
        {
            throw new NotImplementedException();
        }

        public string LogIn(string mail, string hashPwd)
        {
            throw new NotImplementedException();
        }

        public string OuvrirCompte(string mail, string hashPwd)
        {
            throw new NotImplementedException();
        }

        public string Retrait(string mail, string hashPwd, int compteId, float montant)
        {
            throw new NotImplementedException();
        }

        public string SignIn(string mail, string hashPwd)
        {
            throw new NotImplementedException();
        }

        public string Virement(string mail, string hashPwd, int compteFrom, int compteTo)
        {
            throw new NotImplementedException();
        }
    }
}
