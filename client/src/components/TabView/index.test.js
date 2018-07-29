import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import TabView from './index';

it('renders correctly', () => {
    const component = shallow(<TabView />);

    expect(component).toMatchSnapshot();
});
