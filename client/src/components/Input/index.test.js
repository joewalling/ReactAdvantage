import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import Input from './index';

it('renders correctly', () => {
    const component = shallow(<Input />);

    expect(component).toMatchSnapshot();
});