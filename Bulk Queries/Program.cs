using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Search;
using System;
using System.Net;


namespace EventFrames
{
    class Program
    {
        static void Main(string[] args)
        {
            NetworkCredential credential = new NetworkCredential(connectionInfo.user, connectionInfo.password);
            var piSystem = (new PISystems())[connectionInfo.AFServerName];
            Console.WriteLine($"connecting to : {connectionInfo.AFServerName} - {connectionInfo.AFDatabaseName}");
            piSystem.Connect(credential);
            var afdb = piSystem.Databases[connectionInfo.AFDatabaseName];
            Console.WriteLine("connected");

            //element search
            var query = "Template:'Antimatter Relay'";
            var search = new AFElementSearch(afdb, "Relay Search", query);

            var results = search.FindElements(0, true, 1000);
            var attrList = new AFAttributeList();

            foreach (var element in results)
            {
                Console.WriteLine($"{element.Name}");
                foreach (var attribute in element.Attributes)
                {
                    //not optimized
                    //var snapShot = attribute.GetValue();
                    //Console.WriteLine($"{attribute.Name}: {snapShot.Value.ToString()} {snapShot.UOM}");

                    //optimized
                    attrList.Add(attribute);
                }
            }

            //one call to get values
            var snapShots = attrList.GetValue();
            foreach (var snapShot in snapShots)
            {
                Console.WriteLine($"Element: {snapShot.Attribute.Element.Name} - {snapShot.Attribute.Name}: {snapShot.Value.ToString()} {snapShot.UOM}" );
            }

            Console.WriteLine("completed execution");
            //Console.ReadKey();
        }
    }
}
