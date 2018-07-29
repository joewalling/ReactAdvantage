import React from 'react';
import Enzyme, { shallow, mount } from 'enzyme';

import Dashboard from './index';

it('renders correctly', () => {
    const component = shallow(<Dashboard />);

    expect(component).toMatchSnapshot();
});