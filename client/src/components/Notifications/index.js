import React, { Component } from 'react';
import './index.css';

export default class Notifications extends Component {
    render() {
        return (
            <div>
                <button onClick={this.toggleSidebar} className="notification-button">
                    <i className="fa far fa-bell" />
                </button>
            </div>
        );
    }
}
