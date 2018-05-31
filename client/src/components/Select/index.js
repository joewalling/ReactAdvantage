import React, { Component } from 'react';
import {MultiSelect as PrimeMultiSelect} from 'primereact/components/multiselect/MultiSelect';

import './index.css'

export default class MultiSelect extends Component {
    render() {
        return (
            <div >
                <PrimeMultiSelect className="select" {...this.props} />
            </div>
        );
    }
}
