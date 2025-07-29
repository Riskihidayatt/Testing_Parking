#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestParking
{
    public class Vehicle
    {
        public string RegistrationNumber { get; }
        public string Colour { get; }
        public string Type { get; }

        public Vehicle(string registrationNumber, string colour, string type)
        {
            RegistrationNumber = registrationNumber;
            Colour = colour;
            Type = type;
        }
    }

    public class ParkingLot
    {
        private Vehicle?[]? slots;
        private int capacity;

        public void CreateParkingLot(int capacity)
        {
            this.capacity = capacity;
            slots = new Vehicle[capacity];
            Console.WriteLine($"Created a parking lot with {capacity} slots");
        }

        public void Park(string registrationNumber, string colour, string type)
        {
            if (slots == null)
            {
                Console.WriteLine("Parking lot has not been created yet.");
                return;
            }
            
            if (!type.Equals("Mobil", StringComparison.OrdinalIgnoreCase) && !type.Equals("Motor", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Invalid vehicle type. Only Mobil and Motor are allowed.");
                return;
            }

            for (int i = 0; i < capacity; i++)
            {
                if (slots[i] == null)
                {
                    slots[i] = new Vehicle(registrationNumber, colour, type);
                    Console.WriteLine($"Allocated slot number: {i + 1}");
                    return;
                }
            }
            Console.WriteLine("Sorry, parking lot is full");
        }

        public void Leave(int slotNumber)
        {
            if (slots == null)
            {
                Console.WriteLine("Parking lot has not been created yet.");
                return;
            }

            if (slotNumber > 0 && slotNumber <= capacity)
            {
                if (slots[slotNumber - 1] != null)
                {
                    slots[slotNumber - 1] = null;
                    Console.WriteLine($"Slot number {slotNumber} is free");
                }
                else
                {
                    Console.WriteLine($"Slot number {slotNumber} is already empty.");
                }
            }
            else
            {
                Console.WriteLine($"Invalid slot number. Please provide a slot between 1 and {capacity}.");
            }
        }

        public void Status()
        {
            if (slots == null)
            {
                Console.WriteLine("Parking lot has not been created yet.");
                return;
            }

            Console.WriteLine("Slot No.    Type           Registration No    Colour");
            bool empty = true;
            for (int i = 0; i < capacity; i++)
            {
                if (slots[i] != null)
                {
                    empty = false;
                    Console.WriteLine($"{i + 1,-12}{slots[i]!.Type,-15}{slots[i]!.RegistrationNumber,-19}{slots[i]!.Colour}");
                }
            }
            if(empty)
            {
                Console.WriteLine("Parking lot is empty.");
            }
        }

        public void TypeOfVehicles(string type)
        {
            if (slots == null) return;
            int count = slots.Count(v => v != null && v.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
            Console.WriteLine(count);
        }

        public void RegistrationNumbersForVehiclesWithOddPlate()
        {
            if (slots == null) return;
            var result = slots.Where(v => v != null && IsPlateOdd(v.RegistrationNumber))
                              .Select(v => v!.RegistrationNumber);
            Console.WriteLine(string.Join(", ", result));
        }

        public void RegistrationNumbersForVehiclesWithEvenPlate()
        {
            if (slots == null) return;
            var result = slots.Where(v => v != null && !IsPlateOdd(v.RegistrationNumber))
                              .Select(v => v!.RegistrationNumber);
            Console.WriteLine(string.Join(", ", result));
        }

        public void RegistrationNumbersForVehiclesWithColour(string colour)
        {
            if (slots == null) return;
            var result = slots.Where(v => v != null && v.Colour.Equals(colour, StringComparison.OrdinalIgnoreCase))
                              .Select(v => v!.RegistrationNumber);
            Console.WriteLine(string.Join(", ", result));
        }

        public void SlotNumbersForVehiclesWithColour(string colour)
        {
            if (slots == null) return;
            var result = Enumerable.Range(0, capacity)
                                   .Where(i => slots[i] != null && slots[i]!.Colour.Equals(colour, StringComparison.OrdinalIgnoreCase))
                                   .Select(i => (i + 1).ToString());
            Console.WriteLine(string.Join(", ", result));
        }

        public void SlotNumberForRegistrationNumber(string registrationNumber)
        {
            if (slots == null) return;
            for (int i = 0; i < capacity; i++)
            {
                if (slots[i] != null && slots[i]!.RegistrationNumber.Equals(registrationNumber, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(i + 1);
                    return;
                }
            }
            Console.WriteLine("Not found");
        }

        private bool IsPlateOdd(string registrationNumber)
        {
            var numberPart = new string(registrationNumber.Where(char.IsDigit).ToArray());
            if (int.TryParse(numberPart.LastOrDefault().ToString(), out int lastDigit))
            {
                return lastDigit % 2 != 0;
            }
            return false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ParkingLot parkingLot = new ParkingLot();
            string? command;

            Console.WriteLine("Welcome to the Parking Lot System!");

            while (true)
            {
                Console.Write("> ");
                command = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(command)) continue;
                if (command.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;

                string[] parts = command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string action = parts[0].ToLower();

                try
                {
                    switch (action)
                    {
                        case "create_parking_lot":
                            if (parts.Length > 1 && int.TryParse(parts[1], out int capacity) && capacity > 0)
                            {
                                parkingLot.CreateParkingLot(capacity);
                            }
                            else
                            {
                                Console.WriteLine("Usage: create_parking_lot [CAPACITY]");
                                Console.WriteLine("Example: create_parking_lot 6");
                            }
                            break;
                        case "park":
                            if (parts.Length == 4)
                            {
                                parkingLot.Park(parts[1], parts[2], parts[3]);
                            }
                            else
                            {
                                Console.WriteLine("Usage: park [REGISTRATION_NUMBER] [COLOUR] [TYPE]");
                                Console.WriteLine("Example: park B-1234-XYZ Putih Mobil");
                            }
                            break;
                        case "leave":
                            if (parts.Length > 1 && int.TryParse(parts[1], out int slot))
                            {
                                parkingLot.Leave(slot);
                            }
                            else
                            {
                                Console.WriteLine("Usage: leave [SLOT_NUMBER]");
                                Console.WriteLine("Example: leave 3");
                            }
                            break;
                        case "status":
                            parkingLot.Status();
                            break;
                        case "type_of_vehicles":
                            if (parts.Length > 1)
                            {
                                parkingLot.TypeOfVehicles(parts[1]);
                            }
                            else
                            {
                                Console.WriteLine("Usage: type_of_vehicles [TYPE]");
                                Console.WriteLine("Example: type_of_vehicles Mobil");
                            }
                            break;
                        case "registration_numbers_for_vehicles_with_odd_plate":
                            parkingLot.RegistrationNumbersForVehiclesWithOddPlate();
                            break;
                        case "registration_numbers_for_vehicles_with_even_plate":
                            parkingLot.RegistrationNumbersForVehiclesWithEvenPlate();
                            break;
                        case "registration_numbers_for_vehicles_with_colour":
                            if (parts.Length > 1)
                            {
                                parkingLot.RegistrationNumbersForVehiclesWithColour(parts[1]);
                            }
                            else
                            {
                                Console.WriteLine("Usage: registration_numbers_for_vehicles_with_colour [COLOUR]");
                                Console.WriteLine("Example: registration_numbers_for_vehicles_with_colour Putih");
                            }
                            break;
                        case "slot_numbers_for_vehicles_with_colour":
                            if (parts.Length > 1)
                            {
                                parkingLot.SlotNumbersForVehiclesWithColour(parts[1]);
                            }
                            else
                            {
                                Console.WriteLine("Usage: slot_numbers_for_vehicles_with_colour [COLOUR]");
                                Console.WriteLine("Example: slot_numbers_for_vehicles_with_colour Merah");
                            }
                            break;
                        case "slot_number_for_registration_number":
                            if (parts.Length > 1)
                            {
                                parkingLot.SlotNumberForRegistrationNumber(parts[1]);
                            }
                            else
                            {
                                Console.WriteLine("Usage: slot_number_for_registration_number [REGISTRATION_NUMBER]");
                                Console.WriteLine("Example: slot_number_for_registration_number B-1234-XYZ");
                            }
                            break;
                        default:
                            Console.WriteLine($"Unknown command: '{command}'");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                Console.WriteLine(); // Add a blank line for better readability
            }
        }
    }
}
