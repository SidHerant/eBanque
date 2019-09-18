using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

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

                return ctx.Clients.ToList();
            }
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
