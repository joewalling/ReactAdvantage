import React, { Component } from 'react';

import Table, { Column } from 'components/Table';
import ButtonMenu from 'components/ButtonMenu';
import Dropdown from 'components/Dropdown';
import Button from 'components/Button';
import SearchQuery from 'components/SearchQuery';
import PageHeader from 'components/PageHeader';
import ConfirmTag from 'components/ConfirmTag';
import BoxShadowContainer from 'components/BoxShadowContainer';
import Form from './components/Form';
import { ApolloClient } from 'apollo-client';
import gql from 'graphql-tag';
import './index.css';
import { HttpLink } from 'apollo-link-http';
import { InMemoryCache } from 'apollo-cache-inmemory';
import { AuthService } from '../../services/AuthService';

import {
    makeExecutableSchema,
    addMockFunctionsToSchema
} from 'graphql-tools';
import { mockNetworkInterfaceWithSchema } from 'apollo-test-utils';
import { typeDefs } from './schema';
import {
    graphql,
    ApolloProvider,
} from 'react-apollo';
const schema = makeExecutableSchema({ typeDefs });
addMockFunctionsToSchema({ schema });
const mockNetworkInterface = mockNetworkInterfaceWithSchema({ schema });

export default class UsersList extends Component {
    authService;
    constructor(props) {
        super(props);
        this.authService = new AuthService();
        this.columns = [{
            header: 'First Name',
            field: 'firstName',
            sortable: true,
            body: this.renderCellTemplate,
        }, {
            header: 'Name',
            field: 'userName',
            sortable: true,
            body: this.renderCellTemplate,
        }, {
            header: 'Last Name',
            field: 'lastName',
            sortable: true,
            body: this.renderCellTemplate,
        }, 
        //{
        //     header: 'Roles',
        //     field: 'roles',
        //     body: this.renderCellTemplate,
        // }, 
        {
            header: 'Email',
            field: 'email',
            sortable: true,
            body: this.renderCellTemplate,
        }, 
        //{
        //     header: 'Email Confirm',
        //     field: 'emailConfirm',
        //     sortable: true,
        //     body: this.renderTag,
        //     className: 'users-tag-cell'
        // }, 
        {
            header: 'Active',
            field: 'active',
            sortable: true,
            body: this.renderTag,
            className: 'users-tag-cell'
        }, {
            header: '',
            field: 'actions',
            body: this.renderActionsTemplate,
            className: 'users-actions-cell',
        }];
    }
    componentWillMount() {
        this.authService.ensureAuthorized().then(authUser => {
            this.reloadUsersList(authUser);
        });
    }

    reloadUsersList(authUser) {
        const link = new HttpLink({
            uri: `${process.env.REACT_APP_API_URI}/graphql`,
            headers:
                {
                    'content-type': 'application/json',
                    'Authorization': `Bearer ${authUser.access_token}`
                }
        });
        const client = new ApolloClient({
            link: link,
            networkInterface: mockNetworkInterface,
            cache: new InMemoryCache(),
        });
        console.log(client);
        // Requesting data

        //client.query({ query: gql`query Query {hero {name  firstName}}`, headers: { 'content-type': 'application/json' } }).then(console.log);
        client.query({
            query: gql`
                  query FeedQuery {
                    users {
                        userName  firstName lastName  email isActive
                    }
                  }
                `
        }).then(response => {
            console.log(response.data.users)
            var userlist=response.data.users;

            this.setState({UsersList: userlist,entries:response.data.users.length})
        })
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
            selectedUser: this.state.UsersList.find(({ id }) => id === selectedId),
        });
    }

    onCreateUser = () => {
        this.onEdit(null);
    }

    onEditSubmit = data => {
        console.log('Success! Form data bellow:');
        console.log(data);
        this.setState({
            popupVisible: false,
            selectedUser: null,
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
        selectedUserId: null,
        UsersList: [],
    };

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
        name: 'firstName',
        operators: 'all',
        label: 'First Name',
        input: {
            type: 'text'
        }
    }, {
        name: 'name',
        operators: 'all',
        label: 'Name',
        input: {
            type: 'text'
        }
    }, {
        name: 'lastName',
        operators: 'all',
        label: 'Last Name',
        input: {
            type: 'text'
        }
     }, {
        name: 'role',
        operators: ['='],
        label: 'Roles',
        input: {
            type: 'text'
        },
    }, {
        name: 'email',
        operators: 'all',
        label: 'Email',
        input: {
            type: 'text'
        },
    }, {
        name: 'date',
        operators: 'all',
        label: 'Date',
        input: {
            type: 'date'
        },
    }];

    normalizeUsers() {
        const normalizedUsers = this.state.UsersList.map(user => ({
            ...user,
            active: user.active ? 'Yes' : 'No',
            emailConfirm: user.emailConfirm ? 'Yes' : 'No',
            actions: this.renderButtonMenu(user.id),
        }));

        return normalizedUsers;
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
            label: 'Unlock',
            icon: '',
            command: e => console.log(`Unlock has been clicked, id is: ${id}`),
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
                <div
                    title={typeof rowData[field] === 'string' ? rowData[field] : '' }
                    className="users-list-cell-value"
                >
                    <span>{rowData[field]}</span>
                </div>
            );

        return content;
    }

    renderActionsTemplate = (rowData, field) => {
        return (
            <div className="actions-cell">
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
    },
        index
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
                key="create-user"
                onClick={this.onCreateUser}
            >
                Create new user
            </Button>
        ];
    }

    renderEditForm() {
        return (
            <Form
                onHide={this.onFormHide}
                onSubmit={this.onEditSubmit}
                visible={this.state.popupVisible}
                user={this.state.selectedUser || {}}
            />
        );
    }

    renderHeader() {
        return (
            <PageHeader
                title="Users"
                subtitle="Manage users and permission."
                actions={this.renderHeaderActions()}
            />
        );
    }

    renderTable(value, columns) {
        return (
            <Table
                value={value}
                rows={this.state.entries}
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
        const tableValue = this.normalizeUsers();
        const columns = this.columns.map(this.renderColumn);

        return (
            <BoxShadowContainer>
                {this.renderHeader()}
                <div className="user-table">
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
