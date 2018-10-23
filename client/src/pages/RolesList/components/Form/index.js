import React, { Component } from 'react';

import TabView, { TabPanel } from 'components/TabView';
import ModalPopup from 'components/ModalPopup';
import Input from 'components/Input';
import Button from 'components/Button';
import Checkbox from 'components/Checkbox';
import Tree, { TreeNode } from 'components/Tree';
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

        formValues.permissions = this.state.treeValue;

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

    onHide = () => {
        const { onHide } = this.props;
        onHide && onHide();
    }

    onCheck = data => {
        this.setState({
            treeValue: data,
        });
    }

    onTabChange = ({ index }) => {
        this.setState({
            activeTabIndex: index,
        });
    }

    getPopupTitle() {
        return this.props.role.roleName
            ? `Edit role: ${this.props.role.roleName}`
            : 'Create role';
    }

    defaultCheckedKeys = ['audit-logs'];

    state = {
        activeTabIndex: 0,
        treeValue: this.defaultCheckedKeys,
        form: {
            name: {
                value: this.props.role.roleName || '',
                validators: ['isFilled'],
                error: '',
            },
            default: {
                value: false,
            },
        },
    };

    treeData = [{
        key: 'Documents',
        title: 'Pages',
        children: [{
            key: 'Administration',
            title: 'Administration',
            children: [{
                key: 'audit-logs',
                title: 'Audit logs',
            }, {
                key: 'email-templates',
                title: 'Email Templates'
            }, {
                key: 'hangfire-dashboard',
                title: 'Hangfire Dashboard'
            }],
        }, {
            key: 'Resources',
            title: 'Resources',
            children: [{
                key: 'full-access',
                title: 'Full access',
            }]
        }],
    }, {
        key: 'Pictures',
        title: 'Editions',
        children: [{
            key: 'edit-roles',
            title: 'Edit Roles'
        }],
    }];

    validateForm() {
        const { form: defaultForm } = this.state;
        let { isFormValid, form } = validateForm(defaultForm);

        this.setState({ form });

        return isFormValid;
    }

    renderRolesForm() {
        const { form } = this.state;

        return (
            <form className="edit-role" onSubmit={this.onSubmit}>
                <div className="edit-role-input-wrapper">
                    <label
                        className="edit-role-label"
                        htmlFor="edit-role-name"
                    >
                        Role Name <sup>*</sup>
                     </label>
                    <Input
                        id="edit-role-name"
                        value={form.name.value}
                        error={form.name.error}
                        name="name"
                        onChange={this.onTextChange}
                        autoFocus
                    />
                </div>
                <div className="edit-role-input-wrapper edit-role-input-wrapper-checkbox">
                    <Checkbox
                        inputId="edit-role-default"
                        label="Default"
                        name="default"
                        checked={Boolean(form.default.value)}
                        value={form.default.value}
                        onChange={({ checked }) => this.onChange('default', checked)}
                    />
                    <div className="edit-role-form-hint">Assign to new users as default</div>
                </div>
            </form>
        );
    }

    renderTreeForm() {
        return (
            <form className="edit-role" onSubmit={this.onSubmit}>
                <Tree
                    checkable
                    defaultExpandAll
                    selectable={false}
                    defaultCheckedKeys={this.defaultCheckedKeys}
                    onCheck={this.onCheck}
                >
                    {this.renderTreeNodeContent(this.treeData)}
                </Tree>
            </form>
        );
    }

    renderActions() {
        return (
            <div>
                <Button
                    label="Cancel"
                    secondary
                    onClick={this.props.onHide}
                    className="edit-role-cancel-button"
                ></Button>
                <Button
                    label="Save"
                    onClick={this.onSubmit}
                    className="edit-role-save-button"
                ></Button>
            </div>
        );
    }

    renderTreeNodeContent(data) {
        return data.map(item => {
            if (item.children) {
                return (
                    <TreeNode
                        key={item.key}
                        title={item.title}
                    >
                        {this.renderTreeNodeContent(item.children)}
                    </TreeNode>
                );
            }

            return (
                <TreeNode key={item.key} title={item.title} />
            );
        });
    }

    render() {
        return (
            <ModalPopup
                visible={this.props.visible}
                onHide={this.onHide}
                footer={this.renderActions()}
                header={this.getPopupTitle()}
                inPortal={false}
                modal
            >
                <TabView
                    activeIndex={this.state.activeTabIndex}
                    onTabChange={this.onTabChange}
                >
                    <TabPanel header="Role Properties">
                        {this.renderRolesForm()}
                    </TabPanel>
                    <TabPanel header="Permissions">
                        {this.renderTreeForm()}
                    </TabPanel>
                </TabView>
            </ModalPopup>
        );
    }
}
