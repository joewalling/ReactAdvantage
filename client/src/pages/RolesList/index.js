import React, { useState, useEffect, createRef } from 'react';
import moment from 'moment';

import ButtonMenu from 'components/ButtonMenu';
import Button from 'components/Button';
import ListView from 'components/ListView';
import Form from './components/Form';
import RolesService from './rolesService';
import './index.css';

const rolesService = new RolesService();

let columns = [
  {
    header: 'Role Name',
    field: 'roleName',
    sortable: true,
    body: renderCellTemplate,
  },
  {
    header: 'Creation time',
    field: 'creationTime',
    sortable: true,
    body: renderCellTemplate,
  },
  {
    header: '',
    field: 'actions',
    body: renderActionsTemplate,
    className: 'roles-actions-cell',
  },
];

let filterFields = [
  {
    name: 'roleName',
    operators: 'all',
    label: 'Role Name',
    input: {
      type: 'text',
    },
  },
];

let renderCellTemplate = (rowData, field) => {
  return field === 'actions' ? (
    rowData[field]
  ) : (
    <div className="roles-list-cell-value">
      <span>{rowData[field]}</span>
    </div>
  );
};

let renderActionsTemplate = (rowData, field) => {
  return <div className="roles-actions-cell">{rowData[field]}</div>;
};

const RolesList = () => {
  const [listItems, setListItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [first, setFirst] = useState(0);
  const [total, setTotal] = useState(0);
  const [entriesValue, setEntriesValue] = useState(10);
  const [showModal, setShowModal] = useState(false);
  const [selectedRole, setSelectedRole] = useState(null);

  let tableRef = createRef();

  useEffect(() => {
    getRoles();
  }, []);

  const getRoles = async () => {
    const res = await rolesService.getRoles();
    setTotal(res.length);
    setListItems(res.slice(0, entriesValue));
    setLoading(false);
  };

  const loadData = async (first, count) => {
    setLoading(true);

    let res = await rolesService.getRoles();

    setFirst(first);
    setListItems(res.slice(first, first + count));
    setLoading(false);
  };

  const onPage = event => {
    loadData(event.first, event.rows);
  };

  const onEdit = async selectedId => {
    let role = listItems.find(d => d.id === selectedId);
    setSelectedRole(role);
    setShowModal(true);
  };

  const onEditSubmit = data => {
    console.log('Success! Form data bellow:');
    console.log(data);
    setShowModal(false);
    setSelectedRole(null);
  };

  const onEntriesValueChange = ({ value }) => {
    setEntriesValue(value);
    loadData(first, value);
  };

  const onCreateNew = () => {
    setSelectedRole(null);
    setShowModal(true);
  };

  const normalizeRecords = records => {
    return records.map(item => ({
      ...item,
      creationTime: moment.unix(item.creationTime).format('MM/DD/YYYY'),
      actions: renderButtonMenu(item.id),
    }));
  };

  let renderButtonMenu = id => {
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
        label: 'Delete',
        icon: '',
        command: () => console.log(`Delete has been clicked, id is: ${id}`),
      },
    ];

    return <ButtonMenu items={actionItems} />;
  };

  const form = showModal ? (
    <Form
      onHide={() => setShowModal(false)}
      onSubmit={onEditSubmit}
      visible={showModal}
      role={selectedRole || {}}
    />
  ) : null;

  const pageActions = [
    <Button
      label="Export"
      key="export"
      onClick={() => tableRef.current.exportCSV()}
    />,
    <Button label="Create new role" key="create-role" onClick={onCreateNew} />,
  ];

  const normalizedRecords = normalizeRecords(listItems);

  return (
    <>
      <ListView
        pageHeader={{
          title: 'Roles',
          subtitle: 'Use roles to group permissions',
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
      {form}
    </>
  );
};

export default RolesList;
