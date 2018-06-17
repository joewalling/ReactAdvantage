import React, { Component } from 'react';
import { FileUpload as PrimeFileUpload } from 'primereact/components/fileupload/FileUpload';

export default class FileUpload extends Component {
    render() {
        return (
            <PrimeFileUpload {...this.props} />
        );
    }
}
