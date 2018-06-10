import React, { Component } from 'react';
import './index.css';

export default class GridItem extends Component {
    render() {
        const {
            gridClassNames,
            title,
            ...restProps
        } = this.props;

        let classNames = ['grid-item'];

        if (gridClassNames) {
            classNames = classNames.concat(gridClassNames.split(' '));
        } else {
            classNames = classNames.concat('ui-g-12 ui-md-4 ui-lg-4'.split(' '));
        }

        return (
            <div
                className={classNames.join(' ')}
                {...restProps}
            >
                {title && <h3 className="grid-item-title">{title}</h3>}
                {this.props.children}
            </div>
        );
    }
}