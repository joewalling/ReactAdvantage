import React, { Component } from 'react';
import {
  BrowserRouter as Router,
  Route,
  Switch
} from "react-router-dom";
import Container from 'layout/container';
import Header from 'layout/header';
import Dashboard from 'pages/Dashboard';
import UsersList from 'pages/UsersList';
import RolesList from 'pages/RolesList';
import TenantsList from 'pages/TenantsList';
import Login from 'pages/Login';
import AuthenticationCallback from 'pages/AuthenticationCallback';
import AuthenticationSilentCallback from "./pages/AuthenticationSilentCallback";
import Logout from "./pages/Logout";
import './App.css';
import 'primereact/resources/themes/nova-light/theme.css';
import 'primereact/resources/primereact.min.css';
import 'primeicons/primeicons.css';
import 'font-awesome/css/font-awesome.css';

class App extends Component {
  render() {
    return (
      <div className="root-wrapper">
        <Router>
          <div className="app">
            <Header />
            <Container>
              <Switch>
                <Route exact path="/" component={Dashboard} />
                <Route exact path="/login" component={Login} />
                <Route exact path="/users" component={UsersList} />
                <Route exact path="/roles" component={RolesList} />
                <Route exact path="/tenants" component={TenantsList} />
                <Route exact path="/authentication/callback" component={AuthenticationCallback}/>
                <Route exact path="/authentication/silentCallback" component={AuthenticationSilentCallback}/>
                <Route exact path="/logout" component={Logout}/>
              </Switch>
            </Container>
          </div>
        </Router>
      </div>
    );
  }
}

export default App;
