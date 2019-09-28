import React from 'react';
import Table, { Column } from 'components/Table';
import Dropdown from 'components/Dropdown';

const PagedTable = ({
  columns,
  onEntriesValueChange,
  entriesValue,
  tableRef,
  ...props
}) => {
  return (
    <React.Fragment>
      <Dropdown
        options={entries}
        value={entriesValue}
        onChange={onEntriesValueChange}
      />
      <Table {...props} tableRef={tableRef} paginator rows={entriesValue}>
        {columns.map(renderColumn)}
      </Table>
    </React.Fragment>
  );
};

export default PagedTable;

const renderColumn = (
  { header, field, sortable, body, className, width },
  index
) => {
  return (
    <Column
      key={index}
      sortable={sortable}
      header={header}
      field={field}
      body={rowData => (body && body(rowData, field)) || rowData[field]}
      className={className}
      style={{ width: width }}
    />
  );
};

const entries = [
  {
    label: 'Show 10 entries',
    value: 10,
  },
  {
    label: 'Show 25 entries',
    value: 25,
  },
  {
    label: 'Show 50 entries',
    value: 50,
  },
  {
    label: 'Show 100 entries',
    value: 100,
  },
];
