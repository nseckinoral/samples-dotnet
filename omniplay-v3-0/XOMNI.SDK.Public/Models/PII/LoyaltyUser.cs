using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.PII
{
    public class LoyaltyUser : User
    {
        public double? AvailablePoints { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string PhoneNumber { get; set; }
        public string EMailAddress { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public override string PIIUserType
        {
            get
            {
                return "Loyalty";
            }
        }
    }
}
