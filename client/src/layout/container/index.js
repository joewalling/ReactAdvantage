import React, { Component } from 'react';
import './index.css';

export default class Container extends Component {
    render() {
        return (
            <section className="root-content-wrapper">
                <section>{this.props.children}</section>
            </section>
        );
    }
}
