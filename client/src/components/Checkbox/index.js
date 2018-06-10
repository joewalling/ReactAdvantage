import React, { Component } from 'react';
import { Checkbox as PrimeCheckbox } from 'primereact/components/checkbox/Checkbox';

import './index.css';

export default class Checkbox extends Component {
    renderLabel() {
        const { label, inputId } = this.props;

        return (
            <label htmlFor={inputId} className="checkbox-label">
                {label}
            </label>
        );
    }

    render() {
        const { label, ...restProps } = this.props;

        return (
            <div className="checkbox-wrapper">
                <PrimeCheckbox {...restProps} />
                {label && this.renderLabel()}
            </div>
        );
    }
}
