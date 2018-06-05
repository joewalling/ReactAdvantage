import React, { Component } from 'react';
import { GridWrapper, GridItem } from "components/Grid";
import Input from 'components/Input';
import Button from 'components/Button';
import Checkbox from 'components/Checkbox';
import validators from 'utils/validators';
import {
    REQUIRED,
    EMAIL,
    PASSWORDS_MATCH,
} from 'constants/validationMessages';
import logo from 'assets/logo.png';
import './index.css';

const validationMessages = {
    isFilled: REQUIRED,
    isEmail: EMAIL,
    passwordsMatch: PASSWORDS_MATCH,
}

export default class Login extends Component {
    onChange = (name, value) => {
        const state = {...this.state};
        state.form[name].value = value;

        this.setState(state);
    }

    onTextChange = ({ target: { name, value } }) => {
        this.onChange(name, value);
    }

    onCheckboxChange = ({ checked }) => {
        this.onChange('rememberMe', checked);
    }

    onSubmit = event => {
        event.preventDefault();

        const isFormValid = this.validateForm();

        if (!isFormValid) {
            return;
        }

        const { form } = this.state;
        const formValues = Object.keys(form).reduce((values, key) => {
            values[key] = form[key].value;

            return values;
        }, {});

        console.log('Success! Form values will be bellow');
        console.log(formValues);

        if (this.state.formType === 'forgotPassword') {
            this.setState({
                form: this.formStates.resetPassword,
                formType: 'resetPassword',
            });
        }

        if (this.state.formType === 'resetPassword') {
            this.setState({
                form: this.formStates.login,
                formType: 'login',
            });
        }

        this.resetCurrentForm();
    }

    onForgotPasswordClick = event => {
        event.preventDefault();

        const state = {...this.state};
        state.formType = 'forgotPassword';
        state.form = { ...this.formStates[state.formType] };

        this.setState(state);
    }

    onForgotPasswordBackClick = event => {
        event.preventDefault();
        this.resetCurrentForm();
        const state = {...this.state};
        state.formType = 'login';
        state.form = { ...this.formStates[state.formType] };

        this.setState(state);
    }

    get title() {
        const { formType } = this.state;
        let title;

        switch (formType) {
            case 'forgotPassword':
                title = 'Forgot password?';
                break;
            case 'resetPassword':
                title = 'Reset password';
                break;
            default:
                title = 'Log in';
                break;
        }

        return title;
    }

    get description() {
        const { formType } = this.state;

        return formType === 'forgotPassword'
            ? 'A password resent link will be sent to your email to reset your password. If you don\'t get an email within a few minutes, please re-try.'
            : ''
    }

    formStates = {
        login: {
            name: {
                value: '',
                validators: ['isFilled'],
                error: '',
            },
            password: {
                value: '',
                validators: ['isFilled'],
                error: '',
            },
            rememberMe: {
                value: false
            },
        },
        forgotPassword: {
            email: {
                value: '',
                validators: ['isEmail', 'isFilled'],
                error: '',
            }
        },
        resetPassword: {
            password: {
                value: '',
                validators: ['isFilled'],
                error: '',
            },
            repeatPassword: {
                value: '',
                validators: ['isFilled'],
                error: '',
            }
        }
    }

    state = {
        formType: 'login',
        form: this.formStates.login
    }

    resetCurrentForm() {
        const { form } = this.state;

        Object.keys(form).forEach(key => {
            form[key].value = '';
            form[key].error = '';
        });
    }

