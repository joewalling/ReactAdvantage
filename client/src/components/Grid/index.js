import React, { Component } from 'react';
import GridItem from "./components/GridItem";

import './index.css';

class GridWrapper extends Component {
    render() {
        return (
          <div className="ui-g grid-row">
            {this.props.children}
          </div>
        );
    }
}

export { GridWrapper, GridItem };
