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
import Login from 'pages/Login';
import './App.css';
import 'primereact/resources/primereact.min.css';
import 'primereact/resources/themes/omega/theme.css';
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
                <Route exact path="/users" component={UsersList} />
                <Route exact path="/login" component={Login} />
              </Switch>
            </Container>
          </div>
        </Router>
      </div>
    );
  }
}

export default App;
