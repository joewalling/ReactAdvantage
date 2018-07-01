import React, { Component } from 'react';

import avatarSrc from 'assets/profile-default-image.svg';

import TabView, { TabPanel } from 'components/TabView';
import ModalPopup from 'components/ModalPopup';
import FileUpload from 'components/FileUpload';
import Input from 'components/Input';
import Checkbox from 'components/Checkbox';
import Button from 'components/Button';
import validateForm from 'utils/validateForm';
import { PASSWORDS_MATCH } from 'constants/validationMessages';

import './index.css';

export default class Form extends Component {
    onSubmit = event => {
        event.preventDefault();
        const isFormValid = this.validateForm();

        if (!isFormValid) {
            this.setState({
                activeTabIndex: 0,
            });

            return;
        }

        const { onSubmit } = this.props;
        const { form } = this.state;
        let formValues = Object.keys(form).reduce((values, key) => {
            values[key] = form[key].value;

            return values;
        }, {});

        if (form.randomPassword.value) {
            const {
                password,
                repeatPassword,
                ...formData
            } = formValues;

            formValues = { ...formData };
        }

        onSubmit(formValues)
    }

    onChange = (name, value) => {
        const state = {...this.state};
        state.form[name].value = value;

        this.setState(state);
    }

    onBeforeUpload = ({ xhr, files }) => {
        const { form } = this.state;

        form.image.value = files[0];
        this.setState({
            profileSrc: files[0].objectURL,
            form,
        });
    }

    onRolesChange = ({
        checked,
        originalEvent: {
            target: { name }
        }
    }, id) => {
        const { form } = this.state;

        if (checked) {
            form.roles.value.push(id);
        } else {
            form.roles.value.splice(form.roles.value.indexOf(id), 1);
        }

        this.setState({ form });
    }

    onTextChange = ({ target: { name, value } }) => {
        this.onChange(name, value);
    }

    onMaskChange = ({
        originalEvent: {
            target: { name, value }
        }
    }) => {
        this.onChange(name, value);
    }

    onTabChange = ({ index }) => {
        this.setState({
            activeTabIndex: index,
        });
    }

    state = {
        activeTabIndex: 0,
        profileSrc: avatarSrc,
        form: {
            firstName: {
                value: this.props.user.firstName || '',
                validators: ['isFilled'],
                error: '',
            },
            lastName: {
                value: this.props.user.lastName || '',
                validators: ['isFilled'],
                error: '',
            },
            email: {
                value: this.props.user.email || '',
                validators: ['isEmail', 'isFilled'],
                error: '',
            },
            phone: {
                value: this.props.user.phone || '',
            },
            name: {
                value: this.props.user.name || '',
            },
            password: {
                value: '',
                validators: ['isFilled'],
                error: '',
            },
            repeatPassword: {
                value: '',
                validators: ['isFilled'],
                error: '',
            },
            image: {
                value: '',
            },
            randomPassword: {
                value: false,
            },
            changePasswordOnLogin: {
                value: false,
            },
            activationEmail: {
                value: false,
            },
            active: {
                value: false,
            },
            roles: {
                value: this.props.user.roles || [],
            }
        }
    };

    roles = [{
        id: 1,
        name: 'admin',
    }, {
        id: 2,
        name: 'moderator',
    }, {
        id: 3,
        name: 'user',
    }];

    validateForm() {
        const { form: defaultForm } = this.state;

        if (defaultForm.randomPassword.value) {
            defaultForm.password.validators = null;
            defaultForm.password.error = '';
            defaultForm.repeatPassword.validators = null;
            defaultForm.repeatPassword.error = '';
        } else {
            defaultForm.password.validators = ['isFilled'];
            defaultForm.repeatPassword.validators = ['isFilled'];
        }

        let { isFormValid, form } = validateForm(defaultForm);

        if (
            !defaultForm.randomPassword.value &&
            (!form.repeatPassword.error || form.repeatPassword.error === PASSWORDS_MATCH)
        ) {
            if (form.password.value !== form.repeatPassword.value) {
                form.password.error = '\r';
                form.repeatPassword.error = PASSWORDS_MATCH;
                isFormValid = false;
            } else {
                form.password.error = '';
                form.repeatPassword.error = '';
            }
        }

        this.setState({ form });

        return isFormValid;
    }

    renderPasswordsFields() {
        const { form } = this.state;

        return [
            <div key="password" className="edit-user-input-wrapper">
                <label
                    className="edit-user-label"
                    htmlFor="edit-user-password"
                >
                    Password
                </label>
                <Input
                    id="edit-user-password"
                    value={form.password.value}
                    error={form.password.error}
                    name="password"
                    onChange={this.onTextChange}
                    password
                />
            </div>,
            <div key="repeat-password" className="edit-user-input-wrapper">
                <label
                    className="edit-user-label"
                    htmlFor="edit-user-repeat-password"
                >
                    Repeat Password
                </label>
                <Input
                    id="edit-user-repeat-password"
                    value={form.repeatPassword.value}
                    error={form.repeatPassword.error}
                    name="repeatPassword"
                    type="password"
                    onChange={this.onTextChange}
                />
            </div>
        ];
    }

