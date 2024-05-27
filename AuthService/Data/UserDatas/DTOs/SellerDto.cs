using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Data.UserDatas.DTOs
{
    public class SellerDto
    {
        //public string Guid { get; set; }
        public string Business_name { get; set; }
        public string Business_address { get; set; }
        public string phonenumber { get; set; }
        public string bank_acc_num { get; set; }
        public string bank_name { get; set; }
        public string reg_num { get; set; }
        public string UserId { get; set; }
    }
}
