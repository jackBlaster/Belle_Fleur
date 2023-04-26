using MySql.Data;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Security;
using System.Configuration;
using System.Runtime.ExceptionServices;
using System.Xml;


Console.WriteLine("----Bienvenue dans la base de donnée Belle Fleur----\n Veuillezz appuyer sur une touche...");
Console.ReadKey();
Console.Clear();
Console.WriteLine("Veuillez taper votre identifiant :");
string username = Console.ReadLine();


string connectionString = "SERVER=localhost;PORT=3301;database=loueur;UID=root;PASSWORD=root;";
MySqlConnection connection = new MySqlConnection(connectionString);
connection.Open();

//PREMIER MENU POUR Root/root
//Faire affichage et test sur la valeur de choice
int choice1=Convert.ToInt32(Console.ReadLine());
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
    default:break;
}

static void Lecture_DB()
{

}

static void Ajout_DB()
{

}
static void Modifier_DB()
{

}
static void Show_Stats()
{

}

static void Export_XML()
{

}
static void Export_JSON()
{

}