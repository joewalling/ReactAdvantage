import React, { Component } from 'react';

import FileUpload from 'components/FileUpload';
import Input from 'components/Input';
import ModalPopup from 'components/ModalPopup';
import Button from 'components/Button';
import validateForm from 'utils/validateForm';
import { PASSWORDS_MATCH } from 'constants/validationMessages';

import avatarSrc from './assets/profile-default-image.svg'
import './index.css';

export default class UserSettings extends Component {
    onHide = () => {
        this.props.onHide();
    }

    onSubmit = event => {
        event.preventDefault();
        const isFormValid = this.validateForm();

        if (!isFormValid) {
            return;
        }

        const { onSave } = this.props;
        const { form } = this.state;
        const formValues = Object.keys(form).reduce((values, key) => {
            values[key] = form[key].value;

            return values;
        }, {});

        onSave(formValues)
    }

    onChange = ({ target: { name, value } }) => {
        const state = {...this.state};
        state.form[name].value = value;

        this.setState(state);
    }

    onMaskChange = ({ originalEvent }) => {
        this.onChange(originalEvent);
    }

    onBeforeUpload = ({ xhr, files }) => {
        const { form } = this.state;

        form.image.value = files[0];
        this.setState({
            profileSrc: files[0].objectURL,
            form,
        });
    }

    state = {
        profileSrc: avatarSrc,
        form: {
            firstName: {
                value: 'admin',
                validators: ['isFilled'],
                error: '',
            },
            lastName: {
                value: 'admin',
                validators: ['isFilled'],
                error: '',
            },
            email: {
                value: 'jwalling@jwallingis.com',
                validators: ['isEmail', 'isFilled'],
                error: '',
            },
            phone: {
                value: '',
            },
            name: {
                value: 'admin',
            },
            password: {
                value: '',
            },
            repeatPassword: {
                value: '',
            },
            image: {
                value: '',
            }
        }
    }

    validateForm() {
        const { form: defaultForm } = this.state;
        let { isFormValid, form } = validateForm(defaultForm);

        if (form.password.value !== form.repeatPassword.value) {
            form.password.error = '\r';
            form.repeatPassword.error = PASSWORDS_MATCH;
            isFormValid = false;
        } else {
            form.password.error = '';
            form.repeatPassword.error = '';
        }

        this.setState({ form });

        return isFormValid;
    }

    renderActions() {
        return (
            <div>
                <Button
                    secondary
                    onClick={this.onHide}
                >
                    Cancel
                </Button>
                <Button
                    onClick={this.onSubmit}
                >
                    Save
                </Button>
            </div>
        );
    }

    renderForm() {
        const { form } = this.state;

        return (
            <div className="profile-form">
                <form onSubmit={this.onSubmit}>
                    <div className="profile-form-upload">
                        <label>
                            <FileUpload
                                className="custom-file-upload"
                                auto
                                mode="basic"
                                onSelect={this.onBeforeUpload}
                            />
                            <img src={this.state.profileSrc} alt="User" />
                            <div className="profile-form-upload-hint">
                                Click here to change image
                            </div>
                        </label>
                    </div>
                    <div className="profile-form-input-wrapper profile-form-input-wrapper-right">
                        <label
                            className="profile-form-label"
                            htmlFor="first-name"
                        >
                            First Name <sup>*</sup>
                         </label>
                        <Input
                            id="first-name"
                            value={form.firstName.value}
                            error={form.firstName.error}
                            name="firstName"
                            onChange={this.onChange}
                        />
                    </div>
                    <div className="profile-form-input-wrapper profile-form-input-wrapper-right">
                        <label
                            className="profile-form-label"
                            htmlFor="last-name"
                        >
                            Last Name <sup>*</sup>
                        </label>
                        <Input
                            id="last-name"
                            value={form.lastName.value}
                            error={form.lastName.error}
                            name="lastName"
                            onChange={this.onChange}
                        />
                    </div>
                    <div className="profile-form-input-wrapper profile-form-input-wrapper-clear">
                        <label
                            className="profile-form-label"
                            htmlFor="email-address"
                        >
                            Email Address <sup>*</sup>
                        </label>
                        <Input
                            id="email-address"
                            value={form.email.value}
                            error={form.email.error}
                            name="email"
                            onChange={this.onChange}
                        />
                    </div>
                    <div className="profile-form-input-wrapper">
                        <label
                            className="profile-form-label"
                            htmlFor="phone-number"
                        >
                            Phone Number
                        </label>
                        <Input
                            id="phone-number"
                            value={form.phone.value}
                            name="phone"
                            onChange={this.onMaskChange}
                            mask="(999) 999-9999"
                        />
                    </div>
                    <div className="profile-form-input-wrapper">
                        <label
                            className="profile-form-label"
                        >
                            User Name <sup>*</sup>
                        </label>
                        <Input
                            disabled
                            value={form.name.value}
                        />
                        <div className="profile-form-tip">
                            Can not change username of admin
                        </div>
                    </div>
                    <div className="profile-form-input-wrapper">
                        <label
                            className="profile-form-label"
                            htmlFor="password"
                        >
                            Password
                        </label>
                        <Input
                            id="password"
                            value={form.password.value}
                            error={form.password.error}
                            name="password"
                            onChange={this.onChange}
                            password
                        />
                    </div>
                    <div className="profile-form-input-wrapper">
                        <label
                            className="profile-form-label"
                            htmlFor="repeat-password"
                        >
                            Repeat Password
                        </label>
                        <Input
                            id="repeat-password"
                            value={form.repeatPassword.value}
                            error={form.repeatPassword.error}
                            name="repeatPassword"
                            onChange={this.onChange}
                        />
                    </div>
                </form>
            </div>
        );
    }

    render() {
        return (
            <div>
                <ModalPopup
                    visible={this.props.visible}
                    onHide={this.props.onHide}
                    header="Edit user"
                    className="popup"
                    footer={this.renderActions()}
                    modal
                >
                    {this.renderForm()}
                </ModalPopup>
            </div>
        );
    }
}
