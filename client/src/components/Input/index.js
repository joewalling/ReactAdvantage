import React, { Component } from 'react';
import { InputText } from 'primereact/components/inputtext/InputText';
import { InputMask } from 'primereact/components/inputmask/InputMask';
import { Password } from 'primereact/components/password/Password';
import './index.css';

export default class Input extends Component {
    constructor(props) {
        super(props);

        let Input = InputText;

        if (this.props.mask) {
            Input = InputMask;
        } else if (this.props.password) {
            Input = Password;
        }

        this.InputType = Input;
    }

    render() {
        const {
            className = '',
            error,
            password,
            ...restProps
        } = this.props;

        const { InputType } = this;

        const classNames = ['input'].concat(className.split(' '));

        error && classNames.push('ui-state-error');

        return (
            <div className="input-wrapper">
                <InputType
                    className={classNames}
                    {...restProps}
                />
                {error && <div className="input-error">{error}</div>}
            </div>
        );
    }
}
