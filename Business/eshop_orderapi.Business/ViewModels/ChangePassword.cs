namespace eshop_orderapi.Business.ViewModels
{
    public class ChangePassword
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}