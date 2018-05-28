import React, { Component } from 'react';
import { Sidebar as PrimeSidebar } from 'primereact/components/sidebar/Sidebar';
import FontAwesomeIcon from '@fortawesome/react-fontawesome';
import faTimes from '@fortawesome/fontawesome-free-solid/faTimes';
import './index.css';

export default class Sidebar extends Component {
    render() {
        return (
            <PrimeSidebar {...this.props}>
                <button
                    className="sidebar-close-button"
                    onClick={this.props.onHide}
                >
                    <FontAwesomeIcon className="far" icon={faTimes} />
                </button>
                {this.props.children}
            </PrimeSidebar>
        );
    }
}
