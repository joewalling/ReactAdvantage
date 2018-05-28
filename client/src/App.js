import React, { Component } from 'react';
import {
  BrowserRouter as Router,
  Route,
  Switch
} from "react-router-dom";
import Container from 'layout/container';
import Header from 'layout/header';
import TopMenu from 'layout/topMenu';
import Dashboard from 'pages/Dashboard';
import MenuItem1 from 'pages/MenuItem1';
import './App.css';
import 'primereact/resources/primereact.min.css';
import 'primereact/resources/themes/omega/theme.css';
import 'font-awesome/css/font-awesome.css';

class App extends Component {
  render() {
    return (
      <div className="root-wrapper">
        <Router>
          <div>
            <Header />
            <Container>
              <Dashboard />
              {/*<Switch>
                <Route exact path="/" component={Dashboard} />
                <Route exact path="/menuitem1" component={MenuItem1} />
              </Switch>*/}
            </Container>
          </div>
        </Router>
      </div>
    );
  }
}

export default App;
