import React from "react";
import { AuthService } from '../../services/AuthService';

export default class  AuthenticationSilentCallback extends React.Component {
    constructor(props) {
        super(props);
        this.authService = new AuthService();
    }

    componentWillMount() {
        this.authService.signinSilentCallback();
    }

    render() {
        return (
            <p></p>
        )
    }
}