import React, { Component } from 'react';
import moment from 'moment';

import Table, { Column } from 'components/Table';
import ButtonMenu from 'components/ButtonMenu';
import Dropdown from 'components/Dropdown';
import Button from 'components/Button';
import SearchQuery from 'components/SearchQuery';
import PageHeader from 'components/PageHeader';
import ConfirmTag from 'components/ConfirmTag';
import BoxShadowContainer from 'components/BoxShadowContainer';

import Form from './components/Form';
import './index.css';
import tenants from './tenants';

export default class TenantsList extends Component{
    constructor(props){
        super(props);

        this.columns = [{
            header: 'Tenancy code Name',
            field: 'tenantName',
            sortable: true,
            body: this.renderCellTemplate,
        }, {
            header: 'Name',
            field: 'name',
            sortable: true,
            body: this.renderCellTemplate,
        }, {
            header: 'Edition',
            field: 'edition',
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
            className: 'tenants-actions-cell',
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

    onEdit = selectedId => {
        this.setState({
            popupVisible: true,
            selectedTenant: tenants.find(({ id }) => id === selectedId),
        });
    }

    onEditSubmit = data => {
        console.log('Success! Form data bellow:');
        console.log(data);
        this.setState({
            popupVisible: false,
            selectedUser: null,
        });
    }

    onCreateTenant = () => {
        this.onEdit(null);
    }

    setTableRef = ref => {
        this.tableRef = ref && ref.tableRef;
    }

    state = {
        entries: 10,
        filter: [],
        query: '',
        popupVisible: false,
        selectedTenantId: null,
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
        name: 'tenantName',
        operators: 'all',
        label: 'Tenant Name',
        input: {
            type: 'text'
        }
    }];

    normalizeTenants(tenants) {
        const normalizedTenants = tenants.map(tenant => ({
            ...tenant,
            creationTime: moment.unix(tenant.creationTime).format('MM/DD/YYYY'),
            actions: this.renderButtonOptions(tenant.id),
        }));

        return normalizedTenants;
    }

    normalizeHiddenTableValue(value) {
        return value.map(({ actions, ...restValue }) => ({
            ...restValue
        }));
    }

    //Needs Refactoring

    renderButtonOptions(id) {
        return (
            <div>
                <Button
                    label="Login"
                    onClick={() => console.log(`Login has been clicked, id is: ${id}`)}
                />
                <Button
                    label="Edit"
                    onClick={() => this.onEdit(id)}
                />
                <Button
                    label="Features"
                    onClick={() => console.log(`Edit has been clicked, id is: ${id}`)}
                />
                <Button
                    label="Delete"
                    onClick={() => console.log(`Edit has been clicked, id is: ${id}`)}
                />
            </div>
        );
    }

    renderCellTemplate = (rowData, field) => {
        const content = field === 'actions'
            ? rowData[field]
            : (
                <div className="tenants-list-cell-value">
                    <span>{rowData[field]}</span>
                </div>
            );

        return content;
    }

    renderActionsTemplate = (rowData, field) => {
        return (
            <div className="tenants-actions-cell">
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
            <Dropdown
                options={this.entries}
                value={this.state.entries}
                onChange={this.onDropdownChange}
            />
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

    renderTableHeader() {
        return (
            <SearchQuery
                fields={this.fields}
                onChange={this.onFilterChange}
                onSearch={this.onSearch}
            />
        );
    }

    renderHeaderActions() {
        return [
            <Button
                key="export"
                onClick={() => this.tableRef.exportCSV()}
            >
                Export
            </Button>,
            <Button
                key="create-tenant"
                onClick={this.onCreateTenant}
            >
                Create new tenant
            </Button>
        ];
    }

    renderEditForm() {
        return (
            <Form
                onHide={this.onFormHide}
                onSubmit={this.onEditSubmit}
                visible={this.state.popupVisible}
                tenant={this.state.selectedTenant || {}}
            />
        );
    }

    renderHeader() {
        return (
            <PageHeader
                title="Tenants"
                subtitle="Use Tenants to group permissions"
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

    render() {
        const tableValue = this.normalizeTenants(tenants);
        const columns = this.columns.map(this.renderColumn);

        return (
            <BoxShadowContainer className="tenant-list">
                {this.renderHeader()}
                <div className="tenant-table">
                    {this.renderTableHeader()}
                    {this.renderDropdown()}
                    {this.renderTable(tableValue, columns)}
                    {this.renderHiddenTable(tableValue, columns)}
                </div>
                {this.state.popupVisible && this.renderEditForm()}
            </BoxShadowContainer>
        );
    }
}