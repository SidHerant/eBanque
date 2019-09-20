using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Security;

namespace eBanque
{
    public class eBanqueService : IeBanqueServiceREST
    {
        public const int decouvertPossible = -300;
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
        public ClientDTO GetClient(string mail, string hashPwd)
        {
            ClientDTO clientDTO = null;
            WebOperationContext context = WebOperationContext.Current;
            int id = isAutenticated(mail, hashPwd);
            if (id > 0)
            {
                using (eBanqueContext ctx = new eBanqueContext())
                {
                    Client client = ctx.Clients.FirstOrDefault(c => c.Id == id);
                    clientDTO = new ClientDTO(client);
                    context.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                    context.OutgoingResponse.StatusDescription = "Utilisateur éxistant";
                }
            }
            else
            {
                context.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                context.OutgoingResponse.StatusDescription = "Utilisateur introuvable";
            }
            return clientDTO;
        }

        public CompteDTO GetSolde(string mail, string hashPwd, string compteId)
        {
            CompteDTO compte = null;
            WebOperationContext context = WebOperationContext.Current;
            int compteIdInt;
            if (!Int32.TryParse(compteId, out compteIdInt))
            {
                context.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                context.OutgoingResponse.StatusDescription = "Probleme numero de compte";
                return null;
            }
            int id = isAutenticated(mail, hashPwd);
            if (id > 0)
            {
                using (eBanqueContext dbContext = new eBanqueContext())
                {
                    compte = new CompteDTO(dbContext.Comptes.FirstOrDefault(c => c.Proprietaire.Id == id && c.Id == compteIdInt));
                }
                if (compte==null)
                {
                    context.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
                    context.OutgoingResponse.StatusDescription = "Compte introuvable";
                }
            }
            else
            {
                context.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                context.OutgoingResponse.StatusDescription = "Utilisateur introuvable";
            }
            return compte;
        }

        public void Retrait(string mail, string hashPwd, string compteId, int montant)
        {
            Compte compte = null;
            WebOperationContext context = WebOperationContext.Current;
            int compteIdInt;
            if (!Int32.TryParse(compteId, out compteIdInt))
            {
                context.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                context.OutgoingResponse.StatusDescription = "Probleme numero de compte";
                return;
            }
            int id = isAutenticated(mail, hashPwd);
            if (id > 0)
            {
                using (eBanqueContext dbContext = new eBanqueContext())
                {
                    compte = dbContext.Comptes.FirstOrDefault(c => c.Proprietaire.Id == id && c.Id == compteIdInt);
                    if (compte == null)
                    {
                        context.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
                        context.OutgoingResponse.StatusDescription = "Compte introuvable";
                    }
                    else
                    {
                        if (compte.Solde - montant < decouvertPossible)
                        {
                            context.OutgoingResponse.StatusCode = HttpStatusCode.NotAcceptable;
                            context.OutgoingResponse.StatusDescription = "Fonds insufisant";
                        }
                        else
                        {
                            compte.Solde -= montant;
                            compte.Historique.Add(new Operation
                            {
                                Date = DateTime.Today,
                                Montant = montant,
                                Type = TypeOperation.Retrait,
                                CompteProprietaire = compte
                            });
                            dbContext.SaveChanges();
                            context.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                            context.OutgoingResponse.StatusDescription = "Retrait de " + montant + "€ effectué";
                        }
                    }
                }
            }
            else
            {
                context.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                context.OutgoingResponse.StatusDescription = "Utilisateur introuvable";
            }
        }

        public void Depot(string mail, string hashPwd, string compteId, int montant)
        {
            Compte compte = null;
            WebOperationContext context = WebOperationContext.Current;
            int compteIdInt;
            if (!Int32.TryParse(compteId, out compteIdInt))
            {
                context.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                context.OutgoingResponse.StatusDescription = "Probleme numero de compte";
                return;
            }
            int id = isAutenticated(mail, hashPwd);
            if (id > 0)
            {
                using (eBanqueContext dbContext = new eBanqueContext())
                {
                    compte = dbContext.Comptes.FirstOrDefault(c => c.Proprietaire.Id == id && c.Id == compteIdInt);
                    if (compte == null)
                    {
                        context.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
                        context.OutgoingResponse.StatusDescription = "Compte introuvable";
                    }
                    else
                    {
                        compte.Solde += montant;
                        dbContext.SaveChanges();
                        compte.Historique.Add(new Operation
                        {
                            Date = DateTime.Today,
                            Montant = montant,
                            Type = TypeOperation.Depot,
                            CompteProprietaire = compte
                        });
                        context.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                        context.OutgoingResponse.StatusDescription = "Depot de " + montant + "€ effectué";
                    }
                }
            }
            else
            {
                context.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                context.OutgoingResponse.StatusDescription = "Utilisateur introuvable";
            }
        }

