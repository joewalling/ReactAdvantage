import React, { Component } from 'react';
import GridItem from "./GridItem";

import './index.css';

export default class GridContent extends Component {
    renderItems() {
        return this.props.items
            .map((item, index) =>
                <GridItem
                    key={index}
                    title={item.title}
                    item={item.content}
                />
            );
    }

    render() {
        return (
          <div className="ui-g grid-row">{this.renderItems()}</div>
        );
    }
}

export { GridItem }
