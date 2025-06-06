using CVSWithLibary;

var userManager = new UserManager("Users.txt");
bool loggedIn = false;
string currentUser = "";

while (!loggedIn)
{
    Console.WriteLine("1. Login");
    Console.WriteLine("2. Register");
    Console.Write("Choose option: ");
    string option = Console.ReadLine() ?? "";

    if (option == "1")
    {
        Console.Write("Username: ");
        string username = Console.ReadLine() ?? "";
        Console.Write("Password: ");
        string password = Console.ReadLine() ?? "";

        if (userManager.ValidateUser(username, password))
        {
            Console.WriteLine("Login successful.\n");
            loggedIn = true;
            currentUser = username;
        }
        else
        {
            Console.WriteLine("Invalid username or password.\n");
        }
    }
    else if (option == "2")
    {
        Console.Write("Choose a username: ");
        string username = Console.ReadLine() ?? "";
        if (userManager.UserExists(username))
        {
            Console.WriteLine("Username already exists.\n");
            continue;
        }

        Console.Write("Choose a password: ");
        string password = Console.ReadLine() ?? "";

        if (userManager.AddUser(username, password))
        {
            Console.WriteLine("User registered successfully. You can now login.\n");
        }
        else
        {
            Console.WriteLine("Error registering user.\n");
        }
    }
    else
    {
        Console.WriteLine("Invalid option.\n");
    }
}


var helper = new CsvHelperExample();
var people = helper.Read("people.csv").ToList();

string opc;
do
{
    Console.WriteLine("========== PERSON MANAGEMENT ==========");
    Console.WriteLine("1. Show people");
    Console.WriteLine("2. Add person");
    Console.WriteLine("3. Show report by city");
    Console.WriteLine("4. Save changes");
    Console.WriteLine("5. Delete person");
    Console.WriteLine("0. Exit");
    Console.Write("Choose an option: ");
    opc = Console.ReadLine() ?? "0";

    switch (opc)
    {
        case "1":
            foreach (var person in people)
            {
                Console.WriteLine(person);
            }
            break;

        case "2":
            AddPerson(people);
            break;

        case "3":
            ShowReportByCity(people);
            break;

        case "4":
            SaveChanges();
            Console.WriteLine("Changes saved.\n");
            break;

        case "5":
            DeletePerson(people);
            break;

        case "0":
            SaveChanges();
            Console.WriteLine("Exiting...");
            break;

        default:
            Console.WriteLine("Invalid option.\n");
            break;
    }
} while (opc != "0");

void SaveChanges()
{
    helper.Write("people.csv", people);
}

void AddPerson(List<Person> peopleList)
{
    Console.Write("Enter ID (unique number): ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Invalid ID. Must be a number.\n");
        return;
    }
    if (peopleList.Any(p => p.Id == id))
    {
        Console.WriteLine("ID already exists.\n");
        return;
    }

    Console.Write("Enter first name: ");
    string firstName = Console.ReadLine() ?? "";
    if (string.IsNullOrWhiteSpace(firstName))
    {
        Console.WriteLine("First name cannot be empty.\n");
        return;
    }

    Console.Write("Enter last name: ");
    string lastName = Console.ReadLine() ?? "";
    if (string.IsNullOrWhiteSpace(lastName))
    {
        Console.WriteLine("Last name cannot be empty.\n");
        return;
    }

    Console.Write("Enter phone: ");
    string phone = Console.ReadLine() ?? "";
    if (!IsValidPhone(phone))
    {
        Console.WriteLine("Invalid phone number.\n");
        return;
    }

    Console.Write("Enter city: ");
    string city = Console.ReadLine() ?? "";

    Console.Write("Enter balance (positive number): ");
    if (!decimal.TryParse(Console.ReadLine(), out decimal balance) || balance < 0)
    {
        Console.WriteLine("Balance must be a positive number.\n");
        return;
    }

    var newPerson = new Person
    {
        Id = id,
        FirstName = firstName,
        LastName = lastName,
        Phone = phone,
        City = city,
        Balance = balance
    };

    peopleList.Add(newPerson);
    Console.WriteLine("Person added successfully.\n");
}

bool IsValidPhone(string phone)
{
    
    if (string.IsNullOrWhiteSpace(phone)) return false;
    if (phone.Length < 7) return false;
    return phone.All(ch => char.IsDigit(ch) || ch == '+' || ch == '-');
}

void ShowReportByCity(List<Person> peopleList)
{
    var grouped = peopleList.GroupBy(p => p.City);

    decimal totalGeneral = 0;

    foreach (var group in grouped)
    {
        Console.WriteLine($"\nCiudad: {group.Key}\n");
        Console.WriteLine("ID\tNombres\tApellidos\tSaldo");
        Console.WriteLine("—\t—-------------\t—------------\t—----------");

        decimal subtotal = 0;
        foreach (var p in group)
        {
            Console.WriteLine($"{p.Id}\t{p.FirstName}\t{p.LastName}\t{p.Balance,10:C2}");
            subtotal += p.Balance;
        }
        Console.WriteLine("\t\t\t\t=======");
        Console.WriteLine($"Total: {group.Key}\t\t\t{subtotal,10:C2}\n");
        totalGeneral += subtotal;
    }

    Console.WriteLine("\t\t\t\t=======");
    Console.WriteLine($"Total General:\t\t\t{totalGeneral,10:C2}\n");
}
void DeletePerson(List<Person> peopleList)
{
    Console.Write("Enter the ID of the person to delete: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Invalid ID.\n");
        return;
    }

    var person = peopleList.FirstOrDefault(p => p.Id == id);
    if (person == null)
    {
        Console.WriteLine("Person not found.\n");
        return;
    }

    Console.WriteLine("\nPerson found:");
    Console.WriteLine(person);

    Console.Write("Are you sure you want to delete this person? (Y/N): ");
    string confirm = Console.ReadLine()?.Trim().ToUpper() ?? "N";

    if (confirm == "Y")
    {
        peopleList.Remove(person);
        Console.WriteLine("Person deleted successfully.\n");
    }
    else
    {
        Console.WriteLine("Deletion canceled.\n");
    }
}