import React, { Component } from 'react';
import { Sidebar as PrimeSidebar } from 'primereact/components/sidebar/Sidebar';
import './index.css';

export default class Sidebar extends Component {
    render() {
        return (
            <PrimeSidebar className="right-sidebar-menu" {...this.props}>
                {this.props.children}
            </PrimeSidebar>
        );
    }
}
