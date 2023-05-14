using MySql.Data;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Security;
using System.Configuration;
using System.Runtime.ExceptionServices;
using System.Xml;
using bellefleur;
using MySqlX.XDevAPI;
using System.Text.RegularExpressions;
using System;
using Org.BouncyCastle.Tls.Crypto;
using System.Reflection;
using System.Xml.Linq;
using Newtonsoft.Json;
using Microsoft.VisualBasic;


//----INITIALISATION----//
Console.WriteLine("||----| Bienvenue dans la base de donnée Belle Fleur |----||\n           Veuillez appuyer sur une touche...");
Console.ReadKey();
Console.Clear();

//----CONNEXION AVEC LES IDENTIFIANTS----//
Dictionary<string, string> valid_ids = User.Users;

Console.Write("Veuillez taper votre identifiant : ");
string username = Console.ReadLine();
while (!valid_ids.ContainsKey(username))
{
    Console.Clear();
    Console.Write("Veuillez saisir un identifiant valide : ");
    username = Console.ReadLine();
}
Console.Clear();

Console.Write("Veuillez taper votre mot de passe : ");
string mdp = Console.ReadLine();
while (!(valid_ids.TryGetValue(username, out string true_mdp)&& mdp==true_mdp) )
{
    Console.Clear();
    Console.Write("Veuillez saisir un mot de passe valide : ");
    mdp = Console.ReadLine();
}
Console.Clear();  
string connectionString = null;
MySqlConnection connection = null; 

try
{
    connectionString = "SERVER=localhost;PORT=3301;database=fleurs;UID="+username+";PASSWORD="+mdp+";";
    connection = new MySqlConnection(connectionString);
    connection.Open();
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}


//PREMIER MENU POUR Root/root
bool deco = false;
while (!deco)
{
    Console.Clear();
    Console.WriteLine("Que voulez-vous faire ?\n" +
                       "1-Lire la base de donnée\n" +
                       "2-Ajouter un élément\n" +
                       "3-Modifier un élément\n" +
                       "4-Voir les statistiques\n" +
                       "5-Exporter un fichier XML\n" +
                       "6-Exporter un fichier JSON\n" +
                       "7-Déconnexion");
    Console.Write("Selectionner votre action(1 à 7) : ");
    int choice1=0;
    try { choice1 = Convert.ToInt32(Console.ReadLine()); }
    catch(Exception ex) { Console.WriteLine("Non valide"); }
    while (!(choice1 is int) && (choice1 < 1 || choice1 > 7))
    {
        Console.WriteLine("Veuillez saisir un choix valide");
        choice1 = Convert.ToInt32(Console.ReadLine());
    }
    switch (choice1)
    {
        case 1:
            Console.Clear();
            Lecture_DB(connection);
            break;
        case 2:
            Ajout_DB(connection);
            break;
        case 3:
            Modifier_DB(connection);
            break;
        case 4:
            Show_Stats(connection);
            break;
        case 5:
            Export_XML(connection);
            break;
        case 6:
            Export_JSON(connection);
            break;
        case 7://Deconnexion
            deco = true;
            break;
        default: break;
    }
}
connection.Close();

