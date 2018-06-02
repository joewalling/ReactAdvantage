import React, { Component } from 'react';

export default class HideOnClickOutsideContainer extends Component {
    componentDidMount() {
        document.addEventListener('mousedown', this.onClickOutside);
    }

    componentWillUnmount() {
        document.removeEventListener('mousedown', this.onClickOutside);
    }

    setWrapperRef = ref => {
        this.wrapperRef = ref;
    }

    onClickOutside = ({ target }) => {
        this.wrapperRef
            && !this.wrapperRef.contains(target)
            && this.props.onHide();
    }

    render() {
        return (
            <div ref={this.setWrapperRef}>
                {this.props.children}
            </div>
        );
    }
}
