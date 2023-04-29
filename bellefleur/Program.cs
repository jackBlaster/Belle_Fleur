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


//----INITIALISATION----//
Console.WriteLine("----Bienvenue dans la base de donnée Belle Fleur----\n Veuillez appuyer sur une touche...");
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
//Faire affichage et test sur la valeur de choice
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
    int choice1 = Convert.ToInt32(Console.ReadLine());//MODIFIER (la secu ne marche pas si on entre autre chose qu'un int)
    while (!(choice1 is int) && (choice1 < 1 || choice1 > 7))//MODIFIER (la secu ne marche pas si on entre autre chose qu'un int)
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

//EN COURS (RESTE : modifier la presentation)
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
        int choice1 = Convert.ToInt32(Console.ReadLine());
        while (choice1 < 1 || choice1 > 7)
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
                    string currentRowAsString = "";
                    for (int i = 0; i < reader_client.FieldCount; i++)
                    {
                        if (reader_client.GetValue(i).ToString() != "")
                        {
                            string valueAsString = reader_client.GetValue(i).ToString();
                            currentRowAsString += valueAsString + ", ";
                        }
                    }
                    Console.WriteLine(currentRowAsString);
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
                    string currentRowAsString = "";
                    for (int i = 0; i < reader_cmd.FieldCount; i++)
                    {
                        if (reader_cmd.GetValue(i).ToString() != "")
                        {
                            string valueAsString = reader_cmd.GetValue(i).ToString();
                            currentRowAsString += valueAsString + ", ";
                        }
                    }
                    Console.WriteLine(currentRowAsString);
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
static void Ajout_DB(MySqlConnection connection)
{
    int choice1 = Convert.ToInt32(Console.ReadLine());
    while (choice1 < 1 || choice1 > 7)
    {
        Console.WriteLine("Veuillez saisir un choix valide");
        choice1 = Convert.ToInt32(Console.ReadLine());
    }
    switch (choice1)
    {
        case 1:
            //Ajout clients

            break;
        case 2:
            //Ajout commandes
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

//En cours
static void Modifier_DB(MySqlConnection connection)
{
    int choice1 = Convert.ToInt32(Console.ReadLine());
    while (choice1 < 1 || choice1 > 7)
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
static void Show_Stats(MySqlConnection connection)
{
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
            break;
        case 2:
            //Produits les plus utilisés
            break;
        case 3:
            //Gains = factures - prix composants
            break;
        case 4:
            //Clients (plus de comandes etc...)
            break;
        case 5:
            //Quitter
            break;
        default: break;
    }
}

//FINI
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


static void Modif_cmd(MySqlConnection connection)
{
    Console.Clear();
    Console.WriteLine("Quelle commande voulez-vous modifier ? (numéro de commande)");

    MySqlCommand command_cmd = connection.CreateCommand();
    command_cmd.CommandText = "select * from fleurs.commande where commande.statut not in ('CL');";

    MySqlDataReader reader_cmd;
    reader_cmd = command_cmd.ExecuteReader();

    while (reader_cmd.Read())
    {
        string currentRowAsString = "";
        for (int i = 0; i < reader_cmd.FieldCount; i++)
        {
            if (reader_cmd.GetValue(i).ToString() != "")
            {
                string valueAsString = reader_cmd.GetValue(i).ToString();
                currentRowAsString += valueAsString + ", ";
            }
        }
        Console.WriteLine(currentRowAsString+"\n");
    }
    reader_cmd.Close();
    Console.Write("Votre choix : ");
    int choice = Convert.ToInt32(Console.ReadLine());
    Console.Clear();
    Console.WriteLine("Le statut de la commande " + choice + " est ");
    
}