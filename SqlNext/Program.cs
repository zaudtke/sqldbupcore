using DbUp;
using DbUp.Helpers;
using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
namespace SqlNext
{
    class Program
    {
        static void Main(string[] args)
        {

            //OK FOR NOW, 
            // TODO: Setup Environment stuff to prevent Connection Strings in Source Control
            // Need to figure this out so conditionally load this if local
            // Also need to deal with higher envionments that may not have AD
            // At this time SQL is Login only, not AD
            // We want to find a way to prevent checking in credentials into source control
            // May need to use Envioronment Variables....
            var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

            var connectionString = config["localdbconnection"];

            /*
             * --no-upgrade
             * --no-idempotent
             * --no-dataload
             * --no-security
             * --create-db
             * 
             */


            bool upgrade = true;
            bool idempotent = true;
            bool dataload = true;
            bool security = true;
            bool create_db = false;

            if (args.Any(arg => arg.ToLower().Equals("--create-db")))
            {
                create_db = true;
            }
            if (args.Any(arg => arg.ToLower().Equals("--no-upgrade")))
            {
                upgrade = false;
            }
            if (args.Any(arg => arg.ToLower().Equals("--no-security")))
            {
                security = false;
            }

            if (args.Any(arg => arg.ToLower().Equals("--no-idempotent")))
            {
                idempotent = false;
            }

            if (args.Any(arg => arg.ToLower().Equals("--no-dataload")))
            {
                dataload = false;
            }

            try
            {
                if (create_db)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Creating Database......");

                    EnsureDatabase.For.SqlDatabase(connectionString);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Creating Database...... Success");
                }

                if (upgrade)
                {

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Upgrading Database......");

                    var engine = DeployChanges.To
                                           .SqlDatabase(connectionString)
                                           .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), name => name.Contains(".migrations."))
                                           .LogToConsole()
                                           .Build();


                    engine.PerformUpgrade();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Upgrading Database...... Success");

                }

                if (security)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Creating Security......");

                    var engine = DeployChanges.To
                                            .SqlDatabase(connectionString)
                                            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), name => name.Contains(".security."))
                                            .LogToConsole()
                                            .JournalTo(new NullJournal())
                                            .Build();


                    engine.PerformUpgrade();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Creating Security...... Success");


                }

                if (idempotent)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Running Idempotent......");

                    var engine = DeployChanges.To
                                            .SqlDatabase(connectionString)
                                            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), name => name.Contains(".idempotent."))
                                            .LogToConsole()
                                            .JournalTo(new NullJournal())
                                            .Build();

                    engine.PerformUpgrade();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Running Idempotent...... Success");
                }

                if (dataload)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Running Data Loads......");

                    var engine = DeployChanges.To
                                            .SqlDatabase(connectionString)
                                            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), name => name.Contains(".dataload."))
                                            .LogToConsole()
                                            .JournalTo(new NullJournal())
                                            .Build();

                    engine.PerformUpgrade();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Running Data Loads...... Success");
                }
            }
            catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Failure in Database Migration: {e.Message}");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Enter Key to Exit:");
            Console.ReadLine();
        }
    }
}

