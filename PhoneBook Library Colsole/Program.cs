using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.ComponentModel;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;

class Contact
{
  public string Name { get; set; }
  public string Number { get; set; }
  public Contact(string name, string number)
  {
    this.Name = name;
    this.Number = number;
  }
}

class Program
{
  static string filePath = "contacts.json";
  static List<Contact> contacts = new List<Contact>();
  static void Main()
  {
    LoadContacts();
    while (true)
    {
      System.Console.WriteLine("1. Add contact");
      System.Console.WriteLine("2. Search by name or number");
      System.Console.WriteLine("3. Delete contact");
      System.Console.WriteLine("4. Print all contacts");
      System.Console.WriteLine("5. Exit");
      System.Console.Write("Enter option: ");
      string? type = Console.ReadLine();
      switch (type)
      {
        case "1": Add(); break;
        case "2": SearchContact(); break;
        case "3": DeleteContact(); break;
        case "4": PrintAllContacts(contacts); break;
        case "5": SaveToJson(); return;
        default: System.Console.WriteLine("Invalid option!!"); break;
      }
    }
  }

  static void LoadContacts()
  {
    if (File.Exists(filePath))
    {
      string json = File.ReadAllText(filePath);
      if (!string.IsNullOrWhiteSpace(json))
      {
        contacts = JsonSerializer.Deserialize<List<Contact>>(json);
      }
    }
  }

  static void Add()
  {
    string? name = "";
    string? number = "";
    // name must be valid
    while (string.IsNullOrWhiteSpace(name))
    {
      System.Console.Write("Enter a valid name: ");
      name = Console.ReadLine();
    }
    // number must be valid
    while (!Regex.IsMatch(number, @"^(013|014|015|016|017|018)\d{8}$"))
    {
      System.Console.Write("Enter a valid number: ");
      number = Console.ReadLine();
    }
    contacts.Add(new Contact(name, number));
  }

  static void SearchContact()
  {
    System.Console.Write("Enter name or number: ");
    string? query = Console.ReadLine().ToLower();
    var results = contacts.FindAll(c =>
      c.Name.ToLower().Contains(query) ||
      c.Number.ToLower().Contains(query)
    );
    PrintAllContacts(results);
  }

  static void DeleteContact()
  {
    System.Console.Write("Enter name or number to delete: ");
    string? query = Console.ReadLine().ToLower();
    var target = contacts.Find(c =>
      c.Name.ToLower() == query ||
      c.Number.ToLower() == query
    );
    if (target != null)
    {
      contacts.Remove(target);
      Console.ForegroundColor = ConsoleColor.Blue;
      System.Console.WriteLine("Contact has been deleted!!");
      Console.ResetColor();
    }
    else
    {
      Console.ForegroundColor = ConsoleColor.Red;
      System.Console.WriteLine("Contact not found!!");
      Console.ResetColor();
    }
  }

  static void PrintAllContacts(List<Contact> cons)
  {
    System.Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Green;
    System.Console.WriteLine($"Total {cons.Count} contacts");
    foreach (var contact in cons)
    {
      System.Console.WriteLine($" ------------------------");
      System.Console.WriteLine($"| Name:  {contact.Name.PadLeft(15)} |");
      System.Console.WriteLine($"| Number:{contact.Number.PadLeft(15)} |");
      System.Console.WriteLine($" ------------------------");
    }
    Console.ResetColor();
  }

  static void SaveToJson()
  {
    contacts.Sort((a, b) => a.Name.CompareTo(b.Name));
    string json = JsonSerializer.Serialize(contacts, new JsonSerializerOptions { WriteIndented = true });
    File.WriteAllText(filePath, json);
  }
}
