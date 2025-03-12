using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using TestConsole;

namespace Quickbooks_X_Exact {
    internal class Program {
        static void Main(string[] args) {
            string ExactRecordsPath = @"C:\Users\steve\Documents\3000-THE_EFORUM_FACTORY_BV-11-03-2025-CRMAccounts.csv";
            string QBRecordsPath = @"C:\Users\steve\Documents\EFF_QB_CUSTOMERS.csv";


            List<CustomerExact> exacts = [];
            List<CustomerQB> qbs = [];

            List<string> exactsBTW = [];
            List<string> qbsBTW = [];

            List<string> exactsAdres = [];
            List<string> qbsAdres = [];



            #region init_file_reading_company_name


            using StreamReader reader = new(ExactRecordsPath, true);
            {
                while (!reader.EndOfStream) {
                    var parts = reader.ReadLine().Split(';');

                    CustomerExact c = new() {
                        CompanyName = parts[1],
                        BTW = parts[8],
                        Adres = parts[2]
                    };

                    exacts.Add(c);
                }
            };


            using StreamReader reader2 = new(QBRecordsPath, true);
            {
                while (!reader2.EndOfStream) {
                    var parts = reader2.ReadLine().Split(';');

                    CustomerQB c = new() {
                        CompanyName = parts[2],
                        BTW = [parts[23], parts[24], parts[35]],
                        Adres = [parts[21], parts[22]]

                    };
                    qbs.Add(c);
                };
            }

            #endregion

            


            List<CustomerExact> resultName = exacts
                .Where(E => !qbs.Select(q => q.CompanyName).Contains(E.CompanyName, new MyComparer())).ToList();




            List<CustomerExact> resultBtw = exacts
                
               .Where(E => !qbs.All(q => q.BTW.All(cn => BtwNumber(E.BTW) != BtwNumber(cn)))).ToList();

            //Klopt nog niet :( qbs.adres is een array...
            //List<CustomerExact> resultAdres = exacts
            //    .Where(E => !qbs.Select(q => q.Adres).Contains(E.Adres, new MyComparer())).ToList();

            int counter = 0;
            foreach (var item in resultName) {
                Console.WriteLine($"{counter++}, {item.CompanyName}");
                
            }



            //List<CustomerExact> matchingExacts = [];


            //foreach (var item in exacts) {
            //    foreach (var itemQBS in qbs) {
            //        if (item.CompanyName == itemQBS.CompanyName) {
            //            matchingExacts.Add(item);
            //        } 
            //    }
            //}

            //Console.WriteLine(matchingExacts.Count);
            //Console.WriteLine(exacts.Except(matchingExacts).Count());







            #region init_file_reading_btw


            //using StreamReader readerBTW = new(ExactRecordsPath, true);
            //{
            //    while (!readerBTW.EndOfStream) {
            //        var company = readerBTW.ReadLine().Split(';')[8];
            //        exactsBTW.Add(company);
            //    }
            //};

            //using StreamReader readerBTW2 = new(QBRecordsPath, true);
            //{
            //    while (!readerBTW2.EndOfStream) {
            //        var company = readerBTW2.ReadLine().Split(';')[24];
            //        qbsBTW.Add(company);
            //    }
            //};


            //using StreamReader readerBTW3 = new(QBRecordsPath, true);
            //{
            //    while (!readerBTW3.EndOfStream) {
            //        var company = readerBTW3.ReadLine().Split(';')[23];
            //        qbsBTW.Add(company);
            //    }
            //};

            //using StreamReader readerBTW4 = new(QBRecordsPath, true);
            //{
            //    while (!readerBTW4.EndOfStream) {
            //        var company = readerBTW4.ReadLine().Split(';')[35];
            //        qbsBTW.Add(company);
            //    }
            //};

            //foreach (var item in qbsBTW) {
            //    Console.WriteLine(item);
            //}

            #endregion



            #region init_file_reading_company_address



            //using StreamReader readerAdres = new(ExactRecordsPath, true);
            //{
            //    while (!readerAdres.EndOfStream) {
            //        var company = readerAdres.ReadLine().Split(';')[2];
            //        exactsAdres.Add(company);

            //    }
            //};


            //using StreamReader readerAdres2 = new(QBRecordsPath, true);
            //{
            //    while (!readerAdres2.EndOfStream) {
            //        var company = readerAdres2.ReadLine().Split(';')[21];
            //        qbsAdres.Add(company);
            //    }
            //};


            //using StreamReader readerAdres3 = new(QBRecordsPath, true);
            //{
            //    while (!readerAdres3.EndOfStream) {
            //        var company = readerAdres3.ReadLine().Split(';')[22];
            //        qbsAdres.Add(company);
            //    }
            //};



            //var resultAdres = exactsAdres.Intersect(qbsAdres, new MyComparer());

            //int counter = 0;
            //foreach (var r in resultAdres) {
            //    if (!string.IsNullOrEmpty(r)) {
            //        counter++;
            //        Console.WriteLine($"{counter}) {r}");
            //    }
            //}

            //Console.ReadKey();

            #endregion



















            //var exactsBTWNormal = exactsBTW.Select(BtwNumber).ToList();
            //var qbs2Normal = qbsBTW.Select(BtwNumber).ToList();

            //foreach (var item in qbsBTW) {
            //    Console.WriteLine(item);
            //}

            //var commonNumbers = exactsBTWNormal.Intersect(qbs2Normal);

            //int counter = 0;
            //foreach (var r in commonNumbers) {
            //    counter++;
            //    Console.WriteLine($"{counter}) {r}");
            //}

            //Console.ReadKey();

            //var result = exacts.Intersect(qbs, new MyComparer());

            //int counter = 0;
            //foreach (var r in result) {
            //    counter++;
            //    Console.WriteLine($"{counter}) {r}");
            //}

            //Console.ReadKey();

            static string BtwNumber(string btw) {
                if (btw is null) return string.Empty;
                return Regex.Replace(btw, @"\D", ""); // Verwijdert alle niet-numerieke tekens

            }
        }

        class MyComparer : IEqualityComparer<string> {
            public bool Equals(string? x, string? y) {
                if (x is null || y is null) return false;

                // Clean the strings by removing whitespace and converting to uppercase
                string cleanX = x.Replace(" ", "").ToUpper();
                string cleanY = y.Replace(" ", "").ToUpper();

                // Check if either string contains the other
                return cleanX.Contains(cleanY) || cleanY.Contains(cleanX);
            }

            public int GetHashCode([DisallowNull] string obj) {
                return 0;
            }
        }

    }
}