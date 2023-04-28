using MySql.Data;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Security;
using System.Configuration;
using System.Runtime.ExceptionServices;
using System.Xml;
using bellefleur;


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

try
{
    string connectionString = "SERVER=localhost;PORT=3301;database=fleurs;UID="+username+";PASSWORD="+mdp+";";
    MySqlConnection connection = new MySqlConnection(connectionString);
    connection.Open();
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}



//PREMIER MENU POUR Root/root
//Faire affichage et test sur la valeur de choice
Console.WriteLine("Que voulez-vous faire ?\n"+
                   "1-Lire la base de donnée\n" +
                   "2-Ajouter un élément\n" +
                   "3-Modifier un élément\n" +
                   "4-Voir les statistiques\n" +
                   "5-Exporter un fichier XML\n" +
                   "6-Exporter un fichier JSON\n" +
                   "7-Déconnexion" );
Console.Write("Selectionner votre action(1 à 7) : ");
int choice1 = Convert.ToInt32(Console.ReadLine());
while ((choice1 is int)&&(choice1 < 1 || choice1 > 7))
{
    Console.WriteLine("Veuillez saisir un choix valide");
    choice1 = Convert.ToInt32(Console.ReadLine());
}
switch (choice1)
{
    case 1:Lecture_DB();
        break;
    case 2:Ajout_DB();
        break;
    case 3: Modifier_DB();
        break;
    case 4: Show_Stats();
        break;
    case 5: Export_XML();
        break;
    case 6:Export_JSON();
        break;
    case 7://Deconnexion
        break;
    default:break;
}

//A FAIRE
static void Lecture_DB()
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
            //Liste clients
            break;
        case 2:
            //Liste commandes
            break;
        case 3:
            //Stocks
            break;
        case 4:
            //Quitter;
            break;
        default: break;
    }

}

//A FAIRE
static void Ajout_DB()
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

//A FAIRE
static void Modifier_DB()
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
static void Show_Stats()
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

//A FAIRE
static void Export_XML()
{

}

//A FAIRE
static void Export_JSON()
{

}