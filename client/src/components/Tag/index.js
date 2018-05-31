import React, { Component } from 'react';
import './index.css';

export default class Tag extends Component {
    render() {
        const { type, text } = this.props;
        const classNames = ['tag', type];

        const iconClassNames = ['fa'];

        switch (type) {
            case 'announcement':
                iconClassNames.push('fa-bolt');
                break;
            case 'enhancement':
                iconClassNames.push('fa-bullhorn');
                break;
            default:
                break;
        }

        return (
            <div className={classNames.join(' ')}>
                <div className="tag-icon">
                    <i className={iconClassNames.join(' ')}></i>
                </div>
                <div className="tag-text">
                    {text}
                </div>
            </div>
        );
    }
}
