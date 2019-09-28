import React, { Component } from 'react';
import { DataTable } from 'primereact/components/datatable/DataTable';

import Row from './components/Row';
import Column from './components/Column';

export default function Table({ tableRef, ...props }) {
  return (
    <DataTable {...props} ref={tableRef}>
      {props.children}
    </DataTable>
  );
}

export { Row, Column };
