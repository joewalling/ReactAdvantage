import React, { Component } from 'react';
import { Row as PrimeRow } from 'primereact/components/row/Row';

export default class Row extends Component {
    render() {
        return (
            <PrimeRow {...this.props}>
                {this.props.children}
            </PrimeRow>
        );
    }
}