    renderUserForm() {
        const { form } = this.state;

        return (
            <form className="edit-user" onSubmit={this.onSubmit}>
                <div className="edit-user-upload">
                    <label>
                        <FileUpload
                            className="custom-file-upload"
                            auto
                            mode="basic"
                            onSelect={this.onBeforeUpload}
                        />
                        <img src={this.state.profileSrc} alt="User" />
                    </label>
                </div>
                <div className="edit-user-input-wrapper edit-user-input-wrapper-right">
                    <label
                        className="edit-user-label"
                        htmlFor="edit-user-first-name"
                    >
                        First Name <sup>*</sup>
                     </label>
                    <Input
                        id="edit-user-first-name"
                        value={form.firstName.value}
                        error={form.firstName.error}
                        name="firstName"
                        onChange={this.onTextChange}
                        autoFocus
                    />
                </div>
                <div className="edit-user-input-wrapper edit-user-input-wrapper-right">
                    <label
                        className="edit-user-label"
                        htmlFor="edit-user-last-name"
                    >
                        Last Name <sup>*</sup>
                    </label>
                    <Input
                        id="edit-user-last-name"
                        value={form.lastName.value}
                        error={form.lastName.error}
                        name="lastName"
                        onChange={this.onTextChange}
                    />
                </div>
                <div className="edit-user-input-wrapper edit-user-input-wrapper-clear">
                    <label
                        className="edit-user-label"
                        htmlFor="edit-user-email-address"
                    >
                        Email Address <sup>*</sup>
                    </label>
                    <Input
                        id="edit-user-email-address"
                        value={form.email.value}
                        error={form.email.error}
                        name="email"
                        onChange={this.onTextChange}
                    />
                </div>
                <div className="edit-user-input-wrapper">
                    <label
                        className="edit-user-label"
                        htmlFor="edit-user-phone-number"
                    >
                        Phone Number
                    </label>
                    <Input
                        id="edit-user-phone-number"
                        value={form.phone.value}
                        name="phone"
                        onChange={this.onMaskChange}
                        mask="(999) 999-9999"
                    />
                </div>
                <div className="edit-user-input-wrapper">
                    <label
                        className="edit-user-label"
                    >
                        User Name <sup>*</sup>
                    </label>
                    <Input
                        disabled
                        value={form.name.value}
                        readOnly
                    />
                    <div className="edit-user-tip">
                        Can not change username of admin
                    </div>
                </div>
                <div className="edit-user-input-wrapper">
                    <Checkbox
                        inputId="edit-user-set-random-password"
                        label="Set random password"
                        name="randomPassword"
                        checked={Boolean(form.randomPassword.value)}
                        value={form.randomPassword.value}
                        onChange={({ checked }) => this.onChange('randomPassword', checked)}
                    />
                </div>

                {!form.randomPassword.value && this.renderPasswordsFields()}

                <div className="edit-user-input-wrapper">
                    <Checkbox
                        inputId="edit-user-change-password-on-login"
                        label="Should change password on next login"
                        name="changePasswordOnLogin"
                        checked={Boolean(form.changePasswordOnLogin.value)}
                        value={form.changePasswordOnLogin.value}
                        onChange={({ checked }) => this.onChange('changePasswordOnLogin', checked)}
                    />
                </div>
                <div className="edit-user-input-wrapper">
                    <Checkbox
                        inputId="edit-user-activation-email"
                        label="Send activation email"
                        name="activationEmail"
                        checked={Boolean(form.activationEmail.value)}
                        value={form.activationEmail.value}
                        onChange={({ checked }) => this.onChange('activationEmail', checked)}
                    />
                </div>
                <div className="edit-user-input-wrapper">
                    <Checkbox
                        inputId="edit-user-active"
                        label="Active?"
                        name="active"
                        checked={Boolean(form.active.value)}
                        value={form.active.value}
                        onChange={({ checked }) => this.onChange('active', checked)}
                    />
                </div>
            </form>
        );
    }

    renderRolesForm() {
        const {
            form: {
                roles: {
                    value = []
                } = {}
            } = {}
        } = this.state;

        return (
            <form className="edit-user" onSubmit={this.onSubmit}>
                {this.roles.map(({ id, name }) => (
                    <div
                        className="edit-user-input-wrapper"
                        key={`${id}${name}`}
                    >
                        <Checkbox
                            inputId={`${id}${name}`}
                            label={name}
                            name={`${id}${name}`}
                            checked={Boolean(value.includes(id))}
                            value={value.includes(id)}
                            onChange={event => this.onRolesChange(event, id)}
                        />
                    </div>
                ))}
            </form>
        );
    }

    renderActions() {
        return (
            <div>
                <Button
                    secondary
                    onClick={this.props.onHide}
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

    renderRolesTabTitle() {
        const counter = 3;
        return (
            <span>
                Roles <span className="users-edit-form-counter">{counter}</span>
            </span>
        );
    }

    render() {
        return (
            <ModalPopup
                visible={this.props.visible}
                onHide={this.props.onHide}
                footer={this.renderActions()}
                className="Modal-Popup"
                inPortal={false}
                modal
            >
                <TabView
                    activeIndex={this.state.activeTabIndex}
                    onTabChange={this.onTabChange}
                    className="edit-user-tabview"
                >
                    <TabPanel
                        header="User information"
                        headerClassName="users-edit-form-tab-header"
                    >
                        {this.renderUserForm()}
                    </TabPanel>
                    <TabPanel
                        header={this.renderRolesTabTitle()}
                        headerClassName="users-edit-form-tab-header users-edit-form-tab-header-counter"
                    >
                        {this.renderRolesForm()}
                    </TabPanel>
                </TabView>
            </ModalPopup>
        );
    }
}
