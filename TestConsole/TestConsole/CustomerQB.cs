using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole {
    internal record CustomerQB
    {
        private string _company;
        public string CompanyName {
            get => _company;
            init => _company = value.ToUpper().Trim();
        }
        public string[] BTW { get; init; } = new string[3];
        public string[] Adres { get; init; } = new string[2];
    }
}
