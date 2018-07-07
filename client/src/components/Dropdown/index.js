import React, { Component } from 'react';
import { Dropdown as PrimeDropdown } from 'primereact/components/dropdown/Dropdown';

import './index.css';

export default class Dropdown extends Component {
    render() {
        return (
            <PrimeDropdown className="dropdown" {...this.props} />
        );
    }
}
