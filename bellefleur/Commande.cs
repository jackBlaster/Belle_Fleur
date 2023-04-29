using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bellefleur
{
    internal class Commande
    {
        private int id_commande;
        private string adresse_livraison;
        private DateTime date_commande;
        private DateTime date_livraison;
        private string message;
        private float prix;
        private string statut;
        private string email;
        private string mdp;
        private string nom_bouquet;

        public int Id_commande { get { return id_commande; } set { id_commande = value; } }
        public string Adresse_livraison { get { return adresse_livraison; } set { adresse_livraison = value; } }
        public DateTime Date_commande { get { return date_commande; } set { date_commande = value; } }
        public DateTime Date_livraison { get { return date_livraison; } set { date_livraison = value; } }
        public string Message { get { return message; } set { message = value; } }
        public float Prix { get { return prix; } set { prix = value; } }
        public string Statut { get { return statut; } set { statut = value; } }
        public string Email { get { return email; } set { email = value; } }
        public string Mdp { get { return mdp; } set { mdp = value; } }
        public string Nom_bouquet { get { return nom_bouquet; } set { nom_bouquet = value; } }

        public Commande(int id_commande, string adresse_livraison, DateTime date_commande, DateTime date_livraison, string message, float prix, string statut, string email, string mdp, string nom_bouquet)
        {
            this.id_commande = id_commande;
            this.adresse_livraison = adresse_livraison;
            this.date_commande = date_commande;
            this.date_livraison = date_livraison;
            this.message = message;
            this.prix = prix;
            this.statut = statut;
            this.email = email;
            this.mdp = mdp;
            this.nom_bouquet = nom_bouquet;
        }
    }
}
