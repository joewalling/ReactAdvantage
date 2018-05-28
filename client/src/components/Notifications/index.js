import React, { Component } from 'react';
import FontAwesomeIcon from '@fortawesome/react-fontawesome';
import faBell from '@fortawesome/fontawesome-free-solid/faBell';

import './index.css';

export default class Notifications extends Component {
    render() {
        return (
            <div>
                <button onClick={this.toggleSidebar} className="button">
                    <FontAwesomeIcon className="far" icon={faBell} />
                </button>
            </div>
        );
    }
}
