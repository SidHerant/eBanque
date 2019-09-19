using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace eBanque
{
    [ServiceContract]
    public interface IeBanqueServiceREST
    {
        /*
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/Solde",
            ResponseFormat = WebMessageFormat.Json)]
        string GetSolde(string mail, string hashPwd, int compteId);


        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/Retrait",
            ResponseFormat = WebMessageFormat.Json)]
        string Retrait(string mail, string hashPwd, int compteId, float montant);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/Depot",
            ResponseFormat = WebMessageFormat.Json)]
        string Depot(string mail, string hashPwd, int compteId, float montant);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/OuvrirCompte",
            ResponseFormat = WebMessageFormat.Json)]
        string OuvrirCompte(string mail, string hashPwd);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/CloturerCompte",
            ResponseFormat = WebMessageFormat.Json)]
        string CloturerCompteCompte(string mail, string hashPwd, int compteId);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/addBeneficiaire",
            ResponseFormat = WebMessageFormat.Json)]
        string addBeneficiaire(string mail, string hashPwd, int compteId);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/Virement",
            ResponseFormat = WebMessageFormat.Json)]
        string Virement(string mail, string hashPwd, int compteFrom, int compteTo);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/Historique",
            ResponseFormat = WebMessageFormat.Json)]
        string Historique(string mail, string hashPwd, int compteId);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/SignIn",
            ResponseFormat = WebMessageFormat.Json)]
        string SignIn(string mail, string hashPwd);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/LogIn",
            ResponseFormat = WebMessageFormat.Json)]
        string LogIn(string mail, string hashPwd);
        */

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/GetData",
            ResponseFormat = WebMessageFormat.Json)]
        List<Client> GetData();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/GetData/{id}",
            ResponseFormat = WebMessageFormat.Json)]
        ClientDTO GetClient(string id);
    }


    [DataContract]
    public class ClientDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Nom { get; set; }

        [DataMember]
        public string Prenom { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public List<CompteDTO> Comptes { get; set; }

        [DataMember]
        public List<CompteLieDTO> Beneficiaires { get; set; }
    }

    [DataContract]
    public class CompteDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int Solde { get; set; }

        [DataMember]
        public string Nom { get; set; }

        [DataMember]
        public List<OperationDTO> Historique { get; set; }
    }

    [DataContract]
    public class OperationDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public int Montant { get; set; }

        [DataMember]
        public TypeOperation Type { get; set; }

        [DataMember]
        public string Label { get; set; }

        [DataMember]
        public CompteLieDTO CompteLie { get; set; }
    }

    [DataContract]
    public class CompteLieDTO
    {
        [DataMember]
        public string NomProprietaire { get; set; }

        [DataMember]
        public string PrenomProprietaire { get; set; }

        [DataMember]
        public string NomCompte { get; set; }
    }
}