        public void OuvrirCompte(string mail, string hashPwd, string name)
        {
            Compte compte = null;
            WebOperationContext context = WebOperationContext.Current;
            int id = isAutenticated(mail, hashPwd);
            if (id > 0)
            {
                using (eBanqueContext dbContext = new eBanqueContext())
                {
                    compte = dbContext.Comptes.FirstOrDefault(c => c.Proprietaire.Id == id && c.Nom == name);
                    if (compte != null)
                    {
                        context.OutgoingResponse.StatusCode = HttpStatusCode.Found;
                        context.OutgoingResponse.StatusDescription = "Nom de compte déja éxistant";
                    }
                    else
                    {
                        dbContext.Comptes.Add(new Compte()
                        {
                            Nom = "name",
                            Solde = 0,
                            Proprietaire = dbContext.Clients.FirstOrDefault(c => c.Id == id),
                        });
                        dbContext.SaveChanges();
                        context.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                        context.OutgoingResponse.StatusDescription = "Compte ajouté";
                    }
                }
            }
            else
            {
                context.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                context.OutgoingResponse.StatusDescription = "Utilisateur introuvable";
            }
        }

        public void CloturerCompte(string mail, string hashPwd, string compteId)
        {
            Compte compte = null;
            WebOperationContext context = WebOperationContext.Current;
            int compteIdInt;
            if (!Int32.TryParse(compteId, out compteIdInt))
            {
                context.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                context.OutgoingResponse.StatusDescription = "Probleme numero de compte";
                return;
            }
            int id = isAutenticated(mail, hashPwd);
            if (id > 0)
            {
                using (eBanqueContext dbContext = new eBanqueContext())
                {
                    compte = dbContext.Comptes.FirstOrDefault(c => c.Id == compteIdInt);
                    if (compte == null)
                    {
                        context.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
                        context.OutgoingResponse.StatusDescription = "Compte introuvable";
                    }
                    else
                    {
                        dbContext.Comptes.Remove(compte);
                        dbContext.SaveChanges();
                        context.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                        context.OutgoingResponse.StatusDescription = "Compte cloturé";
                    }
                }
            }
            else
            {
                context.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                context.OutgoingResponse.StatusDescription = "Utilisateur introuvable";
            }
        }

        public void addBeneficiaire(string mail, string hashPwd, int compteId)
        {
            Compte compte = null;
            WebOperationContext context = WebOperationContext.Current;
            int id = isAutenticated(mail, hashPwd);
            if (id > 0)
            {
                using (eBanqueContext dbContext = new eBanqueContext())
                {
                    compte = dbContext.Comptes.FirstOrDefault(c => c.Id == compteId);
                    if (compte == null)
                    {
                        context.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
                        context.OutgoingResponse.StatusDescription = "Compte introuvable";
                    }
                    else
                    {
                        if (compte.Proprietaire.Id == id)
                        {
                            context.OutgoingResponse.StatusCode = HttpStatusCode.PreconditionFailed;
                            context.OutgoingResponse.StatusDescription = "Impossible d'ajouter son propre compte comme Bénéficiaire.";
                        }
                        else
                        {
                            dbContext.Clients.FirstOrDefault(c => c.Id == id).Beneficiaires.Add(compte);
                            dbContext.SaveChanges();

                            context.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                            context.OutgoingResponse.StatusDescription = "Bénéficiaire ajouté";
                        }
                    }
                }
            }
            else
            {
                context.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                context.OutgoingResponse.StatusDescription = "Utilisateur introuvable";
            }
        }

