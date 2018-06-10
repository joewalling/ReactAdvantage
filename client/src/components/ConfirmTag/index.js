import React from 'react';
import classnames from 'classnames';

import './index.css';

const ConfirmTag = props => {
    const { active, ...restProps } = props;
    const classNames = classnames({
        'confirm-tag': true,
        inactive: !active,
    });

    return (
        <div className={classNames} {...restProps}>
            {props.children}
        </div>
    );
}

export default ConfirmTag;