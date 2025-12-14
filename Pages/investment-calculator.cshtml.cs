using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Finance_Calculator.Pages
{
    public class InvestmentModel : PageModel
    {
        [BindProperty]
        public decimal initial_investment { get; set; }

        [BindProperty]
        public decimal monthly_contribution { get; set; }

        [BindProperty]
        public decimal annual_interest { get; set; }

        [BindProperty]
        public int years { get; set; }

        public InvestmentYearResult[]? Results { get; set; }

        public void OnPost()
        {
            if (years <= 0 || initial_investment < 0 || annual_interest < 0 || monthly_contribution < 0)
            {
                Results = null;
                return;
            }

            Results = new InvestmentYearResult[years];

            decimal balance = initial_investment;

            for (int i = 0; i < years; i++)
            {
                decimal interest = balance * (annual_interest / 100);
                decimal contribution = monthly_contribution * 12;
                decimal endBalance = balance + interest + contribution;

                Results[i] = new InvestmentYearResult
                {
                    Year = i + 1,
                    StartBalance = balance,
                    Contribution = contribution,
                    Interest = interest,
                    EndBalance = endBalance
                };

                balance = endBalance;
            }
        }
    }

    public class InvestmentYearResult
    {
        public int Year { get; set; }
        public decimal StartBalance { get; set; }
        public decimal Contribution { get; set; }
        public decimal Interest { get; set; }
        public decimal EndBalance { get; set; }
    }
}