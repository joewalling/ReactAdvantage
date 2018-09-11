import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import Sidebar from './index';

it('renders correctly', () => {
    const component = shallow(<Sidebar />);

    expect(component).toMatchSnapshot();
});