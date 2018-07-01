import React, { Component } from 'react';
import moment from 'moment';

import ModalPopup from 'components/ModalPopup';
import Input from 'components/Input';
import Checkbox from 'components/Checkbox';
import Button from 'components/Button';
import Dropdown from 'components/Dropdown';
import Calendar from 'components/Calendar';
import validateForm from 'utils/validateForm';

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

        formValues.subscription = moment(formValues.subscription).format('X');

        onSubmit(formValues)
    }

    onChange = (name, value) => {
        const state = {...this.state};
        state.form[name].value = value;

        this.setState(state);
    }

    onTextChange = ({ target: { name, value } }) => {
        this.onChange(name, value);
    }

    onDropdownChange = ({ value }) => {
        const state = {...this.state};
        state.form.editions.value = value;

        this.setState(state);
    }

    getPopupTitle() {
        return this.props.tenant.tenantName
            ? `Edit tenant: ${this.props.tenant.tenantName}`
            : 'Create tenant';
    }

    editions = [{
        label: 'Standard',
        value: 'Standard',
    }, {
        label: 'Pro',
        value: 'Pro',
    }]

    state = {
        form: {
            name: {
                value: this.props.tenant.tenantName || '',
                validators: ['isFilled'],
                error: '',
            },
            editions: {
                value: this.editions[0].value,
            },
            subscription: {
                value: new Date(),
                validators: ['isFilled'],
                error: '',
            },
            active: {
                value: false,
            },
        }
    };

    validateForm() {
        const { form: defaultForm } = this.state;
        let { isFormValid, form } = validateForm(defaultForm);

        this.setState({ form });

        return isFormValid;
    }

    renderForm() {
        const { form } = this.state;

        return (
            <form className="edit-tenant" onSubmit={this.onSubmit}>
                <div className="edit-tenant-input-wrapper">
                    <label
                        className="edit-tenant-label"
                        htmlFor="edit-tenant-name"
                    >
                        Name <sup>*</sup>
                    </label>
                    <Input
                        id="edit-tenant-name"
                        value={form.name.value}
                        error={form.name.error}
                        onChange={this.onTextChange}
                        name="name"
                        autoFocus
                    />
                </div>
                <div className="edit-tenant-input-wrapper">
                    <label className="edit-tenant-label">
                        Edition
                    </label>
                    <Dropdown
                        options={this.editions}
                        value={form.editions.value}
                        onChange={this.onDropdownChange}
                    />
                </div>
                <div className="edit-tenant-input-wrapper">
                    <label className="edit-tenant-label">
                        Subscription end date <sup>*</sup>
                    </label>
                    <Calendar
                        value={form.subscription.value}
                        minDate={new Date()}
                        onChange={({ value }) => this.onChange('subscription', value)}
                    />
                </div>
                <div className="edit-tenant-input-wrapper">
                    <Checkbox
                        inputId="edit-tenant-active"
                        label="Active"
                        name="active"
                        checked={Boolean(form.active.value)}
                        value={form.active.value}
                        onChange={({ checked }) => this.onChange('active', checked)}
                    />
                </div>
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

    render() {
        return (
            <ModalPopup
                visible={this.props.visible}
                onHide={this.props.onHide}
                header={this.getPopupTitle()}
                footer={this.renderActions()}
                inPortal={false}
                modal
            >
                {this.renderForm()}
            </ModalPopup>
        );
    }
}
