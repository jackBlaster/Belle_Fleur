using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bellefleur
{
    internal class Client
    {
        private string email;
        private string nom;
        private string prenom;
        private string telephone;
        private string adresse;
        private string mdp;
        private string carte_credit;
        private string fidelite;

        public string Email { get { return email; } set { email = value; } }
        public string Nom { get { return nom; } set { nom = value; } }
        public string Prenom { get { return prenom; } set { prenom = value; } }
        public string Telephone { get { return telephone; } set { telephone = value; } }
        public string Adresse { get { return adresse; } set { adresse = value; } }
        public string Mdp { get { return mdp; } set { mdp = value; } }
        public string Carte_credit { get { return carte_credit; } set { carte_credit = value; } }
        public string Fidelite { get { return fidelite; } set { fidelite = value; } }

        public Client(string email, string nom, string prenom, string telephone, string adresse, string mdp, string carte_credit=null, string fidelite=null)
        {
            this.email = email;
            this.nom = nom;
            this.prenom = prenom;
            this.telephone = telephone;
            this.adresse = adresse;
            this.mdp = mdp;
            this.carte_credit = carte_credit;
            this.fidelite = fidelite;
        }
        public Client(): this("N/C", "N/C", "N/C", "N/C", "N/C", "N/C", "N/C", "N/C")
        { }

        public string ToString()
        {
            string client = "=====================================================" +
                            "\nEMAIL : " + this.email
                            +"\nNOM :" + this.nom +
                            "\nPRENOM : " + this.prenom +
                            "\nTELEPHONE : " + this.telephone +
                            "\nADRESSE : " + this.adresse +
                            "\nFIDELITE : " + this.fidelite +
                            "\n=====================================================" ;
            return client;
        }
    }
}
