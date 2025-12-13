using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Finance_Calculator.Pages
{
    public class CreditModel : PageModel
    {
        private readonly ILogger<CreditModel> _logger;


        [BindProperty]
        public int credit_amount { get; set; }
        [BindProperty]
        public int credit_length { get; set; }
        [BindProperty]
        public decimal credit_interest { get; set; }
        [BindProperty]
        public string payment_type { get; set; }

        [BindProperty]
        public int promo_period { get; set; }
        [BindProperty]
        public decimal promo_interest { get; set; }
        [BindProperty]
        public int gratis_period { get; set; }

        [BindProperty]
        public decimal application_fee { get; set; }
        [BindProperty]
        public string application_fee_option { get; set; }

        [BindProperty]
        public decimal processing_fee { get; set; }
        [BindProperty]
        public string processing_fee_option { get; set; }

        [BindProperty]
        public decimal other_starter_fees { get; set; }
        [BindProperty]
        public string other_starter_fees_option { get; set; }

        [BindProperty]
        public decimal yearly_management_fees { get; set; }
        [BindProperty]
        public string yearly_management_fees_option { get; set; }

        [BindProperty]
        public decimal yearly_other_fees { get; set; }
        [BindProperty]
        public string yearly_other_fees_option { get; set; }

        [BindProperty]
        public decimal monthly_management_fees { get; set; }
        [BindProperty]
        public string monthly_management_fees_option { get; set; }

        [BindProperty]
        public decimal monthly_other_fees { get; set; }
        [BindProperty]
        public string monthly_other_fees_option { get; set; }

        public decimal[,] result;

        public CreditModel(ILogger<CreditModel> logger)
        {
            _logger = logger;
        }

        public void OnPost()
        {
            AssignData();

            if (!Validate())
            {
                return;
            }

            Calculate();
        }

        private void Calculate()
        {
            int years = 0;
            decimal
                initial_fees = 0.0m,
                yearly_fee = 0.0m,
                monthly_fee = 0.0m,
                principal_balance = 0.0m,
                principal_installment = 0.0m,
                monthly_interest = 0.0m,
                total_fees = 0.0m,
                total_interest = 0.0m,
                total_installments = 0.0m;

            if (credit_length > 12)
            {
                years = (credit_length - 1) / 12;
                ViewData["years"] = years;
            }

            initial_fees += calculateFee(application_fee_option, credit_amount, application_fee);
            initial_fees += calculateFee(processing_fee_option, credit_amount, processing_fee);
            initial_fees += calculateFee(other_starter_fees_option, credit_amount, other_starter_fees);

            total_fees += initial_fees;

            principal_balance = (decimal)credit_amount;

            result = new decimal[credit_length + 1, 7];

            result = FillRow(result, 0, 0, 0, 0, principal_balance, initial_fees, credit_amount - initial_fees);

            for (int i = 1; i <= credit_length; i++)
            {
                decimal monthly_installment = 0.0m, money_flow = 0.0m, monthly_fees_total = 0.0m;
                decimal this_month_interest = credit_interest / 100;

                if (i <= promo_period)
                {
                    this_month_interest = promo_interest / 100;
                }

                principal_balance -= principal_installment;
                monthly_interest = principal_balance * this_month_interest / 12;

                if (i > gratis_period)
                {
                    if (payment_type == "descreasing")
                    {
                        principal_installment = credit_amount / (credit_length - gratis_period);
                        monthly_installment = principal_installment + monthly_interest;
                    }
                    else
                    {
                        monthly_installment = (decimal)Microsoft.VisualBasic.Financial.Pmt((double)this_month_interest / 12, credit_length - i + 1, (double)-principal_balance);
                        principal_installment = monthly_installment - monthly_interest;
                    }
                }
                else
                {
                    monthly_installment = monthly_interest;
                }

                monthly_fees_total += calculateFee(monthly_management_fees_option, principal_balance, monthly_management_fees);
                monthly_fees_total += calculateFee(monthly_other_fees_option, principal_balance, monthly_other_fees);

                if (i % 12 == 0)
                {
                    monthly_fees_total += calculateFee(yearly_management_fees_option, principal_balance, yearly_management_fees);
                    monthly_fees_total += calculateFee(yearly_other_fees_option, principal_balance, yearly_other_fees);
                }

                total_interest += monthly_interest;
                total_installments += monthly_installment;
                total_fees += monthly_fees_total;

                money_flow = -(monthly_fees_total + monthly_installment);

                result = FillRow(result, i, monthly_installment, principal_installment, monthly_interest, principal_balance, monthly_fees_total, money_flow);
            }

            decimal total_paid = total_installments + total_fees;
            double interestGPR = Microsoft.VisualBasic.Financial.Rate(credit_length, (double)-(total_paid / credit_length), credit_amount) * 12;
            decimal GPR = (decimal)Math.Pow((interestGPR / 12) + 1.0, 12) - 1;

            ViewData["result"] = result;
        }

        private decimal calculateFee(string option, decimal total, decimal fee)
        {
            if (option == "percent")
            {
                return total * (fee / 100);
            }
            else
            {
                return fee;
            }
        }

        private void AssignData()
        {
            ViewData["credit_amount"] = credit_amount;
            ViewData["credit_length"] = credit_length;
            ViewData["credit_interest"] = credit_interest;
            ViewData["payment_type"] = payment_type;
            ViewData["promo_period"] = promo_period;
            ViewData["promo_interest"] = promo_interest;
            ViewData["gratis_period"] = gratis_period;
            ViewData["application_fee"] = application_fee;
            ViewData["application_fee_option"] = application_fee_option;
            ViewData["processing_fee"] = processing_fee;
            ViewData["processing_fee_option"] = processing_fee_option;
            ViewData["other_starter_fees"] = other_starter_fees;
            ViewData["other_starter_fees_option"] = other_starter_fees_option;
            ViewData["yearly_management_fees"] = yearly_management_fees;
            ViewData["yearly_management_fees_option"] = yearly_management_fees_option;
            ViewData["yearly_other_fees"] = yearly_other_fees;
            ViewData["yearly_other_fees_option"] = yearly_other_fees_option;
            ViewData["monthly_management_fees"] = monthly_management_fees;
            ViewData["monthly_management_fees_option"] = monthly_management_fees_option;
            ViewData["monthly_other_fees"] = monthly_other_fees;
            ViewData["monthly_other_fees_option"] = monthly_other_fees_option;

            return;
        }

        private bool Validate()
        {

            bool has_error = false;

            if (credit_amount < 100 || credit_amount > 10000000)
            {
                has_error = MarkError("credit_amount_error");
            }
            if (credit_length <= 0 || credit_length > 960)
            {
                has_error = MarkError("credit_length_error");
            }
            if (credit_interest < 0 || credit_interest > 500)
            {
                has_error = MarkError("credit_interest_error");
            }
            if (payment_type != "annuity" && payment_type != "descreasing")
            {
                has_error = MarkError("payment_type_error");
            }

            if (promo_period > credit_length || promo_period < 0)
            {
                has_error = MarkError("promo_period_error");
            }
            if (promo_interest > credit_interest || promo_interest < 0)
            {
                has_error = MarkError("promo_interest_error");
            }

            if (!ValidateFeeField("application_fee", application_fee, application_fee_option))
            {
                has_error = true;
            }
            if (!ValidateFeeField("processing_fee", processing_fee, processing_fee_option))
            {
                has_error = true;
            }
            if (!ValidateFeeField("other_starter_fees", other_starter_fees, other_starter_fees_option))
            {
                has_error = true;
            }
            if (!ValidateFeeField("yearly_management_fees", yearly_management_fees, yearly_management_fees_option))
            {
                has_error = true;
            }
            if (!ValidateFeeField("yearly_other_fees", yearly_other_fees, yearly_other_fees_option))
            {
                has_error = true;
            }
            if (!ValidateFeeField("monthly_management_fees", monthly_management_fees, monthly_management_fees_option))
            {
                has_error = true;
            }
            if (!ValidateFeeField("monthly_other_fees", monthly_other_fees, monthly_other_fees_option))
            {
                has_error = true;
            }

            ViewData["has_error"] = has_error;

            if (has_error)
            {
                return false;
            }
            return true;
        }

        private bool ValidateFeeField(string field_name, decimal value, string option)
        {
            if (option == "currency")
            {
                if (value < 0 || value > credit_amount)
                {
                    return !MarkError(field_name + "_error");
                }
            }
            else if (option == "percent")
            {
                if (value < 0 || value > 40)
                {
                    return !MarkError(field_name + "_error");
                }
            }
            else
            {
                return !MarkError(field_name + "_option_error");
            }
            return true;
        }

        private bool MarkError(string field_name)
        {
            ViewData[field_name] = "is-invalid";
            return true;
        }

        protected decimal[,] FillRow(decimal[,] array, int num, decimal mesecVnoska, decimal vnoskaGlavnica, decimal vnoskaLihva, decimal ostatukGlavnica, decimal taksi, decimal potok)
        {
            array[num, 0] = num;
            array[num, 1] = Decimal.Round(mesecVnoska, 2, MidpointRounding.AwayFromZero);
            array[num, 2] = Decimal.Round(vnoskaGlavnica, 2, MidpointRounding.AwayFromZero);
            array[num, 3] = Decimal.Round(vnoskaLihva, 2, MidpointRounding.AwayFromZero);
            array[num, 4] = Decimal.Round(ostatukGlavnica, 2, MidpointRounding.AwayFromZero);
            array[num, 5] = Decimal.Round(taksi, 2, MidpointRounding.AwayFromZero);
            array[num, 6] = Decimal.Round(potok, 2, MidpointRounding.AwayFromZero);

            return array;
        }
    }
}