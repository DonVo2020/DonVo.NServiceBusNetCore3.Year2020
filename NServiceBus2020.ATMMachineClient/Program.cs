using CreditCardValidator;
using Microsoft.Extensions.Configuration;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Serilog;
using NServiceBus2020.DomainBase;
using NServiceBus2020.Messages.Commands;
using NServiceBus2020.NServiceBus;
using StructureMap;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NServiceBus2020.ATMMachineClient
{
    class Program
    {
        private static ICurrentCardNumber _currentCardNumber;

        static async Task Main()
        {
            var endpoint = "NServiceBus2020.ATMMachineClient.Client";
            Console.Title = endpoint;
            var configuration = BuildConfigurationBuilder();
            var logger = LoggerConfiguration.CreateLogger(configuration, endpoint);
            LogManager.Use<SerilogFactory>().WithLogger(logger);
            var container = BuildContainer();
            _currentCardNumber = container.GetInstance<ICurrentCardNumber>();
            IEndpointInstance endpointInstance = await BusEndpointInstance.Learning(endpoint, container); ;
            OptionScreen1(endpointInstance);
            await endpointInstance.Stop().ConfigureAwait(false);
        }

        private static async void OptionScreen1(IEndpointInstance endpointInstance)
        {
            WriteChooseOption();
            Console.WriteLine("1.Request a virtual bank card");
            Console.WriteLine("2.Insert a virtual bank card");
            Console.WriteLine("Press Esc to exit");
            Console.WriteLine();
            while (true)
            {
                var auditInfo = new AuditInfo { By = "login User", Created = DateTime.UtcNow };
                var key = Console.ReadKey();
                Console.WriteLine();
                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        break;
                    case ConsoleKey.NumPad1:
                        _currentCardNumber.Number = String.Empty;
                        var message = new RequestBankCard(auditInfo,CreditCardFactory.RandomCardNumber(CardIssuer.MasterCard), "1");
                        Console.WriteLine($"card has been requested and will virtual be posted");
                        await endpointInstance.Send("NServiceBus2020.BankTransactionsService", message).ConfigureAwait(false);
                        Console.WriteLine($"waiting for card ...");
                        while (true)
                        {
                            if (_currentCardNumber.Number != message.CardNumber) continue;
                            Console.WriteLine($"received bank card {message.CardNumber} and inserting ...");
                            break;
                        }
                        InsertCard(endpointInstance, _currentCardNumber.Number);
                        break;
                    case ConsoleKey.NumPad2:
                        //read Card
                        if (String.IsNullOrEmpty(_currentCardNumber.Number))
                        {
                            Console.WriteLine("Enter the card number");
                            _currentCardNumber.Number = Console.ReadLine();
                        }
                        InsertCard(endpointInstance,_currentCardNumber.Number);
                        break;
                    default:
                        Console.WriteLine("Please select correct option");
                        break;
                }
            }
        }

        private static async void InsertCard(IEndpointInstance endpointInstance,string cardNumber)
        {
            Console.WriteLine();
            Console.WriteLine($"Virtual bank card {cardNumber} has been inserted");
            Console.WriteLine();
            //read PIN
            Console.WriteLine("Enter the pin");
            var pin = int.Parse(Console.ReadLine());
            //TODO: validate bankcard and pin return token - Identity Server or something similar
            OptionScreen2(endpointInstance, cardNumber);
        }

        private static async void WriteOptionScreen2Options()
        {
            WriteChooseOption();
            Console.WriteLine("1.To withdraw money");
            Console.WriteLine("2.To deposite Money");
            Console.WriteLine("Press Esc to exit");
            Console.WriteLine();
        }

        private static async void OptionScreen2(IEndpointInstance endpointInstance, string cardNumber)
        {
            WriteOptionScreen2Options();

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();
                var auditInfo = new AuditInfo { By = cardNumber, Created = DateTime.UtcNow };
                double quantity;
                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        break;
                    case ConsoleKey.NumPad1:
                        Console.WriteLine("Enter the amount to withdraw");
                        {
                            quantity = double.Parse(Console.ReadLine());
                            var message = new WithdrawMoney(auditInfo, cardNumber, quantity);
                            await endpointInstance.Send("NServiceBus2020.BankTransactionsService", message).ConfigureAwait(false);
                            WriteOptionScreen2Options();
                        }
                        break;
                    case ConsoleKey.NumPad2:
                        Console.WriteLine("Enter the amount to deposit");
                        {
                            quantity = double.Parse(Console.ReadLine());
                            var message = new DepositMoney(auditInfo, cardNumber, quantity);
                            await endpointInstance.Send("NServiceBus2020.BankTransactionsService", message).ConfigureAwait(false);
                            Console.WriteLine($"Checked and {quantity} has been deposited successfully..");
                            WriteOptionScreen2Options();
                        }
                        break;
                    default:
                        Console.WriteLine("Please select correct option");
                        break;
                }
            }
        }

        private static void WriteChooseOption()
        {
            Console.WriteLine();
            Console.WriteLine("*** PLEASE CHOOSE AN OPTION ***");
            Console.WriteLine();
        }

        private static IConfigurationRoot BuildConfigurationBuilder() =>
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

        private static IContainer BuildContainer() => new Container(x => {
                x.For<ICurrentCardNumber>().Use<CurrentCardNumber>().Singleton();
            }
        );
    }
}