        public void Virement(string mail, string hashPwd, string compteFromRaw, int compteTo, int montant)
        {
            Compte cmtFrom = null;
            Compte cmtTo = null;
            WebOperationContext context = WebOperationContext.Current;
            int compteFrom;
            if (!Int32.TryParse(compteFromRaw, out compteFrom))
            {
                context.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                context.OutgoingResponse.StatusDescription = "Probleme numero de compte";
                return;
            }
            int id = isAutenticated(mail, hashPwd);
            if (id > 0)
            {
                using (eBanqueContext dbContext = new eBanqueContext())
                {
                    cmtFrom = dbContext.Comptes.FirstOrDefault(c => c.Id == compteFrom);
                    cmtTo = dbContext.Comptes.FirstOrDefault(c => c.Id == compteTo);
                    if (cmtFrom == null || cmtTo == null)
                    {
                        context.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
                        context.OutgoingResponse.StatusDescription = "Compte introuvable";
                    }
                    else
                    {
                        if (cmtFrom.Proprietaire.Id == id && cmtFrom.Proprietaire.Beneficiaires.Any(c => c.Id == cmtTo.Id))
                        {
                            if (cmtFrom.Solde - montant < decouvertPossible)
                            {
                                context.OutgoingResponse.StatusCode = HttpStatusCode.NotAcceptable;
                                context.OutgoingResponse.StatusDescription = "Fonds insufisant";
                            }
                            else
                            {
                                cmtFrom.Solde -= montant;
                                cmtTo.Solde += montant;
                                cmtFrom.Historique.Add(new Operation
                                {
                                    Date = DateTime.Today,
                                    Montant = montant,
                                    Type = TypeOperation.Virement,
                                    CompteProprietaire = cmtFrom,
                                    CompteLie = cmtTo
                                });
                                dbContext.SaveChanges();
                                context.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                                context.OutgoingResponse.StatusDescription = "Virement de " + montant + "€ effectué";
                            }
                        }
                        else
                        {
                            context.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                            context.OutgoingResponse.StatusDescription = "Vous n'avez pas l'autorisation d'effectuer ce virement";
                        }
                    }
                }
            }
            else
            {
                context.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                context.OutgoingResponse.StatusDescription = "Utilisateur introuvable";
            }
        }

        public List<OperationDTO> Historique(string mail, string hashPwd, string compteId)
        {
            Compte compte = null;
            List<OperationDTO> historique = null;
            WebOperationContext context = WebOperationContext.Current;
            int compteIdInt;
            if (!Int32.TryParse(compteId, out compteIdInt))
            {
                context.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                context.OutgoingResponse.StatusDescription = "Probleme numero de compte";
                return null;
            }
            int id = isAutenticated(mail, hashPwd);
            if (id > 0)
            {
                using (eBanqueContext dbContext = new eBanqueContext())
                {
                    compte = dbContext.Comptes.FirstOrDefault(c => c.Id == compteIdInt);
                    if (compte == null)
                    {
                        context.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
                        context.OutgoingResponse.StatusDescription = "Compte introuvable";
                    }
                    else
                    {
                        if (compte.Proprietaire.Id != id)
                        {
                            context.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                            context.OutgoingResponse.StatusDescription = "Vous n'avez pas l'autorisation d'effectuer ce virement";
                        }
                        else
                        {
                            historique = compte.Historique.Select(o => new OperationDTO(o)).ToList();
                        }
                    }
                }
            }
            else
            {
                context.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                context.OutgoingResponse.StatusDescription = "Utilisateur introuvable";
            }
            return historique;
        }

        public void SignIn(string mail, string hashPwd, string nom, string prenom)
        {
            WebOperationContext context = WebOperationContext.Current;
            if (!string.IsNullOrEmpty(mail) && !string.IsNullOrEmpty(hashPwd) && !string.IsNullOrEmpty(nom) && !string.IsNullOrEmpty(prenom))
            {
                using (eBanqueContext dbContext = new eBanqueContext())
                {
                    if (dbContext.Clients.Any(c => c.Email == mail))
                    {
                        context.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                        context.OutgoingResponse.StatusDescription = "Mail déja utilisé";
                    }
                    else
                    {
                        dbContext.Clients.Add(new Client
                        {
                            Email = mail,
                            hashPwd = hashPwd,
                            Nom = nom,
                            Prenom = prenom
                        });
                        dbContext.SaveChanges();
                        context.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                        context.OutgoingResponse.StatusDescription = "Client créé";
                    }
                }
            }
        }

        public void LogIn(string mail, string hashPwd)
        {
            WebOperationContext context = WebOperationContext.Current;
            int id = isAutenticated(mail, hashPwd);
            if (id > 0)
            {
                context.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                context.OutgoingResponse.StatusDescription = "Utilisateur existant";
            }
            else
            {
                context.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                context.OutgoingResponse.StatusDescription = "Utilisateur introuvable";
            }
        }


        private int isAutenticated(string mail, string hashPwd)
        {
            Client client;
            using (eBanqueContext ctx = new eBanqueContext())
            {
                client = ctx.Clients.FirstOrDefault(c => string.Compare(c.Email, mail, true) == 0 && c.hashPwd == hashPwd);
            }
            return client?.Id ?? 0; 
        }
    }
}