    validateForm() {
        const { form } = this.state;
        let formValid = true;

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

                    formValid = false;

                    return validationMessages[validatorKey];
                }, '');

            form[key].error = errorMessage;
        });

        if (
            this.state.formType === 'resetPassword' &&
            form.password.value !== form.repeatPassword.value
        ) {
            form.repeatPassword.error = validationMessages.passwordsMatch;
            // highlight field without error text
            form.password.error = '\r';
            formValid = false;
        }

        this.setState({
            form,
            ...this.state,
        });

        return formValid;
    }

    renderResetPasswordForm() {
        const { form } = this.state;

        return [
            <div key="new-password" className="login-input-wrapper">
                <Input
                    password
                    placeholder="New password"
                    name="password"
                    value={form.password.value}
                    error={form.password.error}
                    onChange={this.onTextChange}
                />
            </div>,
            <div key="new-password-confirm" className="login-input-wrapper">
                <Input
                    password
                    placeholder="Repeat new password"
                    name="repeatPassword"
                    value={form.repeatPassword.value}
                    error={form.repeatPassword.error}
                    onChange={this.onTextChange}
                />
            </div>,
            <div key="actions" className="login-input-wrapper login-input-wrapper-actions">
                <div className="login-button-wrapper">
                    <Button
                        className="login-forgot-password"
                        onClick={this.onSubmit}
                    >
                        Save
                    </Button>
                </div>
            </div>
        ];
    }

    renderForgotPasswordForm() {
        const { form } = this.state;

        return [
            <div key="forgot-password" className="login-input-wrapper">
                <Input
                    placeholder="Email"
                    name="email"
                    value={form.email.value}
                    error={form.email.error}
                    onChange={this.onTextChange}
                />
            </div>,
            <div key="actions" className="login-input-wrapper login-input-wrapper-actions">
                <div className="login-button-wrapper">
                    <Button
                        onClick={this.onForgotPasswordBackClick}
                        secondary
                    >
                        Back
                    </Button>
                </div>
                <div className="login-button-wrapper">
                    <Button
                        className="login-forgot-password"
                        onClick={this.onSubmit}
                    >
                        Submit
                    </Button>
                </div>
            </div>
        ];
    }

    renderLoginForm() {
        const { form } = this.state;

        return [
            <div key="name" className="login-input-wrapper">
                <Input
                    placeholder="Username"
                    name="name"
                    value={form.name.value}
                    error={form.name.error}
                    onChange={this.onTextChange}
                />
            </div>,
            <div key="password" className="login-input-wrapper">
                <Input
                    password
                    placeholder="Password"
                    name="password"
                    value={form.password.value}
                    error={form.password.error}
                    onChange={this.onTextChange}
                />
            </div>,
            <div key="actions" className="login-input-wrapper login-input-wrapper-actions">
                <div className="login-button-wrapper">
                    <Button onClick={this.onSubmit}>
                        Log in
                    </Button>
                </div>
                <div className="login-checkbox-wrapper">
                    <Checkbox
                        inputId="remember-me"
                        label="Remember me"
                        name="rememberMe"
                        checked={Boolean(form.rememberMe.value)}
                        value={form.rememberMe.value}
                        onChange={this.onCheckboxChange}
                    />
                </div>
                <div className="login-button-wrapper">
                    <button
                        className="login-forgot-password"
                        onClick={this.onForgotPasswordClick}
                    >
                        Forgot password?
                    </button>
                </div>
            </div>
        ];
    }

    renderForm() {
        const { formType } = this.state;
        let formContent;

        switch (formType) {
            case 'forgotPassword':
                formContent = this.renderForgotPasswordForm();
                break;
            case 'resetPassword':
                formContent = this.renderResetPasswordForm();
                break;
            default:
                formContent = this.renderLoginForm();
                break;
        }

        return (
            <form
                className="login-form"
                onSubmit={this.onSubmit}
            >
                {formContent}
            </form>
        )
    }

    render() {
        const { description, title } = this;
        return (
            <div className="login">
                <GridWrapper>
                    <GridItem gridClassNames="ui-md-12">
                        <div className="login-content-wrapper">
                            <div className="login-logo">
                                <img src={logo} alt="" />
                            </div>
                            <h2>{title}</h2>
                            {description && <p>{description}</p>}
                            {this.renderForm()}
                        </div>
                    </GridItem>
                </GridWrapper>
            </div>
        );
    }
}
