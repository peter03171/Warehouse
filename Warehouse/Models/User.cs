using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Models
{
    [Table("Users")]
    public class User
    {
        public Int32 UserId { get; set; }
        [Required(ErrorMessage = "請輸入帳號")]
        public String UserName { get; set; }
        [Required(ErrorMessage = "請輸入密碼")]
        [DataType(DataType.Password)]
        public String PassWord { get; set; }
        public String Role { get; set; }
    }
}