//FINI
//Fonction permettant de lire les éléments de la database
//switch-case pour choisir quelle table lire
static void Lecture_DB(MySqlConnection connection)
{
    bool leave = false;
    while (!leave)
    {
        Console.Clear();
        Console.WriteLine("Que voulez-lire ? : \n" +
            "1-Liste de clients\n" +
            "2-Liste des commandes\n" +
            "3-Stocks\n" +
            "4-Quitter");
        int choice1 = 0;
        try { choice1 = Convert.ToInt32(Console.ReadLine()); }
        catch (Exception ex) { Console.WriteLine("Non valide"); }
        while (!(choice1 is int) && (choice1 < 1 || choice1 > 7))
        {
            Console.WriteLine("Veuillez saisir un choix valide");
            choice1 = Convert.ToInt32(Console.ReadLine());
        }
        switch (choice1)
        {
            case 1:
                //clients
                Console.Clear();
                Console.WriteLine("Voici la liste des clients enregistrés : ");
                MySqlCommand command_client = connection.CreateCommand();
                command_client.CommandText = "SELECT * from fleurs.client;";

                MySqlDataReader reader_client;
                reader_client = command_client.ExecuteReader();

                    
                while (reader_client.Read())
                {
                    string[] currentRowAsString = new string[8];
                    for (int i = 0; i < reader_client.FieldCount; i++)
                    {

                        if (reader_client.GetValue(i).ToString() != "")
                        {
                            string valueAsString = reader_client.GetValue(i).ToString();
                            currentRowAsString[i] = valueAsString;
                        }
                    }
                    bellefleur.Client c = new bellefleur.Client(currentRowAsString[0], currentRowAsString[2], currentRowAsString[3], currentRowAsString[4], currentRowAsString[5], currentRowAsString[7]);
                    Console.WriteLine(c.ToString());
                }
                reader_client.Close();
                Console.WriteLine("\nCliquez sur une touche pour continuer...");
                Console.ReadKey();
                break;
            case 2:
                //Liste commandes
                Console.Clear();
                Console.WriteLine("Voici la liste des commandes enregistrées : ");
                MySqlCommand command_cmd = connection.CreateCommand();
                command_cmd.CommandText = "SELECT * from fleurs.commande;";

                MySqlDataReader reader_cmd;
                reader_cmd = command_cmd.ExecuteReader();

                while (reader_cmd.Read())
                {
                    string[] currentRowAsString = new string[10];
                    for (int i = 0; i < reader_cmd.FieldCount; i++)
                    {
                        if (reader_cmd.GetValue(i).ToString() != "")
                        {
                            string valueAsString = reader_cmd.GetValue(i).ToString();
                            currentRowAsString[i] = valueAsString;
                        }
                    }
                    Commande cmd = new Commande(Convert.ToInt32(currentRowAsString[0]), currentRowAsString[1], Convert.ToDateTime(currentRowAsString[2]), Convert.ToDateTime(currentRowAsString[3]),
                                                                currentRowAsString[4], Single.Parse(currentRowAsString[5]), currentRowAsString[6], currentRowAsString[7], currentRowAsString[8],
                                                                currentRowAsString[9]);
                    Console.WriteLine(cmd.ToString());
                }
                reader_cmd.Close();
                Console.WriteLine("\nCliquez sur une touche pour continuer...");
                Console.ReadKey();
                break;
            case 3:
                //Stocks
                Console.Clear();
                Console.WriteLine("Voici le stock de fleurs : ");
                MySqlCommand command_fleur = connection.CreateCommand();
                command_fleur.CommandText = "SELECT id_fleur,stock from fleurs.fleur;";

                MySqlDataReader reader_fleur;
                reader_fleur = command_fleur.ExecuteReader();

                while (reader_fleur.Read())
                {
                    string currentRowAsString = "";
                    for (int i = 0; i < reader_fleur.FieldCount; i++)
                    {
                        if (reader_fleur.GetValue(i).ToString() != "")
                        {
                            string valueAsString = reader_fleur.GetValue(i).ToString();
                            currentRowAsString += valueAsString + ", ";
                        }
                    }
                    Console.WriteLine(currentRowAsString);
                }
                reader_fleur.Close();

                Console.WriteLine();
                Console.WriteLine("Voici le stock d'accessoire : ");
                MySqlCommand command_acc = connection.CreateCommand();
                command_acc.CommandText = "SELECT id_accessoire,stock from fleurs.accessoire;";

                MySqlDataReader reader_acc;
                reader_acc = command_acc.ExecuteReader();

                while (reader_acc.Read())
                {
                    string currentRowAsString = "";
                    for (int i = 0; i < reader_acc.FieldCount; i++)
                    {
                        if (reader_acc.GetValue(i).ToString() != "")
                        {
                            string valueAsString = reader_acc.GetValue(i).ToString();
                            currentRowAsString += valueAsString + ", ";
                        }
                    }
                    Console.WriteLine(currentRowAsString);
                }
                reader_acc.Close();
                Console.WriteLine("\nCliquez sur une touche pour continuer...");
                Console.ReadKey();
                break;
            case 4:
                leave = true;
                break;
            default: break;
        }
        

    }

}

