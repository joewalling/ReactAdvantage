import React from 'react';
import { CSSTransition } from 'react-transition-group';
import './index.css';

const FadeInContainer = props => (
    <CSSTransition
        classNames="fade"
        {...props}
        timeout={{ enter: 200, exit: 200 }}
    />
);

export default FadeInContainer;
