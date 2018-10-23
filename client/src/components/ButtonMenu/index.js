import React, { Component } from 'react';
import { SplitButton } from 'primereact/components/splitbutton/SplitButton';

export default class ButtonMenu extends Component {
    render() {
        const {
            label,
            items,
            ...props
        } = this.props;
        return (
            <SplitButton
                model={this.props.items}
                label={this.props.label}
                {...props}
            />
        );
    }
}
