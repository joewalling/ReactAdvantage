import React, { Component } from 'react';

import './index.css';

export default class PageHeader extends Component {
    render() {
        const {
            title,
            subtitle,
            actions,
            ...props
        } = this.props;

        return (
            <header className="page-header" {...props}>
                <h2>
                    <span>{`${title} `}</span>
                    {subtitle}
                </h2>
                <div className="users-list-header-actions">
                    {actions}
                </div>
            </header>
        );
    }
}
