import React, { Component } from 'react';
import {Card} from 'primereact/components/card/Card';

import Button from "../Button";

import "./index.css"

export default class Post extends Component {

    render() {
        let readMoreButton = <Button
            className="ui-button-secondary"
            label="Read more"
        />;

        return (
            <div className="post">
                <Card className="content" footer={readMoreButton}>
                    {this.props.text}
                </Card>

            </div>
        );
    }
}