//A FAIRE
//Fonction permettant d'ajouter des éléments à la database via des sous-fonctions plus spécifiques
static void Ajout_DB(MySqlConnection connection)
{
    Console.Clear(); 
    Console.WriteLine("Que voulez-vous faire ?" +
        "\n1-Ajouter un client\n2-Ajouter une commande\n3-Ajouter un bouquet au catalogue\n4-Quitter\nVotre choix : ");
    int choice1 = 0;
    try { choice1 = Convert.ToInt32(Console.ReadLine()); }
    catch (Exception ex) { Console.WriteLine("Non valide"); }
    while (!(choice1 is int) && (choice1 < 1 || choice1 > 7))
    {
        Console.WriteLine("Veuillez saisir un choix valide");
        choice1 = Convert.ToInt32(Console.ReadLine());
    }
    switch (choice1)
    {
        case 1:
            //Ajout clients
            Ajout_clients(connection);
            break;
        case 2:
            //Ajout commandes
            Ajout_cmd(connection);
            break;
        case 3:
            //Ajout bouquet type
            break;
        case 4:
            //Quitter
            break;
        default: break;
    }
}

//Sous-fonction liée à Ajout_DB() : permet d'ajouter un client
static void Ajout_clients(MySqlConnection connection)
{
    Console.Clear();
    Console.WriteLine("Veuillez renseigner les éléments du client :");
    Console.WriteLine("email : "); string email = Console.ReadLine();
    Console.WriteLine("nom : "); string nom = Console.ReadLine();
    Console.WriteLine("prenom : "); string prenom = Console.ReadLine();
    Console.WriteLine("tel : "); string tel = Console.ReadLine();
    Console.WriteLine("adresse : "); string adresse = Console.ReadLine();
    Console.WriteLine("mdp : "); string mdp = Console.ReadLine();
    bellefleur.Client new_client = new bellefleur.Client(email, nom, prenom, tel, adresse, mdp);

    MySqlCommand command_client = connection.CreateCommand();
    command_client.CommandText = "INSERT INTO Fleurs.client (email, mdp, nom, prenom, telephone, adresse_facturation, carte_credit, fidelite)" +
                                 " VALUES ('"+new_client.Email+"','"+new_client.Mdp+"','"+ new_client.Nom+"','" + new_client.Prenom + "','" + new_client.Telephone + "','" + new_client.Adresse +
                                 "','" + new_client.Carte_credit + "','" + new_client.Fidelite+"');";
    command_client.ExecuteNonQuery();
    Console.WriteLine("Le client a été enregistré\nCliquez pour continuer...");
    Console.ReadKey();

}

//EN COURS Faire en sorte que les stocks changent
//Sous-fonction liée à Ajout_DB() : permet d'ajouter une commande
static void Ajout_cmd(MySqlConnection connection)
{
    Console.Clear();
    Console.WriteLine("Veuillez renseigner les éléments de la commande :");
    Console.WriteLine("Adresse de livraison : "); string adresse_livraison = Console.ReadLine();
    Console.WriteLine("Date de la commande affectée à la date actuelle : "); DateTime date_commande = DateTime.Now;
    Console.WriteLine("Date de livraison (format : AAAA-MM-JJ) : "); DateTime date_livraison = DateTime.Parse(Console.ReadLine());
    Console.WriteLine("Message : "); string message = Console.ReadLine();
    Console.WriteLine("Prix : "); float prix = float.Parse(Console.ReadLine());
    Console.WriteLine("Statut : "); string statut = Console.ReadLine();
    Console.WriteLine("Email : "); string email = Console.ReadLine();
    Console.WriteLine("Mot de passe : "); string mdp = Console.ReadLine();
    Console.WriteLine("Nom du bouquet : "); string nom_bouquet = Console.ReadLine();

    MySqlCommand command_commande = connection.CreateCommand();
    command_commande.CommandText = "INSERT INTO Fleurs.Commande (adresse_livraison, date_commande, date_livraison, message, prix, statut, email, mdp, nom_bouquet)" +
    " VALUES ('" + adresse_livraison + "','" + date_commande.ToString("yyyy-MM-dd HH:mm:ss") + "','" + date_livraison.ToString("yyyy-MM-dd") + "','" + message +
    "'," + prix + ",'" + statut + "','" + email + "','" + mdp + "','" + nom_bouquet + "');";
    command_commande.ExecuteNonQuery();
    Console.WriteLine("La commande a été enregistrée\nCliquez pour continuer...");
    Console.ReadKey();

}

