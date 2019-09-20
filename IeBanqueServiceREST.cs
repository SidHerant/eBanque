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
        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/{compteId}/Solde",
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        CompteDTO GetSolde(string mail, string hashPwd, string compteId);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/{compteId}/Retrait",
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Retrait(string mail, string hashPwd, string compteId, int montant);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/{compteId}/Depot",
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Depot(string mail, string hashPwd, string compteId, int montant);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/OuvrirCompte",
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        void OuvrirCompte(string mail, string hashPwd, string name);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/{compteId}/CloturerCompte",
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        void CloturerCompte(string mail, string hashPwd, string compteId);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/addBeneficiaire",
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        void addBeneficiaire(string mail, string hashPwd, int compteId);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/{compteFromRaw}/Virement",
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Virement(string mail, string hashPwd, string compteFromRaw, int compteTo, int montant);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/{compteId}/Historique",
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<OperationDTO> Historique(string mail, string hashPwd, string compteId);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/SignIn",
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        void SignIn(string mail, string hashPwd, string nom, string prenom);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/LogIn",
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        void LogIn(string mail, string hashPwd);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/",
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        ClientDTO GetClient(string mail, string hashPwd);

        /// /////////////////////
        /*
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/GetData",
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Client> GetData();*/
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

        public ClientDTO(Client client)
        {
            Id = client.Id;
            Nom = client.Nom;
            Prenom = client.Prenom;
            Email = client.Email;
            Comptes = client.Comptes.Select(c => new CompteDTO(c)).ToList();
            Beneficiaires = client.Beneficiaires.Select(b => new CompteLieDTO(b)).ToList();
        }

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

        public CompteDTO(Compte compte)
        {
            Id = compte.Id;
            Nom = compte.Nom;
            Solde = compte.Solde;
            Historique = compte.Historique.Select(o => new OperationDTO(o)).ToList();
        }
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

        public OperationDTO(Operation operation)
        {
            Id = operation.Id;
            Date = operation.Date;
            Label = operation.Label;
            Montant = operation.Montant;
            Type = operation.Type;
            CompteLie = new CompteLieDTO(operation.CompteLie);
        }
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

        public CompteLieDTO(Compte compte)
        {
            NomCompte = compte.Nom;
            NomProprietaire = compte.Proprietaire.Nom;
            PrenomProprietaire = compte.Proprietaire.Prenom;
        }
    }
}
