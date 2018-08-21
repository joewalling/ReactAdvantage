import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import Dropdown from './index';

it('renders correctly', () => {
    const component = shallow(<Dropdown />);

    expect(component).toMatchSnapshot();
});
