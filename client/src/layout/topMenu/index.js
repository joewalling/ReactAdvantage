import React, { Component } from 'react';
import { Link } from "react-router-dom";
import FontAwesomeIcon from '@fortawesome/react-fontawesome'
import faCoffee from '@fortawesome/fontawesome-free-solid/faCoffee'

export default class TopMenu extends Component {
    render() {
        return (
            <div>
                <FontAwesomeIcon icon={faCoffee} />
                <Link to="/">Dashboard</Link>
                <Link to="/menuitem1">MenuItem1</Link>
            </div>
        );
    }
}