//En cours - faire clients bouquets et stocks
//Fonction permettant de modifier des éléments de la database via des sous-fonctions plus spécifiques
static void Modifier_DB(MySqlConnection connection)
{
    Console.Clear();
    Console.WriteLine("Que voulez-vous modifier ?" +
        "\n1-Client\n2-Commande\n3-Bouquet du catalogue\n4-Quitter");
    int choice1 = 0;
    try { choice1 = Convert.ToInt32(Console.ReadLine()); }
    catch (Exception ex) { Console.WriteLine("Non valide"); }
    while (!(choice1 is int) && (choice1 < 1 || choice1 > 7))
    {
        Console.WriteLine("Veuillez saisir un choix valide");
        choice1 = Convert.ToInt32(Console.ReadLine());
    }
    switch (choice1)
    {
        case 1:
            //Modifier clients
            break;
        case 2:
            //Modifier commandes
            Modif_cmd(connection);
            break;
        case 3:
            //Modifier bouquet type
            break;
        case 4:
            //Modifier stocks
            break;
        case 5:
            //Quitter
            break;
        default: break;
    }
}

//A FAIRE
//Fonction permettant de choisir des statistiques de vente via des sous-fonctions plus spécifiques
static void Show_Stats(MySqlConnection connection)
{
    bool leave = false;
    while (!leave)
    {
        Console.Clear();
        Console.WriteLine("Quelle type de statistique voulez-vous voir ?\n1-Statistiques de ventes\n2-Statistiques sur les produits\n3-Statistiques sur les bénéfices" +
            "\n4-Statistiques sur les clients\n5-Quitter");
        int choice1 = Convert.ToInt32(Console.ReadLine());
        while (choice1 < 1 || choice1 > 7)
        {
            Console.WriteLine("Veuillez saisir un choix valide");
            choice1 = Convert.ToInt32(Console.ReadLine());
        }
        switch (choice1)
        {
            case 1:
                //Ventes
                stat_vente(connection);
                break;
            case 2:
                //Produits les plus utilisés
                stats_produits(connection);
                break;
            case 3:
                //Gains = factures - prix composants
                MySqlCommand command_client = connection.CreateCommand();
                command_client.CommandText = "select sum(prix) from commande;";

                MySqlDataReader reader_client;
                reader_client = command_client.ExecuteReader();

                while (reader_client.Read())
                {
                    string currentRowAsString = "Le total des commandes s'éleve à ";
                    for (int i = 0; i < reader_client.FieldCount; i++)
                    {
                        if (reader_client.GetValue(i).ToString() != "")
                        {
                           
                             string valueAsString = reader_client.GetValue(i).ToString();
                             currentRowAsString += valueAsString + "";
                            
                        }
                    }
                    currentRowAsString += " €.";
                    Console.WriteLine(currentRowAsString);
                }
                reader_client.Close();
                break;
            case 4:
                //Clients (plus de comandes etc...)
                break;
            case 5:
                //Quitter
                leave = true;
                break;
            default: break;
        }
    }
}

#region <Stats ventes - Sous-méthodes>
static void stat_vente(MySqlConnection connection)
{
    Console.Clear();
    Console.WriteLine("Quelle type de statistique voulez-vous voir ?\n1-Ventes par mois\n2-Ventes par année\n3-Nombre de ventes\n4-Quitter");
    bool leave = false;
    while (!leave)
    {
        int choice1 = 0;
        try { choice1 = Convert.ToInt32(Console.ReadLine()); }
        catch (Exception ex) { Console.WriteLine("Non valide"); }
        while (!(choice1 is int) && (choice1 < 1 || choice1 > 3))
        {
            Console.WriteLine("Veuillez saisir un choix valide");
            choice1 = Convert.ToInt32(Console.ReadLine());
        }
        switch (choice1)
        {
            case 1:
                stat_ventes_mois(connection);
                break;
            case 2:
                stat_ventes_annee(connection);
                break;
            case 3:
                stat_nb_ventes(connection);
                break;
            case 4:
                //Quitter
                leave = true;
                break;
            default: break;
                
        }
    }
}

