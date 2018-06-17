import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import { Dialog } from 'primereact/components/dialog/Dialog';

import './index.css';

const modalRoot = document.getElementById('modal-root');

export default class ModalPopup extends Component {
    componentDidMount() {
        modalRoot.appendChild(this.el);
    }

    componentWillUnmount() {
        modalRoot.removeChild(this.el);
    }

    el = document.createElement('div');

    render() {
        return (
            ReactDOM.createPortal(
                <div className="dialog-wrapper">
                    <Dialog {...this.props}>
                        {this.props.children}
                    </Dialog>
                </div>,
                this.el
            )
        );
    }
}
