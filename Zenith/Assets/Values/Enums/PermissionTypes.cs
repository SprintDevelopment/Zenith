using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Assets.Values.Enums
{
    public enum PermissionTypes
    {
        DontCare = -1,
        Buys,
        Sales,
        Accounts,
        Cashes,
        Cheques,
        Companies,
        Sites,
        Materials,
        Mixtures,
        Machines,
        MachineOutgoes,
        Outgoes,
        OutgoCategories,
        MachineIncomes,
        Incomes,
        IncomeCategories,
        Personnel,
        SalaryPayments,
        MachineReport,
        SaleProfitReport,
        CompanyAggregateReport,
        Users,
        Notes,
    }
}
