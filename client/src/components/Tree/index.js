import React, { Component } from 'react';
import RcTree, { TreeNode } from 'rc-tree';

import 'rc-tree/assets/index.less';
import './index.css';

export default class Tree extends Component {
    render() {
        return (
            <RcTree className="tree-component" {...this.props} />
        );
    }
}

export { TreeNode };