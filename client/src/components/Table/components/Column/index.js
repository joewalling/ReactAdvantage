import React, { Component } from 'react';
import { Column as PrimeColumn } from 'primereact/components/column/Column';

export default class Column extends Component {
    render() {
        return (
            <PrimeColumn {...this.props}>
                {this.props.children}
            </PrimeColumn>
        );
    }
}
