//using CraftDailyCorner.ViewModels.Front;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;

//namespace CraftDailyCorner.ViewModels.Front
//{
//    public class VMCheckout
//    {

//        public List<VMCartItem> Items { get; set; } = new();

//        public int TotalAmount { get; set; }
//        [StringLength(20,MinimumLength =2)]
//        [Required(ErrorMessage ="請輸入收件人姓名")]
//        [Display(Name ="收件人姓名")]
//        public string ReceiverName { get; set; } =null!;
//        [StringLength(10, MinimumLength = 10)]
//        [Required(ErrorMessage ="請輸入收件人手機號碼")]
//        [Display(Name ="收件人手機號碼")]
//        public string ReceiverPhone { get; set; } = null!;
//        [StringLength(50)]
//        [Required(ErrorMessage ="請輸入收件地址")]
//        [Display(Name ="收件地址")]
//        public string ReceiverAddress { get; set; } = null!;
//    }
//}
