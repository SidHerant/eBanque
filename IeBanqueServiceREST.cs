using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace eBanque
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
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
    }


    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