static void stat_ventes_mois(MySqlConnection connection)
{
    MySqlCommand command_client = connection.CreateCommand();
    command_client.CommandText = "select MONTH(date_commande),count(*) from commande group by MONTH(date_commande);";

    MySqlDataReader reader_client;
    reader_client = command_client.ExecuteReader();

    while (reader_client.Read())
    {
        string currentRowAsString = "";
        for (int i = 0; i < reader_client.FieldCount; i++)
        {
            if (reader_client.GetValue(i).ToString() != "")
            {
                if (i != reader_client.FieldCount - 1)
                {
                    string valueAsString = reader_client.GetValue(i).ToString();
                    currentRowAsString += valueAsString + " => ";
                }
                else
                {
                    string valueAsString = reader_client.GetValue(i).ToString();
                    currentRowAsString += valueAsString + "";
                }
            }
        }
        Console.WriteLine(currentRowAsString);
    }
    reader_client.Close();
}

static void stat_ventes_annee(MySqlConnection connection)
{
    MySqlCommand command_client = connection.CreateCommand();
    command_client.CommandText = "select YEAR(date_commande),count(*) from commande group by YEAR(date_commande);";

    MySqlDataReader reader_client;
    reader_client = command_client.ExecuteReader();

    while (reader_client.Read())
    {
        string currentRowAsString = "";
        for (int i = 0; i < reader_client.FieldCount; i++)
        {
            if (reader_client.GetValue(i).ToString() != "")
            {
                if (i != reader_client.FieldCount - 1)
                {
                    string valueAsString = reader_client.GetValue(i).ToString();
                    currentRowAsString += valueAsString + " => ";
                }
                else
                {
                    string valueAsString = reader_client.GetValue(i).ToString();
                    currentRowAsString += valueAsString + "";
                }
            }
        }
        Console.WriteLine(currentRowAsString);
    }
    reader_client.Close();
}

static void stat_nb_ventes(MySqlConnection connection)
{
    Console.Clear();
    MySqlCommand command_client = connection.CreateCommand();
    command_client.CommandText = "select count(*) from commande ;";

    MySqlDataReader reader_client;
    reader_client = command_client.ExecuteReader();

    while (reader_client.Read())
    {
        string currentRowAsString = "Il y a eu un total de ";
        for (int i = 0; i < reader_client.FieldCount; i++)
        {
            if (reader_client.GetValue(i).ToString() != "")
            {
                string valueAsString = reader_client.GetValue(i).ToString();
                currentRowAsString += valueAsString;

            }
            currentRowAsString += " commandes.";
        }
        Console.WriteLine(currentRowAsString);
    }
    reader_client.Close();
}
#endregion

#region<Stats produits - Sous-méthodes>
static void stats_produits(MySqlConnection connection)
{
    Console.Clear();
    bool leave = false;
    while (!leave)
    {
        Console.Clear();
        Console.WriteLine("Quelle type de statistique voulez-vous voir ?\n1-Bouquet le plus vendu\n2-Fleur la plus vendue\n3-Accessoire le plus vendu" +
            "\n4-Quitter");
        int choice1 = Convert.ToInt32(Console.ReadLine());
        while (choice1 < 1 || choice1 > 7)
        {
            Console.WriteLine("Veuillez saisir un choix valide");
            choice1 = Convert.ToInt32(Console.ReadLine());
        }
        switch (choice1)
        {
            case 1:

                break;
            case 2:

                break;
            case 3:
                //Gains = factures - prix composants
                break;
            case 4:
                //Quitter
                leave = true;
                break;
            default: break;
        }
    }
}

static void stat_bouquet(MySqlConnection connection)
{
    Console.Clear();
    MySqlCommand command_client = connection.CreateCommand();
    command_client.CommandText = "SELECT nom_bouquet AS nb_ventes FROM commande GROUP BY nom_bouquet ORDER BY nb_ventes DESC LIMIT 1;";

    MySqlDataReader reader_client;
    reader_client = command_client.ExecuteReader();

    while (reader_client.Read())
    {
        string currentRowAsString = "Le bouquet le plus vendu est : ";
        for (int i = 0; i < reader_client.FieldCount; i++)
        {
            if (reader_client.GetValue(i).ToString() != "")
            {
                string valueAsString = reader_client.GetValue(i).ToString();
                currentRowAsString += valueAsString;

            }
            currentRowAsString += ".";
        }
        Console.WriteLine(currentRowAsString);
    }
    reader_client.Close();
}
#endregion

