import React from 'react';

import validators from './validators';
import validateForm from './validateForm';

describe('"email" validator is working', () => {
    test('correct email is passing through validator', () => {
        expect(validators.isEmail('test@test.com')).toEqual(true);
    });

    test('incorrect email isn\'t passing through validator', () => {
        expect(validators.isEmail('test@com.123')).toEqual(false);
    });
});

describe('"required" validator is working', () => {
    test('not empty value is passing through validator', () => {
        expect(validators.isFilled('test')).toEqual(true);
    });

    test('empty value isn\'t passing through validator', () => {
        expect(validators.isFilled('')).toEqual(false);
    });
});

describe('"validateForm" util is working', () => {
    test('"validateForm" is working with correct value', () => {
        const formValues = {
            test: {
                value: 'test@test.com',
                error: '',
                validators: ['isFilled', 'isEmail'],
            }
        }

        const { isFormValid } = validateForm(formValues);

        expect(isFormValid).toEqual(true);
    });

    test('"validateForm" is returning error text', () => {
        const formValues = {
            test: {
                value: 'test@test.123',
                error: '',
                validators: ['isFilled', 'isEmail'],
            }
        }

        const { form } = validateForm(formValues);

        expect(form.test.error).toBeTruthy();
    });
});
