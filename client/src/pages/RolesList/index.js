import React, { Component } from 'react';
import moment from 'moment';

import Table, { Column } from 'components/Table';
import ButtonMenu from 'components/ButtonMenu';
import Dropdown from 'components/Dropdown';
import Button from 'components/Button';
import SearchQuery from 'components/SearchQuery';
import PageHeader from 'components/PageHeader';
import ConfirmTag from 'components/ConfirmTag';

import Form from './components/Form';
import './index.css';
import roles from './roles';

export default class RolesList extends Component {
    constructor(props) {
        super(props);

        this.columns = [{
            header: 'Role Name',
            field: 'roleName',
            sortable: true,
            body: this.renderCellTemplate,
        }, {
            header: 'Creation time',
            field: 'creationTime',
            sortable: true,
            body: this.renderCellTemplate,
        }, {
            header: '',
            field: 'actions',
            body: this.renderActionsTemplate,
            className: 'roles-actions-cell',
        }];
    }

    onDropdownChange = ({ value }) => {
        this.setState({
            entries: value,
        });
    }

    onFilterChange = data => {
        this.setState({
            query: data.query,
        });
    }

    onSearch = () => {
        console.log(`Search, query is: ${this.state.query}`);
    }

    onFormHide = () => {
        this.setState({ popupVisible: false });
    }

    onEdit = selectedRoleId => {
        this.setState({
            popupVisible: true,
            selectedRole: roles.find(({ id }) => id === selectedRoleId),
        });
    }

    onCreateRole = () => {
        this.onEdit(null);
    }

    onEditSubmit = data => {
        console.log('Success! Form data bellow:');
        console.log(data);
        this.setState({
            popupVisible: false,
            selectedRole: null,
        });
    }

    setTableRef = ref => {
        this.tableRef = ref && ref.tableRef;
    }

    state = {
        entries: 10,
        filter: [],
        query: '',
        popupVisible: false,
        selectedRole: null,
    }

    entries = [{
        label: 'Show 10 entries',
        value: 10,
    }, {
        label: 'Show 25 entries',
        value: 25,
    }, {
        label: 'Show 50 entries',
        value: 50,
    }, {
        label: 'Show 100 entries',
        value: 100,
    }];

    fields = [{
        name: 'roleName',
        operators: 'all',
        label: 'Role Name',
        input: {
            type: 'text'
        }
    }];

    normalizeRoles(roles) {
        const normalizedRoles = roles.map(role => ({
            ...role,
            creationTime: moment.unix(role.creationTime).format('MM/DD/YYYY'),
            actions: this.renderButtonMenu(role.id),
        }));

        return normalizedRoles;
    }

    normalizeHiddenTableValue(value) {
        return value.map(({ actions, ...restValue }) => ({
            ...restValue
        }));
    }

    renderButtonMenu(id) {
        const actionItems = [{
            label: 'Permissions',
            icon: '',
            command: e => console.log(`Permissions has been clicked, id is: ${id}`),
        }, {
            label: 'Delete',
            icon: '',
            command: e => console.log(`Delete has been clicked, id is: ${id}`),
        }];

        return (
            <ButtonMenu
                label="Edit"
                items={actionItems}
                onClick={() => this.onEdit(id)}
            />
        );
    }

    renderCellTemplate = (rowData, field) => {
        const content = field === 'actions'
            ? rowData[field]
            : (
                <div className="roles-list-cell-value">
                    <span>{rowData[field]}</span>
                </div>
            );

        return content;
    }

    renderActionsTemplate = (rowData, field) => {
        return (
            <div className="roles-actions-cell">
                {rowData[field]}
            </div>
        );
    }

    renderTag = (rowData, field) => {
        return (
            <ConfirmTag active={rowData[field] === 'Yes'}>
                {rowData[field]}
            </ConfirmTag>
        );
    }

    renderDropdown() {
        return (
            <div className="roles-table-select">
                <Dropdown
                    options={this.entries}
                    value={this.state.entries}
                    onChange={this.onDropdownChange}
                />
            </div>
        );
    }

    renderColumn = ({
        header,
        field,
        sortable,
        body,
        className,
    }, index
    ) => {
        return (
            <Column
                key={index}
                sortable={sortable}
                header={header}
                field={field}
                body={rowData => body(rowData, field)}
                className={className}
            />
        );
    }

    // renderTableHeader() {
    //     return (
    //         <SearchQuery
    //             fields={this.fields}
    //             onChange={this.onFilterChange}
    //             onSearch={this.onSearch}
    //         />
    //     );
    // }

    renderHeaderActions() {
        return [
            <Button
                key="export"
                onClick={() => this.tableRef.exportCSV()}
            >
                Export
            </Button>,
            <Button
                key="create-role"
                onClick={this.onCreateRole}
            >
                Create new role
            </Button>
        ];
    }

    renderHeader() {
        return (
            <PageHeader
                title="Roles"
                subtitle="Use roles to group permissions"
                actions={this.renderHeaderActions()}
            />
        );
    }

    renderTable(value, columns) {
        return (
            <Table
                value={value}
                rows={this.state.entries}
                ref={this.setTableRef}
                responsive
                paginator
            >
                {columns}
            </Table>
        );
    }

    renderHiddenTable(value, columns) {
        /**
         * Since there is no way to hide "actions" cell
         * in exported csv we have to render hidden table
         * without this cell and use "export" method on it.
         * see docs https://www.primefaces.org/primereact/#/datatable/export
         */
        return (
            <Table
                value={this.normalizeHiddenTableValue(value)}
                rows={this.state.entries}
                ref={this.setTableRef}
                responsive
                paginator
                className="hidden"
            >
                {columns.slice(0, -1)}
            </Table>
        );
    }

    renderEditForm() {
        return (
            <Form
                onHide={this.onFormHide}
                onSubmit={this.onEditSubmit}
                visible={this.state.popupVisible}
                role={this.state.selectedRole || {}}
            />
        );
    }

    render() {
        const tableValue = this.normalizeRoles(roles);
        const columns = this.columns.map(this.renderColumn);

        return (
            <section className="roles-list">
                {this.renderHeader()}
                <div className="roles-table">
                    <div className="roles-table-header">
                        {/* {this.renderTableHeader()} */}
                    </div>
                    {this.renderDropdown()}
                    {this.renderTable(tableValue, columns)}
                    {this.renderHiddenTable(tableValue, columns)}
                </div>
                {this.state.popupVisible && this.renderEditForm()}
            </section>
        );
    }
}