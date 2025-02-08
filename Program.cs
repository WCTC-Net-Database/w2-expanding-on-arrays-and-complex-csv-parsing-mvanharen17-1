using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using System.Xml.Linq;

class Program
{
    static string[] lines;

    static void Main()
    {
        string filePath = "input.csv";
        lines = File.ReadAllLines(filePath);

        while (true)
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. Display Characters");
            Console.WriteLine("2. Add Character");
            Console.WriteLine("3. Level Up Character");
            Console.WriteLine("4. Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DisplayAllCharacters(lines);
                    break;
                case "2":
                    AddCharacter(ref lines);
                    break;
                case "3":
                    LevelUpCharacter(lines);
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void DisplayAllCharacters(string[] lines)
    {
        // Skip the header row
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];

            string name;
            int commaIndex;

            // Check if the name is quoted
            if (line.StartsWith("\""))
            {
                string firstRemoved = line.Trim('"');
                var quotePos = firstRemoved.IndexOf('"');
                name = firstRemoved.Substring(0, quotePos);

                var rest = firstRemoved.Substring(name.Length + 2);
                var splits = rest.Split(",");
                string charClass = splits[0];
                var level = splits[1];
                var hp = splits[2];
                string[] equipment = splits[3].Split("|");

                Console.WriteLine($"\nName: {name}");
                Console.WriteLine($"Job: {charClass}");
                Console.WriteLine($"Level: {level}");
                Console.WriteLine($"Hit Points: {hp}");
                Console.WriteLine($"Equipment: {string.Join(", ", equipment)}");
            }
            else
            {
                var cols = lines[i].Split(",");
                name = cols[0];
                string charClass = cols[1];
                var level = cols[2];
                var hp = cols[3];
                string[] equipment = cols[4].Split("|");

                Console.WriteLine($"\nName: {name}");
                Console.WriteLine($"Job: {charClass}");
                Console.WriteLine($"Level: {level}");
                Console.WriteLine($"Hit Points: {hp}");
                Console.WriteLine($"Equipment: {string.Join(", ", equipment)}");
            }
        }
    }

    static void AddCharacter(ref string[] lines)
    {
        // Prompt for character details (name, class, level, hit points, equipment)
        // DO NOT just ask the user to enter a new line of CSV data or enter the pipe-separated equipment string
        // Append the new character to the lines array

        Console.Write("\n=== Add a Character ===\n");

        Console.Write("Enter your character's name: ");
        string charName = Console.ReadLine();

        Console.Write("Enter your character's class: ");
        string charClass = Console.ReadLine();

        Console.Write("Enter your character's level: ");
        int level = int.Parse(Console.ReadLine());

        Console.Write("Enter your character's HP: ");
        int hp = int.Parse(Console.ReadLine());

        Console.Write("Enter your character's equipment (separate items with a '|'): ");
        string[] equipment = Console.ReadLine().Split('|');

        Console.WriteLine($"Welcome, {charName} the {charClass}! You are level {level} with {hp} HP, and your equipment includes: {string.Join(", ", equipment)}.\n");

        using (StreamWriter writer = new StreamWriter("input.csv", true))
        {
            writer.WriteLine($"\n{charName},{charClass},{level},{hp},{string.Join("|", equipment)}");
        }
    }

    static void LevelUpCharacter(string[] lines)
    {
        Console.Write("Enter the name of the character to level up: ");
        string nameToLevelUp = Console.ReadLine();

        // Loop through characters to find the one to level up
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];

            // Check if the name matches the one to level up
            if (line.Contains(nameToLevelUp))
            {
                if (line.StartsWith("\""))
                {
                    string firstRemoved = line.Trim('"');
                    var quotePos = firstRemoved.IndexOf('"');
                    string name = firstRemoved.Substring(0, quotePos);

                    var rest = firstRemoved.Substring(name.Length + 2);
                    var splits = rest.Split(",");
                    string charClass = splits[0];
                    var level = splits[1];
                    var hp = splits[2];
                    string[] equipment = splits[3].Split("|");

                    // Level up the character
                    int j = int.Parse(level);
                    var newLevel = Convert.ToString(j + 1);

                    Console.WriteLine($"{name} is now Level {newLevel}");

                    // Update the line with the new level
                    using (StreamWriter writer = new StreamWriter("input.csv", true))
                    {
                        writer.WriteLine($"\n{name}, {charClass}, {newLevel}, {hp}, {string.Join("|", equipment)}");
                    }
                    break;
                }
                else
                {
                    var cols = lines[i].Split(",");
                    string name = cols[0];
                    string charClass = cols[1];
                    var level = cols[2];
                    var hp = cols[3];
                    var equipment = cols[4];

                    // Level up the character
                    int j = int.Parse(level);
                    var newLevel = Convert.ToString(j + 1);

                    Console.WriteLine($"{name} is now Level {newLevel}");

                    // Update the line with the new level
                    using (StreamWriter writer = new StreamWriter("input.csv", true))
                    {
                        writer.WriteLine($"\n{name}, {charClass}, {newLevel}, {hp}, {string.Join("|", equipment)}");
                    }
                    break;
                }
            }
        }
    }
}