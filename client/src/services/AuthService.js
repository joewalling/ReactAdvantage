import { Log, User, UserManager } from 'oidc-client';

export class AuthService {
    userManager;

    constructor() {
        const settings = {
            authority: `${process.env.REACT_APP_IDENTITY_SERVER_URI}`,
            client_id: 'react',
            redirect_uri: `${process.env.REACT_APP_URI}/authentication/callback`,
            silent_redirect_uri: `${process.env.REACT_APP_URI}/authentication/silentCallback`,
            post_logout_redirect_uri: `${process.env.REACT_APP_URI}/`,
            response_type: 'id_token token',
            scope: 'openid profile ReactAdvantageApi'
        };
        this.userManager = new UserManager(settings);

        Log.logger = console;
        Log.level = Log.INFO;
    }

    getUser() {
        return this.userManager.getUser();
    }

    login() {
        return this.userManager.signinRedirect();
    }

    renewToken() {
        return this.userManager.signinSilent();
    }

    logout() {
        return this.userManager.signoutRedirect();
    }

    ensureAuthorized() {
        return this.getUser().then(user => {
            return new Promise((resolve, reject) => {
                if (user && user.access_token) {
                    resolve(user);
                    //todo after api call:
                    // .catch(error => {
                    //     if (error.response.status === 401) {
                    //         this.renewToken().then(renewedUser => {
                    //             //repeat api call with renewedUser
                    //         });
                    //     }
                    //     throw error;
                    // });
                } else if (user) {
                    this.renewToken().then(renewedUser => {
                        resolve(renewedUser);
                    });
                } else {
                    reject();
                    this.login();
                }
            });
        });
    }

    signinRedirectCallback() {
        return this.userManager.signinRedirectCallback();
    }

    signinSilentCallback() {
        return this.userManager.signinSilentCallback();
    }
}