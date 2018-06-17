import validators from 'utils/validators';
import {
    REQUIRED,
    EMAIL,
    PASSWORDS_MATCH,
} from 'constants/validationMessages';

const validationMessages = {
    isFilled: REQUIRED,
    isEmail: EMAIL,
}

const validateForm = form => {
    let isFormValid = true;

    Object.keys(form).forEach(key => {
        if (!form[key].validators) {
            return;
        }

        const errorMessage = form[key].validators
            .reduce((errorMessage, validatorKey) => {
                const isFieldValid = validators[validatorKey](form[key].value);

                if (isFieldValid) {
                    return errorMessage;
                }

                isFormValid = false;

                return validationMessages[validatorKey];
            }, '');

        form[key].error = errorMessage;
    });

    return { form, isFormValid };
}

export default validateForm;
