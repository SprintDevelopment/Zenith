using Zenith.Models;

namespace Zenith.Repositories
{
    public class CompanyRepository : Repository<Company>
    {
        public override Company Add(Company company)
        {
            company.CreditValue = company.InitialCreditValue;

            return base.Add(company);
        }
    }
}
