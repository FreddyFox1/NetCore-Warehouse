using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Warehouse.Areas.Identity.Pages.Account.Manage
{
    public static class ManageNavPages
    {
        #region Titles
        public static string Index => "Index";
        public static string Email => "Email";
        public static string ChangePassword => "ChangePassword";
        public static string DownloadPersonalData => "DownloadPersonalData";
        public static string DeletePersonalData => "DeletePersonalData";
        public static string ExternalLogins => "ExternalLogins";
        public static string PersonalData => "PersonalData";
        public static string TwoFactorAuthentication => "TwoFactorAuthentication";
        public static string Bitrix => "Bitrix";
        public static string Telegram => "Telegram";
        public static string Users => "Admin";
        public static string Integrator => "Integrator";
        public static string Guide => "Guide";
        public static string Update => "Update";
        #endregion

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);
        public static string EmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, Email);
        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);
        public static string DownloadPersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DownloadPersonalData);
        public static string DeletePersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DeletePersonalData);
        public static string ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ExternalLogins);
        public static string PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);
        public static string TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, TwoFactorAuthentication);

        #region Custom
        public static string UsersNavClass(ViewContext viewContext) => PageNavClass(viewContext, Users);
        public static string BitrixUsersNavClass(ViewContext viewContext) => PageNavClass(viewContext, Bitrix);
        public static string TelegramNavClass(ViewContext viewContext) => PageNavClass(viewContext, Telegram);
        public static string IntegratorNavClass(ViewContext viewContext) => PageNavClass(viewContext, Integrator);
        public static string GuideNavClass(ViewContext viewContext) => PageNavClass(viewContext, Guide);
        public static string UpdateNavClass(ViewContext viewContext) => PageNavClass(viewContext, Update);
        #endregion

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
