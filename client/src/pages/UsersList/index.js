import React, { useState, useEffect, createRef } from 'react';

import ButtonMenu from 'components/ButtonMenu';
import Table, {Column} from 'components/Table'
import Button from 'components/Button';
import ConfirmTag from 'components/ConfirmTag';
import Form from './components/Form';
import ListView from 'components/ListView';
import { UsersService } from './userService';
import './index.css';

const usersService = new UsersService();

const renderCellTemplate = (rowData, field) => {
  return field === 'actions' ? (
    rowData[field]
  ) : (
    <div className="customers-list-cell-value">
      <span>{rowData[field]}</span>
    </div>
  );
};
const renderActionsTemplate = (rowData, field) => {
  return <div className="customers-actions-cell">{rowData[field]}</div>;
};

const renderTag = (rowData, field) => {
  return (
    <ConfirmTag active={rowData[field] === 'Yes'}>{rowData[field]}</ConfirmTag>
  );
};

const columns = [
  {
    header: 'Name',
    field: 'userName',
    sortable: true,
    body: renderCellTemplate,
  },
  {
    header: 'First Name',
    field: 'firstName',
    sortable: true,
    body: renderCellTemplate,
  },
  {
    header: 'Last Name',
    field: 'lastName',
    sortable: true,
    body: renderCellTemplate,
  },
  {
    header: 'Email',
    field: 'email',
    sortable: true,
    body: renderCellTemplate,
  },
  {
    header: 'Email Confirm',
    field: 'emailConfirm',
    sortable: true,
    body: renderTag,
    className: 'users-tag-cell',
  },
  {
    header: 'Active',
    field: 'active',
    sortable: true,
    body: renderTag,
    className: 'users-tag-cell',
  },
  {
    header: '',
    field: 'actions',
    body: renderActionsTemplate,
    className: 'users-actions-cell',
  },
];

const filterFields = [
  {
    name: 'firstName',
    operators: 'all',
    label: 'First Name',
    input: {
      type: 'text',
    },
  },
  {
    name: 'name',
    operators: 'all',
    label: 'Name',
    input: {
      type: 'text',
    },
  },
  {
    name: 'lastName',
    operators: 'all',
    label: 'Last Name',
    input: {
      type: 'text',
    },
  },
  {
    name: 'role',
    operators: ['='],
    label: 'Roles',
    input: {
      type: 'text',
    },
  },
  {
    name: 'email',
    operators: 'all',
    label: 'Email',
    input: {
      type: 'text',
    },
  },
  {
    name: 'date',
    operators: 'all',
    label: 'Date',
    input: {
      type: 'date',
    },
  },
];

const UsersList = () => {
  const [listItems, setListItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [first, setFirst] = useState(0);
  const [total, setTotal] = useState(0);
  const [entriesValue, setEntriesValue] = useState(10);
  const [showModal, setShowModal] = useState(false);
  const [selectedUser, setSelectedUser] = useState(null);

  let tableRef = createRef();

  useEffect(() => {
    getUsers();
  }, []);

  const getUsers = async () => {
    const res = await usersService.getUsers();
    setTotal(res.length);
    setListItems(res.slice(0, entriesValue));
    setLoading(false);
  };

  const loadData = async (first, count) => {
    setLoading(true);

    let res = await usersService.getUsers();
    setFirst(first);
    setListItems(res.slice(first, first + count));
    setLoading(false);
  };

  const onPage = event => {
    loadData(event.first, event.rows);
  };

  const normalizeRecords = records => {
    return records.map(item => ({
      ...item,
      firstName: item.firstName ? item.firstName : '',
      lastName: item.lastName ? item.lastName : '',
      active: item.active ? 'Yes' : 'No',
      emailConfirm: item.emailConfirm ? 'Yes' : 'No',
      actions: renderButtonMenu(item.id),
    }));
  };

  const renderButtonMenu = id => {
    const actionItems = [
      {
        label: 'View/Edit',
        icon: '',
        command: () => onEdit(id),
      },
      {
        label: 'Permissions',
        icon: '',
        command: () =>
          console.log(`Permissions has been clicked, id is: ${id}`),
      },
      {
        label: 'Unlock',
        icon: '',
        command: () => console.log(`Unlock has been clicked, id is: ${id}`),
      },
      {
        label: 'Delete',
        icon: '',
        command: () => console.log(`Delete has been clicked, id is: ${id}`),
      },
    ];
    return <ButtonMenu items={actionItems} />;
  };

  const onEdit = async selectedId => {
    let user = listItems.find(d => d.id === selectedId);
    setSelectedUser(user);
    setShowModal(true);
  };

  const onEditSubmit = data => {
    console.log('Success! Form data bellow:');
    console.log(data);
    setShowModal(false);
    setSelectedUser(null);
  };

  const onEntriesValueChange = ({ value }) => {
    setEntriesValue(value);
    loadData(first, value);
  };

  const onCreateNew = () => {
    setSelectedUser(null);
    setShowModal(true);
  };

  const form = showModal ? (
    <Form
      onHide={() => setShowModal(false)}
      onSubmit={onEditSubmit}
      visible={showModal}
      user={selectedUser || {}}
    />
  ) : null;

  const pageActions = [
    <Button
      label="Export"
      key="export"
      onClick={() => tableRef.current.exportCSV()}
    />,
    <Button
      label="Add"
      icon="fa fa-plus"
      key="create-user"
      onClick={onCreateNew}
    />,
  ];
  
  const normalizeHiddenTableValue = (value) => {
    return value.map(({actions, ...restValue}) => ({
      ...restValue
    }))
  }
  
  const renderColumn = ({
    header,
    field,
    sortable,
    className,
  }, index) => {
    return (
      <Column
        key={index}
        sortable={sortable}
        header={header}
        field={field}
        className={className}
      />
    );
  }
  
  const renderHiddenTable = (value, columns) => {
    return (
      <Table
        value={normalizeHiddenTableValue(value)}
        rows={entriesValue}
        tableRef={tableRef}
        responsive
        paginator
        className="hidden"
      >
        {columns.slice(0,-1)}
      </Table>
    )
  }

  const normalizedRecords = normalizeRecords(listItems);
  const cols = columns.map(renderColumn);

  return (
    <>
      <ListView
        pageHeader={{
          title: 'Users',
          subtitle: 'Mange users and permission',
          actions: pageActions,
        }}
        totalRecords={total}
        value={normalizedRecords}
        responsive={true}
        lazy={true}
        first={first}
        loading={loading}
        onPage={onPage}
        filterFields={filterFields}
        onFilterChange={() => console.log('onFilterChange')}
        entriesValue={entriesValue}
        onSearch={() => console.log('onSearch')}
        onEntriesValueChange={onEntriesValueChange}
        columns={columns}
        tableRef={tableRef}
      />
      {renderHiddenTable(normalizedRecords, cols)}
      {form}
    </>
  );
};

export default UsersList;
