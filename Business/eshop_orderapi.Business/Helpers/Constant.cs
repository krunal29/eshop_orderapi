namespace eshop_orderapi.Business.Helpers
{
    public static class Constant
    {
        public const string AppicationUserData = "ApplicationUserData";
        public const string EmailRegex = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
        public const string DateFormat = "dd/MM/yyyy";
        public const string DateTimeFormat = "dd-MM-yyyy HH:mm";
        public const int AdminId = 1;

        #region MemoryCatch

        public const string Memory_UserRoles = "Memory_UserRoles";

        #endregion MemoryCatch
    }
}