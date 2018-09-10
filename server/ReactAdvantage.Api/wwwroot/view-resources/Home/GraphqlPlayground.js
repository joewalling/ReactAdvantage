window.addEventListener('load', function (event) {
    var mgr = getOidcUserManager();

    mgr.getUser().then(function (user) {
        if (user) {
            //log("User logged in", user.profile);
            initGraphqlPlayground(user);
        }
        else {
            //log("User not logged in");
            mgr.signinRedirect();
        }
    });

    
});

function getOidcUserManager() {
    var config = {
        authority: "https://localhost:44338",
        client_id: "graphqlPlaygroundJs",
        redirect_uri: "https://localhost:44398/Home/GraphqlPlaygroundAuthCallback",
        response_type: "id_token token",
        scope: "openid profile ReactAdvantageApi",
        post_logout_redirect_uri: "https://localhost:44398/Home/GraphqlPlayground"
    };

    var mgr = new Oidc.UserManager(config);

    return mgr;
}

function initGraphqlPlayground(user) {
    GraphQLPlayground.init(document.getElementById('root'), {
        // options as 'endpoint' belong here
        endpoint: 'https://localhost:44398/graphql',
        env: 'dev',
        config: {
            "extensions": {
                "endpoints": {
                    "dev": {
                        "url": "https://localhost:44398/graphql",
                        "headers": {
                            "Authorization": "Bearer " + user.access_token
                        }
                    }
                }
            }
        }
    });
}

function logout() {
    getOidcUserManager().signoutRedirect();
}