#region<Stats clients>
static void best_client(MySqlConnection connection)
{
    Console.Clear();
    MySqlCommand command_client = connection.CreateCommand();
    command_client.CommandText = "SELECT c.nom, c.prenom ,count(*) as cnt FROM client c JOIN commande cm ON c.email = cm.email GROUP BY c.nom, c.prenom order by cnt desc limit 1; ";

    MySqlDataReader reader_client;
    reader_client = command_client.ExecuteReader();

    while (reader_client.Read())
    {
        string currentRowAsString = "Le meilleur client est : ";
        for (int i = 0; i < reader_client.FieldCount; i++)
        {
            if (reader_client.GetValue(i).ToString() != "")
            {
                string valueAsString = reader_client.GetValue(i).ToString();
                currentRowAsString += valueAsString;

            }
            
            currentRowAsString += " avec ";
        }
        currentRowAsString += " commandes";
        Console.WriteLine(currentRowAsString);
    }
    reader_client.Close();
}
#endregion


//FINI
//Fonction permettant d'exporter les clients ayant commander durant le dernier mois en fichier XML
static void Export_XML(MySqlConnection connection)
{
    Console.Clear();
    Console.WriteLine("Création du document XML en cours...");
    
    MySqlCommand command_client = connection.CreateCommand();
    command_client.CommandText = "SELECT c.email,c.nom, c.prenom, c.telephone,c.adresse_facturation,c.mdp"+
                                 " FROM Fleurs.client c"+
                                 " INNER JOIN Fleurs.Commande cmd ON c.email = cmd.email"+
                                 " WHERE cmd.date_commande >= DATE_SUB(NOW(), INTERVAL 1 MONTH)"+
                                 " GROUP BY c.email, c.mdp"+
                                 " HAVING COUNT(DISTINCT cmd.id_commande) > 1; ";

    MySqlDataReader reader_client;
    reader_client = command_client.ExecuteReader();
    List<bellefleur.Client>xml_clients = new List<bellefleur.Client>();
    while (reader_client.Read())
    {
        string[] currentRowAsString = new string[6];
        for (int i = 0; i < reader_client.FieldCount; i++)
        {
            
            if (reader_client.GetValue(i).ToString() != "")
            {
                string valueAsString = reader_client.GetValue(i).ToString();
                currentRowAsString[i] = valueAsString;
            }
        }
        bellefleur.Client c = new bellefleur.Client(currentRowAsString[0], currentRowAsString[1], currentRowAsString[2], currentRowAsString[3], currentRowAsString[4], currentRowAsString[5]);
        xml_clients.Add(c); 
    }
    reader_client.Close();

    XDocument xmlDocument = new XDocument();
    XElement rootElement = new XElement("Clients");
    xmlDocument.Add(rootElement);

    foreach (bellefleur.Client c in xml_clients)
    {
        XElement clientElement = new XElement("Client",
            new XElement("Email", c.Email),
            new XElement("Nom", c.Nom),
            new XElement("Prenom", c.Prenom),
            new XElement("Telephone", c.Telephone),
            new XElement("Adresse", c.Adresse),
            new XElement("Carte_credit", c.Carte_credit),
            new XElement("Mdp", c.Mdp),
            new XElement("Fidelite",c.Fidelite)
        ) ;
        rootElement.Add(clientElement);
    }

    xmlDocument.Save("clients.xml");
    Console.WriteLine("Le document XML clients.xml a été créé avec succès.");
    Console.ReadKey();


}

