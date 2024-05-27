using AuthService.Core.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Data.UserDatas.Model
{
    public class Seller : BaseEntity
    {
        public string Business_name { get; set; }
        public string Business_address { get; set; }
        public string phonenumber { get; set; }
        public string bank_acc_num { get; set; }
        public string bank_name { get; set; }
        public string reg_num { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
