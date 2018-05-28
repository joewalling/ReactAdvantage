import React, { Component } from 'react';
import {MultiSelect as PrimeMultiSelect} from 'primereact/components/multiselect/MultiSelect';

export default class MultiSelect extends Component {
    render() {
        return (
            <PrimeMultiSelect {...this.props} />
        );
    }
}
