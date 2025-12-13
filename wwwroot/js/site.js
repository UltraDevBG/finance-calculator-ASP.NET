(() => {
    'use strict'

    const forms = document.querySelectorAll('.needs-validation')
    const allowed_payment_types = ['annuity', 'decreasing'];

    Array.from(forms).forEach(form => {
        form.addEventListener('submit', event => {
            //event.preventDefault()
           // event.stopPropagation()

            const credit_amount = form.elements['credit_amount'];
            const credit_length = form.elements['credit_length'];
            const credit_interest = form.elements['credit_interest'];
            const payment_type = form.elements['payment_type'];
            const promo_period = form.elements['promo_period'];
            const promo_interest = form.elements['promo_interest'];
            const gratis_period = form.elements['gratis_period'];
            const application_fee = form.elements['application_fee'];
            const processing_fee = form.elements['processing_fee'];
            const other_starter_fees = form.elements['other_starter_fees'];
            const yearly_management_fees = form.elements['yearly_management_fees'];
            const yearly_other_fees = form.elements['yearly_other_fees'];
            const monthly_management_fees = form.elements['monthly_management_fees'];
            const monthly_other_fees = form.elements['monthly_other_fees'];

        }, false);
    });

    const validateCreditAmount = (value) => {
        const creditAmount = parseInt(value);
        if (!isNaN(creditAmount)) {
            return false;
        }
    }

    const getFieldOption = (field, form) => {
        const allowed_field_options = ['percent', 'currency'];

        if (!!form.elements[field.id + '_option']) {
            if (allowed_field_options.includes(form.elements[field.id + '_option'].value)) {
                return form.elements[field.id + '_option'].value;
            }
        }
        return false;
    }
})()