import React, { Component } from 'react';
import './index.css';

export default class GridItem extends Component {
    render(){
        return (
            <div className="ui-g-12 ui-md-4 ui-lg-4 grid-item">
                <h3 className="grid-item-title">{this.props.title}</h3>
                {this.props.item}
            </div>
        );
    }
}