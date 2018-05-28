import React, { Component } from 'react';
import GridItem from "./GridItem";

import './index.css';

export default class GridContent extends Component {
    render(){
        let gridItems = this.props.items
            .map((item, index) => (<GridItem key={index} title={item.title} item={item.content}/>));

        return(
          <div className="ui-g grid-row">{gridItems}</div>
        );
    }
}