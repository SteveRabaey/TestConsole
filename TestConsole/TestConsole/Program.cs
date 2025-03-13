using System.Collections.Generic;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using TestConsole;
using ClosedXML.Excel;

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
                        Btw = parts[8],
                        OriginalBtw = parts[8],
                        Address = parts[2],
                        Plaats = parts[3],
                        Land = parts[4],

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
                        BtwList = [parts[23], parts[24], parts[35]],
                        AddressList = [parts[21], parts[22]],
                        Email = parts[19],
                        MainPhone = parts[14]
                    };
                    qbs.Add(c);
                };
            }


           
            #endregion

            List<CustomerExact> matchingcustomersonbtw = [];
            int i = 1;

            foreach (var item in exacts) {
                foreach (var qb in qbs) {
                    foreach (var btw in qb.BtwList) {

                        if (!string.IsNullOrEmpty(btw)) {
                            if (btw == item.Btw) {
                                //Console.WriteLine($"ExactBTW: {item.Btw} ===== QBBTW {btw}  -> company: {item.CompanyName}");

                                item.Email = qb.Email;
                                item.MainPhone = qb.MainPhone;


                                matchingcustomersonbtw.Add(item);
                            }
                        } else {
                            continue;
                        }

                    }


                }
            }

            matchingcustomersonbtw = matchingcustomersonbtw.DistinctBy(x => x.Btw).ToList();
           

            List<CustomerExact> matchingcustomersonName = [];
            int counter = 1;

            foreach (var c in exacts) {
                foreach (var q in qbs) {
                    if (c.IsCompanyNamaEqual(q.CompanyName)) {
                        matchingcustomersonbtw.Add(c);
                        c.MainPhone = q.MainPhone;
                        c.Email = q.Email;
                    }
                }
            }

            matchingcustomersonbtw = matchingcustomersonbtw.DistinctBy(x => x.CompanyName).ToList();



            foreach (var item in exacts) {
                foreach (var qb in qbs) {
                    if (item.IsAddressEqual(qb.AddressList)) {
                        matchingcustomersonbtw.Add(item);
                        item.Email = qb.Email;
                        item.MainPhone = qb.MainPhone;
                    }
                }
            }

            matchingcustomersonbtw = matchingcustomersonbtw.DistinctBy(x => x.CompanyName).ToList();
            List<CustomerExact> emptyList = [];
            emptyList = exacts.Except(matchingcustomersonbtw).ToList();

            string filePath = @"C:\Users\steve\Documents\nl-NL-Media-resources-files-templates-benl-accounts-xls.xlsx";


            using (var workbook = new XLWorkbook(filePath)) {
                IXLWorksheet worksheet;
                worksheet = workbook.Worksheet("Invoer relaties");

                int row = 3;
                foreach (var item in matchingcustomersonbtw) {
                    worksheet.Cell(row, 1).Value = item.CompanyName;
                    worksheet.Cell(row, 8).Value = item.Address;
                    worksheet.Cell(row, 12).Value = item.Plaats;
                    worksheet.Cell(row, 13).Value = item.Land;
                    worksheet.Cell(row, 14).Value = item.Email;
                    worksheet.Cell(row, 16).Value = item.MainPhone;
                    worksheet.Cell(row, 20).Value = item.OriginalBtw;
                    worksheet.Cell(row, 28).Value = item.Email;

                    row++;
                }
                foreach (var emty in emptyList) {
                    worksheet.Cell(row, 1).Value = emty.CompanyName;
                    worksheet.Cell(row, 8).Value = emty.Address;
                    worksheet.Cell(row, 12).Value = emty.Plaats;
                    worksheet.Cell(row, 13).Value = emty.Land;
                    worksheet.Cell(row, 14).Value = emty.Email;
                    worksheet.Cell(row, 16).Value = emty.MainPhone;
                    worksheet.Cell(row, 20).Value = emty.OriginalBtw;
                    worksheet.Cell(row, 28).Value = emty.Email;
                    row++;
                }
                workbook.Save();
            }
            Console.WriteLine("Data succesvol ingevoegd in het Excel-bestand vanaf rij 2.");



            ////matchingcustomersonbtw = matchingcustomersonbtw.distinctby(x => x.companyname).tolist();

            //matchingcustomersonbtw = matchingcustomersonbtw.Except(matchingcustomersonName).ToList();




            //List<CustomerQB> qbNames = qbs
            //    .Select(q => q.CompanyName).ToList();


            //    .Contains(E.CompanyName, new MyComparer())).ToList();


            // resterende niet matchende customers
            //List<CustomerExact> NotMatchingListAfterName = exacts
            //    .Where(E => !qbs.Select(q => q.CompanyName).Contains(E.CompanyName, new MyComparer())).ToList();




            //List<CustomerExact> NotMatchingListAfterBTW = NotMatchingListAfterName

            //   .Where(E => !qbs.All(q => q.BTWList.All(x => BtwNumber(E.BTW) != BtwNumber(x)))).ToList();





            //List<CustomerExact> NotMatchingListAfterAdres = NotMatchingListAfterBTW
            //    .Where(E => !qbs.Select(q => q.AdresList.Select))





            //List<CustomerExact> NotMatchingListAfterAdres = NotMatchingListAfterBTW.Where(c => !c.Adres.Equals(qbs.ForEach(x => x.AdresList.ForEach(QBadres => QBadres.Contains(c.Adres)))




            //matchingCustomersOnAddress = matchingCustomersOnAddress.DistinctBy(x => x.CompanyName).ToList();

            //matchingCustomersOnAddress = matchingCustomersOnBtw.Except(matchingCustomersOnAddress).ToList();

            //int counter = 0;
            //foreach (var item in matchingCustomersOnAddress) {
            //    Console.WriteLine($"{counter++}, {item.CompanyName} --------- BTW: {item.BTW} --------- ADRES: {item.Adres}");

            //}



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




        }
    }

    class MyComparer : IEqualityComparer<string> {
        public bool Equals(string? x, string? y) {
            if (string.IsNullOrEmpty(x) || string.IsNullOrEmpty(y)) return false;

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
