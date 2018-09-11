import React from 'react';
import classnames from 'classnames';

import './index.css';

const BoxShadowContainer = ({
    className,
    ...props,
}) => (
    <section
        className={`${className || ''} box-shadow-container`}
        {...props}
    >
        {props.children}
    </section>
);

export default BoxShadowContainer;