//FINI(voir si possible de donner le nom clients a la list dans le fichier json)
//Fonction permettant d'exporter les clients ayant commander il y a plus de 6 mois en fichier JSON
static void Export_JSON(MySqlConnection connection)
{
    Console.Clear();
    Console.WriteLine("Création du document JSON en cours...");

    MySqlCommand command_client = connection.CreateCommand();
    command_client.CommandText = "SELECT c.email,c.nom, c.prenom, c.telephone,c.adresse_facturation,c.mdp" +
                                 " FROM Fleurs.client c" +
                                 " INNER JOIN Fleurs.Commande cmd ON c.email = cmd.email" +
                                 " WHERE cmd.date_commande <= DATE_SUB(NOW(), INTERVAL 6 MONTH)" +
                                 " GROUP BY c.email, c.mdp";

    MySqlDataReader reader_client;
    reader_client = command_client.ExecuteReader();
    List<bellefleur.Client> json_clients = new List<bellefleur.Client>();
    while (reader_client.Read())
    {
        string[] currentRowAsString = new string[6];
        for (int i = 0; i < reader_client.FieldCount; i++)
        {

            if (reader_client.GetValue(i).ToString() != "")
            {
                string valueAsString = reader_client.GetValue(i).ToString();
                currentRowAsString[i] = valueAsString;
            }
        }
        bellefleur.Client c = new bellefleur.Client(currentRowAsString[0], currentRowAsString[1], currentRowAsString[2], currentRowAsString[3], currentRowAsString[4], currentRowAsString[5]);
        json_clients.Add(c);
    }
    reader_client.Close();

    //Serialisation en JSON
    string filename = "clients.json";
    StreamWriter fileWriter = new StreamWriter(filename);
    JsonTextWriter jsonWriter = new JsonTextWriter(fileWriter);
    JsonSerializer serializer = new JsonSerializer();
    serializer.Serialize(jsonWriter, json_clients);
    jsonWriter.Close();
    Console.WriteLine($"Le fichier {filename} a été créé avec succès.");
    Console.ReadKey();
}

//EN COURS - ajouter sécurité pour les readlines
//Fonction permettant de modifier l'état des commandes
static void Modif_cmd(MySqlConnection connection)
{
    Console.Clear();
    Console.WriteLine("Quelle commande voulez-vous modifier ? (numéro de commande)");

    MySqlCommand command_cmd = connection.CreateCommand();
    command_cmd.CommandText = "select * from fleurs.commande where commande.statut not in ('CL');";

    MySqlDataReader reader_cmd;
    reader_cmd = command_cmd.ExecuteReader();
    List<bellefleur.Commande> cmd_list = new List<bellefleur.Commande>();
    while (reader_cmd.Read())
    {
        string[] currentRowAsString = new string[10];
        string string_cmd = "";
        for (int i = 0; i < reader_cmd.FieldCount; i++)
        {
            if (reader_cmd.GetValue(i).ToString() != "")
            {
                string valueAsString = reader_cmd.GetValue(i).ToString();
                currentRowAsString[i] = valueAsString;
                string_cmd += valueAsString + " ,";
            }
        }
        Console.WriteLine(string_cmd+"\n");
        Commande cmd = new Commande(Convert.ToInt32(currentRowAsString[0]), currentRowAsString[1], Convert.ToDateTime(currentRowAsString[2]), Convert.ToDateTime(currentRowAsString[3]),
                                                    currentRowAsString[4], Single.Parse(currentRowAsString[5]), currentRowAsString[6], currentRowAsString[7], currentRowAsString[8],
                                                    currentRowAsString[9]);
        cmd_list.Add(cmd);
    }
    reader_cmd.Close();
    Console.Write("Votre choix : ");
    int choice = Convert.ToInt32(Console.ReadLine());
    Console.Clear();
    Console.Write("Le statut de la commande " + choice + " est : ");
    foreach(Commande com in cmd_list)
    {
        if(com.Id_commande== choice)
        {
            Console.WriteLine(com.Statut);
        }
    }
    Console.Write("\nQuel statut voulez-vous mettre ?\nVINV - Verifier inventaire\nCC - Commande complète\nCAL - Commande à livrer\nCL - Commande livrée\nNouveau statut : ");
    cmd_list[choice].Statut = Console.ReadLine();

    command_cmd.CommandText = "UPDATE Fleurs.Commande SET statut ='"+cmd_list[choice].Statut+"' WHERE id_commande ="+choice;
    command_cmd.ExecuteNonQuery();
    Console.WriteLine("Modification effectuée\nVeuillez cliquer pour continuer...");
    Console.ReadKey();
}