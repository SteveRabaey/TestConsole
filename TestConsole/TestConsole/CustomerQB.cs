using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TestConsole {
    internal record CustomerQB {


        private string _companyName = string.Empty;
        public string CompanyName {
            get => _companyName;
            set {
                _companyName = BasicSanitization(value);
            }
        }

        private List<string> _btwList = [];

        public List<string> BtwList {
            get => _btwList;
            init => _btwList = value?.Select(btw => SanitizeBTW(btw).Trim()).ToList() ?? [];
        }

        private List<string> _addresslist = [];
        public List<string> AddressList {
            get => _addresslist;
            init {
                _addresslist = value;
            }
        }

        public string? Email { get; set; }
        public string? MainPhone { get; set; }


        public string BasicSanitization(string s) {
            return s.Trim();
        }

        public string SanitizeBTW(string btw) {
            if (btw is null) {
                return string.Empty;
            }
            return Regex.Replace(btw, @"\D", "");
        }








    }
}
