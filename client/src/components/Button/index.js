import React, { Component } from 'react';
import { Button as PrimeButton } from 'primereact/components/button/Button';

export default class Button extends Component {
    render() {
        const { secondary, danger, children, ...restProps } = this.props;
        const classNames = [];
        secondary && classNames.push('p-button-secondary');
        danger && classNames.push('p-button-danger')

        return (
            <PrimeButton
                className={classNames.join(' ')}
                {...restProps}
            />
        );
    }
}
