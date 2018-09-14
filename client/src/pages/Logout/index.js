import React from "react";
import { AuthService } from '../../services/AuthService';

export default class Logout extends React.Component {
    constructor(props) {
        super(props);
        this.authService = new AuthService();
    }

    componentWillMount() {
        this.authService.logout().then(() => {
            window.location = '/';
        });
    }

    render() {
        return (
            <p>Redirecting...</p>
        )
    }
}