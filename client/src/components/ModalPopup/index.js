import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import { Dialog } from 'primereact/components/dialog/Dialog';

import './index.css';

const modalRoot = document.getElementById('modal-root');

export default class ModalPopup extends Component {
    constructor(props) {
        super(props);
        this.el = document.createElement('div');
    }

    componentDidMount() {
        modalRoot && modalRoot.appendChild(this.el);
    }

    componentWillUnmount() {
        modalRoot && modalRoot.removeChild(this.el);
    }

    render() {
        const { inPortal, ...props } = this.props;

        if (!inPortal) {
            return (
                <div className="dialog-wrapper">
                    <Dialog {...props}>
                        {this.props.children}
                    </Dialog>
                </div>
            );
        }

        return (
            ReactDOM.createPortal(
                <div className="dialog-wrapper">
                    <Dialog {...props}>
                        {this.props.children}
                    </Dialog>
                </div>,
                this.el
            )
        );
    }
}
