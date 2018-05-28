import React, { Component } from 'react';
import { Button as PrimeButton } from 'primereact/components/button/Button';

export default class Button extends Component {
    render() {
        return (
            <PrimeButton {...this.props} />
        );
    }
}
