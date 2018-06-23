import React, { Component } from 'react';
import { TabView as PrimeTabView } from 'primereact/components/tabview/TabView';

import TabPanel from './components/TabPanel';

export default class TabView extends Component {
    render() {
        return (
            <PrimeTabView {...this.props}>
                {this.props.children}
            </PrimeTabView>
        );
    }
}

export { TabPanel };
