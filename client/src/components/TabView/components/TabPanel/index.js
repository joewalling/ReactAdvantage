import React, { Component } from 'react';
import { TabPanel as TabPrimePanel } from 'primereact/components/tabview/TabView';

export default class TabPanel extends Component {
    render() {
        return (
            <TabPrimePanel {...this.props}>
                {this.props.children}
            </TabPrimePanel>
        );
    }
}
