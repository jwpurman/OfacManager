using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data.SqlClient;
using System.Text;
using OfacManager.Models;
using OfacManager.Lib;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace OfacManager
{
    class Program
    {
        static void Main(string[] args)
        {

          
            //get xml
            Console.WriteLine("Getting XML...");
            string xml = Downloader.GetSdnList();

            //parse into object
            Console.WriteLine("Parsing XML...");
            OfacSdnList list = Parser.ParseSdnList(xml);

            //do something with it... :)

            
            //Connect to OFAC Database
            string connectionString = OfacManager.Properties.Settings.Default.connectionString;



            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                //Get the entries from the xml that is parsed and put into columns


                foreach (var entry in list.Entries)
                {
                    Console.WriteLine(string.Format("{0}: {1}", entry.Type, entry.Name));

                  

                    SqlCommand insertCommand = new SqlCommand("INSERT INTO dbo.SDNindividual (Type, Name) VALUES ( @entry, @name )" ,connection);

                    insertCommand.Parameters.Add(new SqlParameter ("entry", entry.Type));

                    insertCommand.Parameters.Add(new SqlParameter("name", entry.Name));

                    Console.WriteLine("Commands Executed! Total rows affected are " + insertCommand.ExecuteNonQuery());
                    Console.WriteLine("Done! Press enter to move to the next step");
                    Console.ReadLine();

                }
               




               




            }


            Console.WriteLine("Doing something with my new object...");
            Console.WriteLine(string.Format("Publish Date: {0}", list.PublishInformation.PublishDate.ToLongDateString()));

        


            foreach (var entry in list.Entries)
            {
                Console.WriteLine(string.Format("{0}: {1}", entry.Type, entry.Name));

            


            }

            Console.WriteLine("---");
            Console.WriteLine("Press 'Enter' To Exit");
            Console.ReadLine();
        }



    }
}
