using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Finance_Calculator.Pages
{
    public class PrepaymentModel : PageModel
    {
        [BindProperty]
        public decimal loan_amount { get; set; }

        [BindProperty]
        public int remaining_months { get; set; }

        [BindProperty]
        public decimal interest_rate { get; set; }

        [BindProperty]
        public decimal prepayment_amount { get; set; }

        [BindProperty]
        public string prepayment_type { get; set; } = "reduce_term";

        public decimal[,]? Result { get; set; }

        public void OnPost()
        {
            if (!ModelState.IsValid) return;

            CalculatePrepayment();
        }

        private void CalculatePrepayment()
        {
            decimal monthly_rate = interest_rate / 100 / 12;

            int new_term;
            decimal monthly_payment;
            decimal principal_balance;

            if (prepayment_type == "reduce_term")
            {
                // Оригиналната вноска за остатъка от кредита
                decimal original_payment = (decimal)Microsoft.VisualBasic.Financial.Pmt(
                    (double)monthly_rate,
                    remaining_months,
                    (double)-loan_amount
                );

                // Нов баланс след предсрочно погасяване
                decimal new_balance = loan_amount - prepayment_amount;

                // Изчисляване на нов брой месеци
                if (original_payment - new_balance * monthly_rate <= 0)
                {
                    new_term = remaining_months; // безопасна проверка
                }
                else
                {
                    new_term = (int)Math.Ceiling(Math.Log(
                        (double)(original_payment / (original_payment - new_balance * monthly_rate))
                    ) / Math.Log(1 + (double)monthly_rate));
                }

                principal_balance = new_balance;
                monthly_payment = original_payment; // вноската остава същата
            }
            else // reduce_payment
            {
                decimal new_balance = loan_amount - prepayment_amount;
                monthly_payment = (decimal)Microsoft.VisualBasic.Financial.Pmt(
                    (double)monthly_rate,
                    remaining_months,
                    (double)-new_balance
                );
                principal_balance = new_balance;
                new_term = remaining_months;
            }

            // Генериране на таблица с резултати
            Result = new decimal[new_term, 5];

            for (int i = 0; i < new_term; i++)
            {
                decimal interest = principal_balance * monthly_rate;
                decimal principal = monthly_payment - interest;

                // Корекция на последния месец, ако останалата главница е по-малка
                if (principal_balance - principal < 0)
                {
                    principal = principal_balance;
                    monthly_payment = principal + interest;
                }

                principal_balance -= principal;
                if (principal_balance < 0) principal_balance = 0;

                Result[i, 0] = i + 1;
                Result[i, 1] = Decimal.Round(monthly_payment, 2);
                Result[i, 2] = Decimal.Round(principal, 2);
                Result[i, 3] = Decimal.Round(interest, 2);
                Result[i, 4] = Decimal.Round(principal_balance, 2);
            }
        }
    }
}
