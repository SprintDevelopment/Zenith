using Zenith.Assets.Attributes;
using System;

namespace Zenith.Assets.Values.Enums
{
    public enum PagesOrOperations
    {
        [NavigationLink(Title = "", Icon = "")]
        None = 0,

        [NavigationLink(Title = "قرارداد جدید", Icon = "")]
        Contract,
        [NavigationLink(Title = "لیست قرارداد ها", Icon = "", NavigationPageSource = "Views/Lists/ContractListPage.xaml")]
        ContractList,

        [NavigationLink(Title = "دریافت بدهی جدید", Icon = "")]
        Recieve,
        [NavigationLink(Title = "پرداخت طلب جدید", Icon = "")]
        Payment,
        [NavigationLink(Title = "لیست تراکنش های مشتریان", Icon = "", NavigationPageSource = "Views/Lists/PersonTransactionListPage.xaml")]
        PersonTransactionListPage,
        [NavigationLink(Title = "چک دریافتنی جدید", Icon = "")]
        RecieveableCheque,
        [NavigationLink(Title = "لیست چک های دریافتنی", Icon = "")]
        RecieveableChequeList,
        [NavigationLink(Title = "چک پرداختنی جدید", Icon = "")]
        PayableCheque,
        [NavigationLink(Title = "لیست چک های پرداختنی", Icon = "")]
        PayableChequeList,

        [NavigationLink(Title = "هزینه جدید", Icon = "")]
        Outgo,
        [NavigationLink(Title = "لیست هزینه ها", Icon = "")]
        OutgoList,
        [NavigationLink(Title = "لیست دسته های درآمد و هزینه", Icon = "", NavigationPageSource = "Views/Lists/OutgoCatListPage.xaml")]
        OutgoCatList,
        
        [NavigationLink(Title = "شخص جدید", Icon = "")]
        Person,
        [NavigationLink(Title = "لیست اشخاص", Icon = "", NavigationPageSource = "Views/Lists/PersonListPage.xaml")]
        PersonList,
        [NavigationLink(Title = "لیست گروه های اشخاص", Icon = "")]
        PersonGroupList,

        [NavigationLink(Title = "لیست پیامک ها", Icon = "")]
        SMSList,
        [NavigationLink(Title = "پیامک جدید", Icon = "")]
        SMS,

        [NavigationLink(Title = "داشبورد", Icon = "")]
        Dashboard,
        [NavigationLink(Title = "لیست گزارشات", Icon = "")]
        ReportList,
        [NavigationLink(Title = "گزارش تراکنش های حساب", Icon = "")]
        AccountTransactionReport,
        [NavigationLink(Title = "گزارش تراکنش های کاربر", Icon = "")]
        PersonTransactionReport,
        [NavigationLink(Title = "گزارش مالیات", Icon = "")]
        TaxReport,
        [NavigationLink(Title = "گزارش کاردکس محصول", Icon = "")]
        ProductCartexReport,
        [NavigationLink(Title = "گزارش ارزش محصول", Icon = "")]
        ProductValueReport,
        [NavigationLink(Title = "گزارش هزینه و درآمد", Icon = "")]
        OutgoIncomeReport,
        [NavigationLink(Title = "گزارش اقساط", Icon = "")]
        LoanReport,
        [NavigationLink(Title = "گزارش چک ها", Icon = "")]
        ChequeReport,
        [NavigationLink(Title = "گزارش خرید و فروش", Icon = "")]
        BuySaleReport,
        [NavigationLink(Title = "گزارش کلی", Icon = "")]
        OverallReport,
        [NavigationLink(Title = "گزارش مالی", Icon = "")]
        FinanceTReport,
        [NavigationLink(Title = "گزارش سود و زیان", Icon = "")]
        GainLoseReport,

        [NavigationLink(Title = "لیست کاربران", Icon = "", NavigationPageSource = "Views/Lists/UserListPage.xaml")]
        UserList,
        [NavigationLink(Title = "کاربر جدید", Icon = "")]
        User,
        [NavigationLink(Title = "دسترسی های کاربر", Icon = "")]
        UserPermission,
        [NavigationLink(Title = "تغییر کلمه عبور", Icon = "")]
        ChangePassword,
        [NavigationLink(Title = "تاریخچه ورود و خروج کاربران", Icon = "")]
        UserAccessLogList,

        [NavigationLink(Title = "لیست یادداشت ها", Icon = "", NavigationPageSource = "Views/Lists/NoteListPage.xaml")]
        NoteList,
        [NavigationLink(Title = "تنظیمات نرم افزار", Icon = "")]
        Settings,
        [NavigationLink(Title = "باز کردن فایل لاگ", Icon = "")]
        OpenLogFile,
        [NavigationLink(Title = "لیست کلیدهای میانبر", Icon = "", NavigationPageSource = "Views/Lists/ShortcutListPage.xaml")]
        ShortcutList,
        [NavigationLink(Title = "پشتیبان گیری از داده ها", Icon = "")]
        Backup_Data,
        [NavigationLink(Title = "بازیابی داده ها", Icon = "")]
        Restore_Data,
        [NavigationLink(Title = "فعالسازی نرم افزار", Icon = "")]
        ProductActivation,

        [NavigationLink(Title = "راهنمای متنی", Icon = "")]
        TextHelp,
        [NavigationLink(Title = "درباره نرم افزار", Icon = "")]
        AboutSoftware,
    }
}
