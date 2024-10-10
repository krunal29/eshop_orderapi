namespace eshop_orderapi.Business.ViewModels
{
    public class ApplicationUserApiRequestModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int RoleId { get; set; }

        public string AspNetUserId { get; set; }

        public string EmailId { get; set; }

        public string AuthToken { get; set; }
    }
}