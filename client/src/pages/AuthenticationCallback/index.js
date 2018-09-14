import React from "react";
import { AuthService } from '../../services/AuthService';

export default class  AuthenticationCallback extends React.Component {
    constructor(props) {
        super(props);
        this.authService = new AuthService();
    }

    componentWillMount() {
        this.authService.signinRedirectCallback().then(() => {
            window.location = '/';
        });
    }

    render() {
        return (
            <p>Redirecting...</p>
        )
    }
}