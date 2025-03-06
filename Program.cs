using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        ParkingLot? parkingLot = null;

        while (true)
        {
            string input = Console.ReadLine() ?? "Default Value";
            if (string.IsNullOrEmpty(input)) continue;

            string[] commands = input.Split(' ');
            switch (commands[0])
            {
                case "create_parking_lot":
                    int size = int.Parse(commands[1]);
                    parkingLot = new ParkingLot(size);
                    Console.WriteLine($"Created a parking lot with {size} slots");
                    break;
                case "park":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("There is no parking lot available");
                        break;
                    }
                    string regNo = commands[1];
                    string color = commands[2];
                    string type = commands[3];
                    parkingLot.ParkVehicle(new Vehicle(regNo, color, type));
                    break;
                case "leave":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("There is no parking lot available");
                        break;
                    }
                    int slot = int.Parse(commands[1]);
                    parkingLot.Leave(slot);
                    break;
                case "status":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("There is no parking lot available");
                        break;
                    }
                    parkingLot.Status();
                    break;
                case "type_of_vehicles":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("There is no parking lot available");
                        break;
                    }
                    parkingLot.CountByType(commands[1]);
                    break;
                case "registration_numbers_for_vehicles_with_odd_plate":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("There is no parking lot available");
                        break;
                    }
                    parkingLot.GetRegistrationNumbersByOddEven("odd");
                    break;
                case "registration_numbers_for_vehicles_with_even_plate":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("There is no parking lot available");
                        break;
                    }
                    parkingLot.GetRegistrationNumbersByOddEven("even");
                    break;
                case "registration_numbers_for_vehicles_with_colour":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("There is no parking lot available");
                        break;
                    }
                    parkingLot.GetRegistrationNumbersByColor(commands[1]);
                    break;
                case "slot_numbers_for_vehicles_with_colour":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("There is no parking lot available");
                        break;
                    }
                    parkingLot.GetSlotsByColor(commands[1]);
                    break;
                case "slot_number_for_registration_number":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("There is no parking lot available");
                        break;
                    }
                    parkingLot.GetSlotByRegistration(commands[1]);
                    break;
                case "exit":
                    return;
                default:
                    Console.WriteLine("Invalid command");
                    break;
            }
        }
    }
}

class Vehicle
{
    public string RegistrationNumber { get; }
    public string Color { get; }
    public string Type { get; }

    public Vehicle(string registrationNumber, string color, string type)
    {
        RegistrationNumber = registrationNumber;
        Color = color;
        Type = type;
    }
}

class ParkingLot
{
    private Dictionary<int, Vehicle> slots;
    private int capacity;

    public ParkingLot(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentException("Capacity must be greater than 0");
        this.capacity = capacity;
        slots = new Dictionary<int, Vehicle>();
    }

    public void ParkVehicle(Vehicle vehicle)
    {
        if (slots.Count >= capacity)
        {
            Console.WriteLine("Sorry, parking lot is full");
            return;
        }

        int slot = Enumerable.Range(1, capacity).FirstOrDefault(i => !slots.ContainsKey(i));
        slots[slot] = vehicle;
        Console.WriteLine($"Allocated slot number: {slot}");
    }

    public void Leave(int slot)
    {
        if (slots.Remove(slot))
            Console.WriteLine($"Slot number {slot} is free");
        else
            Console.WriteLine("Slot is already empty");
    }

    public void Status()
    {
        Console.WriteLine("Slot\tRegistration No.\tType\t Colour");
        foreach (var slot in slots.OrderBy(s => s.Key))
        {
            Console.WriteLine($"{slot.Key}\t\t{slot.Value.RegistrationNumber}\t\t{slot.Value.Type}\t{slot.Value.Color}");
        }
    }

    public void CountByType(string type)
    {
        int count = slots.Values.Count(v => v.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
        Console.WriteLine(count);
    }

    public void GetRegistrationNumbersByOddEven(string oddEven)
    {
        if (oddEven == "odd") {
            var numbers = slots.Values.Where(v => (int.Parse(v.RegistrationNumber.Split('-')[1]) % 2 == 1))
                                .Select(v => v.RegistrationNumber);
            Console.WriteLine(string.Join(", ", numbers));
        }
        if (oddEven == "even") {
            var numbers = slots.Values.Where(v => (int.Parse(v.RegistrationNumber.Split('-')[1]) % 2 == 0))
                                .Select(v => v.RegistrationNumber);
            Console.WriteLine(string.Join(", ", numbers));
        }
    }

    public void GetRegistrationNumbersByColor(string color)
    {
        var numbers = slots.Values.Where(v => v.Color.Equals(color, StringComparison.OrdinalIgnoreCase))
                                  .Select(v => v.RegistrationNumber);
        Console.WriteLine(string.Join(", ", numbers));
    }

    public void GetSlotsByColor(string color)
    {
        var slotNumbers = slots.Where(s => s.Value.Color.Equals(color, StringComparison.OrdinalIgnoreCase))
                               .Select(s => s.Key);
        Console.WriteLine(string.Join(", ", slotNumbers));
    }

    public void GetSlotByRegistration(string registration)
    {
        var slot = slots.FirstOrDefault(s => s.Value.RegistrationNumber == registration).Key;
        Console.WriteLine(slot == 0 ? "Not found" : slot.ToString());
    }
}