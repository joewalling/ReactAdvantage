import React, { Component } from 'react';
import { Button as PrimeButton } from 'primereact/components/button/Button';

export default class Button extends Component {
    render() {
        const { secondary, ...restProps } = this.props;
        const classNames = [];
        secondary && classNames.push('ui-button-secondary');

        return (
            <PrimeButton
                className={classNames.join(' ')}
                {...restProps}
            />
        );
    }
}
