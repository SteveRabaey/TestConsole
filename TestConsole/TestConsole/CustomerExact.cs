using System.Globalization;
using System.Text.RegularExpressions;

namespace TestConsole {
    public record CustomerExact {

        private string _companyName = string.Empty;
        public string CompanyName {
            get => _companyName;
            set {
                _companyName = BasicSanitization(value);
            }
        }

        private string _btw = string.Empty;

        public string Btw {
            get => _btw;
            init => _btw = BasicSanitization(SanitizeBTW(value));
        }

        private string _address = string.Empty;
        public string Address {
            get => _address;
            init {
                _address = BasicSanitization(value);
            }
        }
        public string? Email { get; set; }
        public string? MainPhone { get; set; }
        public string? Plaats { get; set; }
        public string? Land { get; set; }
        public string? OriginalBtw { get; set; }

        public bool IsCompanyNamaEqual(string quickbookCompanyName) {
            if (CompanyName is null || quickbookCompanyName is null) return false;

            return CultureInfo.InvariantCulture.CompareInfo.Compare(CompanyName, quickbookCompanyName, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0;
        }

        public bool IsAddressEqual(List<string> quickbookAddresses) {
            if (Address is null || quickbookAddresses == null || quickbookAddresses.Count == 0)
                return false;

            return quickbookAddresses.Any(qbAddress =>
                CultureInfo.InvariantCulture.CompareInfo.Compare(
                    Address,
                    qbAddress,
                    CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase
                ) == 0
            );
        }

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
