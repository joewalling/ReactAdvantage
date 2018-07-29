import React, { Component } from 'react';
import {Card} from 'primereact/components/card/Card';

import Button from "../Button";

import "./index.css"

export default class Post extends Component {
    renderReadMoreButton() {
        return (
            <Button
                className="ui-button-secondary"
                label="Read more"
            />
        );
    }

    render() {
        return (
            <div className="post">
                <Card className="content" footer={this.renderReadMoreButton()}>
                    {this.props.text}
                </Card>
            </div>
        );
    }
}
