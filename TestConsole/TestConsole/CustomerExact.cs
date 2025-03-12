using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole {
    internal record CustomerExact {
        private string _company;
        public string CompanyName {
            get => _company;
            init => _company = value.ToUpper().Trim();
        }
        public string BTW { get; init; }
        public string Adres { get; init; }
    }
}
