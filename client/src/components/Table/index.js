import React, { Component } from 'react';
import { DataTable } from 'primereact/components/datatable/DataTable';

import Row from './components/Row';
import Column from './components/Column';

export default class Table extends Component {
    setRef = ref => {
        this.tableRef = ref;
    }

    render() {
        return (
            <DataTable {...this.props} ref={this.setRef}>
                {this.props.children}
            </DataTable>
        );
    }
}

export { Row, Column };